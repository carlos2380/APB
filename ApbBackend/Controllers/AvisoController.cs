using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using ApbBackend.DataObjects;
using ApbBackend.Models;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.NotificationHubs;
using System.Collections.Generic;

namespace ApbBackend.Controllers
{
    [Authorize]
    public class AvisoController : TableController<Aviso>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Aviso>(context, Request);
        }

        // GET tables/Aviso
        public IQueryable<Aviso> GetAllAviso()
        {
            return Query(); 
        }

        // GET tables/Aviso/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Aviso> GetAviso(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Aviso/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Aviso> PatchAviso(string id, Delta<Aviso> patch)
        {
             return UpdateAsync(id, patch);
        }
        [AllowAnonymous]
        // POST tables/Aviso
        public async Task<IHttpActionResult> PostAviso(Aviso item)
        {
            Aviso current = await InsertAsync(item);
            // Obtenemos la configuración del proyecto del servidor
            HttpConfiguration config = this.Configuration;
            MobileAppSettingsDictionary settings =
            this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            // Obtenemos las credenciales del Notification Hubs de la Mobile App.
            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings
            .Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            // Creamos un nuevo cliente Notification Hub.
            NotificationHubClient hub = NotificationHubClient
            .CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

            // Enviamos el mensaje.
            // Esto incluye APNS, GCM, WNS, y MPNS.
            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = "Nuevo aviso de tipo " + item.Tipo + ".";

            try
            {
                // Enviamos la notificacion y obenemos el resultado
                var result = await hub.SendTemplateNotificationAsync(templateParams);

                // Escribimos el resultado en los logs.
                config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (System.Exception ex)
            {
                // Escribimos el fallo en los logs.
                config.Services.GetTraceWriter()
                .Error(ex.Message, null, "Push.SendAsync Error");
            }
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Aviso/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteAviso(string id)
        {
             return DeleteAsync(id);
        }
    }
}
