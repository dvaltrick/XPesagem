using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using XPesagem.Interface;
using XPesagem.UWP;

[assembly: Xamarin.Forms.Dependency(typeof(AuthenticateUWP))]
namespace XPesagem.UWP
{
    public class AuthenticateUWP : IAuthenticate
    {
        public async Task<MobileServiceUser> Authenticate(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            try
            {
                return await client.LoginAsync(provider);
            }
            catch (Exception e) {
                return null;
            }
        }
    }
}
