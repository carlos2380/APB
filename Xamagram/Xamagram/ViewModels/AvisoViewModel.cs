using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamagram.Models;
using Xamagram.Services;
using Xamagram.ViewModels.Base;
using Xamarin.Forms;

namespace Xamagram.ViewModels
{
    class AvisoViewModel : ViewModelBase
    {
        private ObservableCollection<Aviso> _items;
        private Aviso _selectedItem;
        private bool _isBusy;

        public ObservableCollection<Aviso> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        public Aviso SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                // Creando un servicio de navegación
                NavigationService.Instance.NavigateTo<AvisoDetailViewModel>(_selectedItem);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;

                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public ICommand RefreshCommand => new Command(async () => await RefreshAsync());


        public override async void OnAppearing(object navigationContext)
        {
            base.OnAppearing(navigationContext);
            await LoadAvisoAsync();
        }

        private async Task RefreshAsync()
        {
            await LoadAvisoAsync();
        }
        

        private async Task LoadAvisoAsync()
        {
            IsBusy = true;

            var result = await XamagramMobileService.Instance.ReadAvisosAsync();

            if (result != null)
            {
                Items = new ObservableCollection<Aviso>(result);
            }

            IsBusy = false;
        }
    }
}
