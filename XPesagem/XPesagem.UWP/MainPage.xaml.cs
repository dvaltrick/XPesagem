﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XPesagem.Interface;

namespace XPesagem.UWP
{
    public sealed partial class MainPage 
    {
        public MainPage()
        {
            this.InitializeComponent();

            OxyPlot.Xamarin.Forms.Platform.UWP.PlotViewRenderer.Init();

            LoadApplication(new XPesagem.App());

        }
        
    }
}
