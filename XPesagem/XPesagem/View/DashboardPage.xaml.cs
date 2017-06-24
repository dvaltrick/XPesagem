using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XPesagem.Interface;
using XPesagem.ViewModel;

namespace XPesagem.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        DashboardViewModel vm = new DashboardViewModel();
        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = vm;
            
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var onAppearingLifeCycleEvents = BindingContext as IPageAppearingEvent;
            if (onAppearingLifeCycleEvents != null)
            {
                var lifecycleHandler = onAppearingLifeCycleEvents;

                base.Appearing += (object sender, EventArgs e) => lifecycleHandler.OnAppearing();
            }
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                vm.ExecutaAbreDetalhe(e.SelectedItem);
            }
        }
    }

}
