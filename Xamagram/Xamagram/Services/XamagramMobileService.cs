using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.SecureStorage;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xamagram.Models;
using Xamarin.Forms;

namespace Xamagram.Services
{
    public partial class XamagramMobileService
    {
        private MobileServiceClient _client;
        private IMobileServiceSyncTable<Aviso> _avisoTable;
        private static XamagramMobileService _instance;

        public static XamagramMobileService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new XamagramMobileService();
                }

                return _instance;
            }
        }

        public MobileServiceClient Client
        {
            get { return _client; }
        }

        public XamagramMobileService()
        {
            _client = new MobileServiceClient(GlobalSettings.AzureUrl);
        }

        private async Task InitializeAsync()
        {
            if ( _avisoTable != null)
                return;

            // Inicialización de SQLite local
            var store = new MobileServiceSQLiteStore(GlobalSettings.Database);
            store.DefineTable<Aviso>();

            await _client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
            _avisoTable = _client.GetSyncTable<Aviso>();
            // Limpiar registros offline.
            await _avisoTable.PurgeAsync(true);
        }

        
        public async Task<IEnumerable<Aviso>> ReadAvisosAsync()
        {
            await InitializeAsync();
            await SynchronizeAsync();
            return await _avisoTable.ToEnumerableAsync();
        }

       
        public async Task AddOrUpdateAvisoAsync(Aviso aviso)
        {
            await InitializeAsync();

            if (string.IsNullOrEmpty(aviso.Id))
            {
                await _avisoTable.InsertAsync(aviso);
            }
            else
            {
                await _avisoTable.UpdateAsync(aviso);
            }

            await SynchronizeAsync();
        }

        public async Task DeleteAvisoAsync(Aviso aviso)
        {
            await InitializeAsync();

            await _avisoTable.DeleteAsync(aviso);

            await SynchronizeAsync();
        }

        private async Task SynchronizeAsync()
        {
            if (!CrossConnectivity.Current.IsConnected)
                return;

            try
            {         
                // Subir cambios a la base de datos remota
                await _client.SyncContext.PushAsync();

                // El primer parámetro es el nombre de la query utilizada intermanente por el client SDK para implementar sync.
                // Utiliza uno diferente por cada query en la App
                await _avisoTable.PullAsync($"all{nameof(Aviso)}", _avisoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException ex)
            {
                if (ex.PushResult.Status == MobileServicePushStatus.CancelledByAuthenticationError)
                {
                    await LoginAsync();
                    await SynchronizeAsync();
                    return;
                }

                if (ex.PushResult != null)
                    foreach (var result in ex.PushResult.Errors)
                        await ResolveErrorAsync(result);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await LoginAsync();
                    await SynchronizeAsync();
                    return;
                }

                throw;
            }
        }

        public async Task LoginAsync()
        {
            const string userIdKey = ":UserId";
            const string tokenKey = ":Token";

            if (CrossSecureStorage.Current.HasKey(userIdKey)
                && CrossSecureStorage.Current.HasKey(tokenKey))
            {
                string userId = CrossSecureStorage.Current.GetValue(userIdKey);
                string token = CrossSecureStorage.Current.GetValue(tokenKey);

                _client.CurrentUser = new MobileServiceUser(userId)
                {
                    MobileServiceAuthenticationToken = token
                };

                return;
            }

            var authService = DependencyService.Get<IAuthService>();
            await authService.LoginAsync(_client, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);

            var user = _client.CurrentUser;

            if (user != null)
            {
                CrossSecureStorage.Current.SetValue(userIdKey, user.UserId);
                CrossSecureStorage.Current.SetValue(tokenKey, user.MobileServiceAuthenticationToken);
            }
        }

        private async Task ResolveErrorAsync(MobileServiceTableOperationError result)
        {
            // Ignoramos si no podemos validar ambas partes.
            if (result.Result == null || result.Item == null)
                return;
            

            var serverAviso = result.Result.ToObject<Aviso>();
            var localAviso = result.Item.ToObject<Aviso>();


            if (serverAviso.Id == localAviso.Id)
            {
                // Los elementos sin iguales, ignoramos el conflicto
                await result.CancelAndDiscardItemAsync();
            }
            else
            {
                // Para nosotros, gana el cliente
                localAviso.AzureVersion = serverAviso.AzureVersion;
                await result.UpdateOperationAsync(JObject.FromObject(localAviso));
            }
        }
    }
}
