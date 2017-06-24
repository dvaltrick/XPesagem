using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XPesagem.Model;
using XPesagem.ViewModel;

namespace XPesagem.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalhePesoPage : ContentPage
    {
        private Marcacao selecionado;

        public DetalhePesoPage(Marcacao selecionado)
        {
            InitializeComponent();
            BindingContext = new DetalhePesoViewModel(selecionado);
        }
    }
}
