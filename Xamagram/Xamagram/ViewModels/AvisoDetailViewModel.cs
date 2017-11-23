using System;
using System.Collections.Generic;
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
    class AvisoDetailViewModel : ViewModelBase
    {
        private Aviso _item;

        public Aviso Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged("Item");
            }
        }
        
        public ICommand CompleteCommand => new Command(async () => await CompleteAsync());

        public override void OnAppearing(object navigationContext)
        {
            base.OnAppearing(navigationContext);

            if (navigationContext is Aviso)
            {
                Item = (Aviso)navigationContext;
            }
        }

        private async Task CompleteAsync()
        {
            if (Item.Id != null)
            {
                Item.cerrado = true;
                await XamagramMobileService.Instance.AddOrUpdateAvisoAsync(Item);

                NavigationService.Instance.NavigateBack();
            }
        }
    }
}
