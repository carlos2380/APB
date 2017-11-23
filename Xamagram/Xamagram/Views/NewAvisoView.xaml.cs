using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamagram.ViewModels;
using Xamarin.Forms;

namespace Xamagram.Views
{
    public partial class NewAvisoView : ContentPage
    {
        public NewAvisoView()
        {
            InitializeComponent();

            BindingContext = new NewAvisoViewModel();
        }

        public NewAvisoView(object parameter)
        {
            InitializeComponent();

            Parameter = parameter;

            BindingContext = new NewAvisoViewModel();
        }

        public object Parameter { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as NewAvisoViewModel;
            if (viewModel != null) viewModel.OnAppearing(Parameter);
        }
    }
}
