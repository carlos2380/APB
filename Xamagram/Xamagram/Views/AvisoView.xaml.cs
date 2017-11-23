using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamagram.ViewModels;
using Xamarin.Forms;

namespace Xamagram.Views
{
    public partial class AvisoView : ContentPage
    {
        public AvisoView()
        {
            InitializeComponent();

            BindingContext = new AvisoViewModel();
        }
        public AvisoView(object parameter)
        {
            InitializeComponent();

            Parameter = parameter;

            BindingContext = new AvisoViewModel();
        }

        public object Parameter { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as AvisoViewModel;
            if (viewModel != null) viewModel.OnAppearing(Parameter);
        }
    }
}
