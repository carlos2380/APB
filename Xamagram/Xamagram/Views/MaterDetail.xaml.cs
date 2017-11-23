using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamagram.Models;
using Xamagram.Services;
using Xamarin.Forms;

namespace Xamagram.Views
{
    public partial class MaterDetail : MasterDetailPage
    {
        Xamagram.Views.Master masterPage;
        public MaterDetail()
        {
           
            InitializeComponent();
            masterPage = new Xamagram.Views.Master(null);
            this.Master = masterPage;
            Detail = new CustomNavigation(new NewAvisoView(null));
            masterPage.ListView.ItemSelected += OnItemSelected;
        }
        public MaterDetail(object parameter)
        {
            
            InitializeComponent();
            masterPage = new Xamagram.Views.Master(null);
            this.Master = masterPage;
            Detail = new NavigationPage(new NewAvisoView(null)) { BarBackgroundColor = Color.Black, BarTextColor = Color.White }; 
            Parameter = parameter;
            masterPage.ListView.ItemSelected += OnItemSelected;
            NavigationService.MasterDetail = this;
        }
        public object Parameter { get; set; }
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterItem;
            if (item != null)
            {
                if (item.Title.Equals("Crear Aviso"))
                {
                    Detail = new NavigationPage(new NewAvisoView(null)) { BarBackgroundColor = Color.Black, BarTextColor = Color.White };
                    masterPage.ListView.SelectedItem = null;
                    IsPresented = false;
                }
                else if (item.Title.Equals("Avisos"))
                {
                    Detail = new NavigationPage(new AvisoView(null)) { BarBackgroundColor = Color.Black, BarTextColor = Color.White };
                    masterPage.ListView.SelectedItem = null;
                    IsPresented = false;
                }
            }
        }
    }
}