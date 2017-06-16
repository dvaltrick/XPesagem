using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XPesagem.ViewModel;

namespace XPesagem.View
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraficoPage : ContentPage
    {
        public GraficoPage()
        {
            InitializeComponent();
            BindingContext = new GraficoViewModel();
        }
    }
}
