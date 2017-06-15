using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XPesagem.Model;

namespace XPesagem.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Executa chamada de nova página de forma genérica para evitar chamada direta das propriedades da
        //aplicação pela ViewModel
        public async Task PushAsync<TViewModel>(params object[] args) where TViewModel : BaseViewModel
        {
            var viewModelType = typeof(TViewModel);

            var viewModelTypeName = viewModelType.Name;
            var viewModelWordLength = "ViewModel".Length;
            var viewTypeName = $"XPesagem.View.{viewModelTypeName.Substring(0, viewModelTypeName.Length - viewModelWordLength)}Page";

            var viewType = Type.GetType(viewTypeName);

            var page = Activator.CreateInstance(viewType) as Page;

            var viewModel = Activator.CreateInstance(viewModelType, args);
            if (page != null) {
                page.BindingContext = viewModel;
            }

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        //Busca propriedade salva
        public static object GetApplicationCurrentProperty(string propertyKey) {
            object retorno = null;

            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey(propertyKey)) {
                retorno = properties[propertyKey];
            }

            return retorno;
        }

        //Insere propriedade para ser acionada de forma global em qualquer parte do código
        public static void SetApplicationCurrentProperty(string propertyKey, object obj) {
            IDictionary<string, object> properties = Application.Current.Properties;

            if (properties.ContainsKey(propertyKey))
            {
                properties[propertyKey] = obj;
            }
            else {
                properties.Add(propertyKey, obj);
            }
        }
        
    }
}
