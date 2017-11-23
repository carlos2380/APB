using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Xamagram.Services;
using Xamagram.ViewModels;
using Xamarin.Forms;

namespace Xamagram.Views
{
    public partial class Master : ContentPage
    {
        public ListView ListView { get { return listView; } }
        public Master(object parameter)
        {
            InitializeComponent();

            Parameter = parameter;

            BindingContext = new MasterViewModel();
        }

        public object Parameter { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as MasterViewModel;
            if (viewModel != null) viewModel.OnAppearing(Parameter);
        }
    }
}