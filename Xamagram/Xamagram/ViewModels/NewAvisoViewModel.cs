using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamagram.Models;
using Xamagram.Services;
using Xamagram.ViewModels.Base;
using Xamarin.Forms;

namespace Xamagram.ViewModels
{
    class NewAvisoViewModel : ViewModelBase
    {
        private string _id;
        private string _uriFotp;
        private string _tipo;
        private string _descripcion;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string UriFoto
        {
            get {
                if(_uriFotp == null)
                {
                    _uriFotp = "http://apbbacknet.azurewebsites.net/Imagenes/NewFoto.png";
                }else if (_uriFotp.Equals("http://apbbacknet.azurewebsites.net/Imagenes/NewFoto.png"))
                {
                    _uriFotp = "http://apbbacknet.azurewebsites.net/Imagenes/NoFoto.png";
                }
                return _uriFotp; }
            set
            {
                if (_uriFotp.Equals("http://apbbacknet.azurewebsites.net/Imagenes/NewFoto.png"))
                {
                    _uriFotp = "http://apbbacknet.azurewebsites.net/Imagenes/NoFoto.png";
                }
                _uriFotp = value;
                OnPropertyChanged("UriFoto");
            }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set
            {
                _descripcion = value;
                OnPropertyChanged("Descripcion");
            }
        }

        public string Tipo
        {
            get {
                if(_tipo == null)
                {
                    return "0";
                }
                return _tipo; }
            set
            {
                if(value.Equals("0"))
                {
                    _tipo = "Otro";
                }
                else if (value.Equals("1"))
                {
                    _tipo = "Pelea Domestica";
                }
                else if (value.Equals("2"))
                {
                    _tipo = "Robo";
                }
                else if (value.Equals("3"))
                {
                    _tipo = "Secuestro";
                }
                else if (value.Equals("4"))
                {
                    _tipo = "Tiroteo";
                }
                else if (value.Equals("5"))
                {
                    _tipo = "Asesinato";
                }
                else if (value.Equals("6"))
                {
                    _tipo = "Terrorismo";
                }
                else if (value.Equals("7"))
                {
                    _tipo = "Atropello y fuga";
                }
                else
                {
                    _tipo = "Otro";
                }
                OnPropertyChanged("Tipo");
            }
        }

        public ICommand CameraCommand => new Command(async () => await CameraAsync());

        public ICommand SaveCommand => new Command(async () => await SaveAsync());

        public ICommand CancelCommand => new Command(Cancel);

        public override void OnAppearing(object navigationContext)
        {
            if (navigationContext is Aviso)
            {
                var aviso = (Aviso)navigationContext;

                Id = aviso.Id;
                UriFoto = aviso.UriFoto;
                Tipo = aviso.Tipo;
                Descripcion = aviso.Descripcion;
            }

            base.OnAppearing(navigationContext);
        }

        private async Task CameraAsync()
        {
            MediaFile result = await PhotoService.Instance.TakePhotoAsync();

            if (!string.IsNullOrEmpty(result.Path))
            {
                UriFoto = await BlobService.Instance.UploadPhotoAsync(result);
            }
        }

        private async Task SaveAsync()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();

            var aviso = new Aviso
            {
                Id = this.Id,
                Tipo = this.Tipo,
                UriFoto = this.UriFoto,
                Descripcion = this.Descripcion,
                cerrado = false,
                lat = position.Latitude,
                lon = position.Longitude
            };

            await XamagramMobileService.Instance.
                AddOrUpdateAvisoAsync(aviso);


                Id = "";
                Tipo = "0";
                UriFoto = "";
                Descripcion = "";

            //NavigationService.Instance.NavigateBack();
        }

        private void Cancel()
        {
            NavigationService.Instance.NavigateBack();
        }
    }
}
