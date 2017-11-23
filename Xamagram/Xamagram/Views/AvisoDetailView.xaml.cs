using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamagram.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;


namespace Xamagram.Views
{
    public partial class AvisoDetailView : ContentPage
    {
        AvisoDetailViewModel avisoDetailVM;
        public AvisoDetailView()
        {
            InitializeComponent();

            BindingContext = new AvisoDetailViewModel();
        }

        public AvisoDetailView(object parameter)
        {
            InitializeComponent();
            Parameter = parameter;

            BindingContext = new AvisoDetailViewModel();
            
        }

        public object Parameter { get; set; }

        AvisoDetailViewModel viewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            AvisoDetailViewModel viewModel = BindingContext as AvisoDetailViewModel;
            if (viewModel != null) viewModel.OnAppearing(Parameter);
            initPosition();
        }

        private async void initPosition()
        {
            var latitud = (BindingContext as AvisoDetailViewModel).Item.lat;
            var longitud = (BindingContext as AvisoDetailViewModel).Item.lon;
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitud, longitud), Distance.FromMiles(1)));


            //Añadir puntero en el mapa
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(latitud, longitud),
                Label = "Posicion Actual",
                Address = "Detalle"
            };

            MyMap.Pins.Add(pin);
        }
    }
}