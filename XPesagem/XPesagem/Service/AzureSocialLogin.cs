using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XPesagem.Interface;

namespace XPesagem.Service
{
    public class AzureSocialLogin
    {
        MobileServiceClient Client { get; set; } = null;
        
        public void Initialize()
        {
            string MyAppServiceURL = "https://xpesagem.azurewebsites.net/";
            Client = new MobileServiceClient(MyAppServiceURL);
        }

        public async Task<MobileServiceUser> LoginAsync()
        {
            Initialize();
            var auth = DependencyService.Get<IAuthenticate>();
            var user = await auth.Authenticate(Client, MobileServiceAuthenticationProvider.Facebook);

            if (user == null)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Ops!", "Não conseguimos conectar a sua conta no facebook", "OK");
                });
            }

            return user;
        }

    }
}
