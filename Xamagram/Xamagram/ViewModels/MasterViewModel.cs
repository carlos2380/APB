using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamagram.Models;
using Xamagram.ViewModels.Base;
using Xamagram.Views;

namespace Xamagram.ViewModels
{
    class MasterViewModel : ViewModelBase
    {
        public List<MasterItem> Items { get { return _items; } }
        private List<MasterItem> _items;
        public MasterViewModel()
        {
            var masterPageItems = new List<MasterItem>();
            masterPageItems.Add(new MasterItem
            {
                Title = "Crear Aviso",
                IconSource = "ic_fiber_new_white_48dp.png",
                TargetType = typeof(NewAvisoView)
            });

            masterPageItems.Add(new MasterItem
            {
                Title = "Avisos",
                IconSource = "ic_new_releases_white_48dp.png",
                TargetType = typeof(AvisoView)
            });

            _items = masterPageItems;
            /*  masterPageItems.Add(new MasterPageItem
              {
                  Title = "TodoList",
                  IconSource = "todo.png",
                  TargetType = typeof(TodoListPage)
              });
              masterPageItems.Add(new MasterPageItem
              {
                  Title = "Reminders",
                  IconSource = "reminders.png",
                  TargetType = typeof(ReminderPage)
              });*/
        }
    }
}
