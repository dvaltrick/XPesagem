using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XPesagem.Model;
using XPesagem.Service;

namespace XPesagem.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        readonly AzureSocialLogin azureLogin = new AzureSocialLogin();
        public ICommand commandLogin { get; }

        public MainViewModel() {
            this.commandLogin = new Command(LoginFacebook);
        }

        public async void LoginFacebook() {
            var user = await azureLogin.LoginAsync();

            FacebookUser fb = new FacebookUser();
            fb.Id = user.UserId;

            SetApplicationCurrentProperty("FacebookUser", fb);
            carregaModelo();
        }

        public async void carregaModelo() {
            await PushAsync<DashboardViewModel>();
        }
    }
}
