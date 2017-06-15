﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XPesagem.ViewModel;

namespace XPesagem.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NovaMarcacaoPage : ContentPage
    {
        public NovaMarcacaoPage()
        {
            InitializeComponent();
            BindingContext = new NovaMarcacaoViewModel();
        }
    }
}
