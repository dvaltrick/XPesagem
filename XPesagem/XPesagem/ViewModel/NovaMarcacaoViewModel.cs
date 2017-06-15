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
    public class NovaMarcacaoViewModel:BaseViewModel
    {
        public string _peso;
        public string peso {
            get {
                return _peso; 
            }
            set {
                _peso = value;
                OnPropertyChanged();
            }
        }

        public bool _IsBusy;
        public bool IsBusy {
            get {
                return _IsBusy;
            }
            set {
                _IsBusy = value;
                OnPropertyChanged();
            }
        }

        public DateTime _data;
        public DateTime data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        AzureSyncTable azureService;
        public ICommand salvarMarcacao { get;  }
        public FacebookUser fb;



        public NovaMarcacaoViewModel() {
            this.IsBusy = false;
            this.salvarMarcacao = new Command(executaSalvarMarcacao);

            this.data = DateTime.Today;
            this.peso = "0,00";

            fb = (FacebookUser)GetApplicationCurrentProperty("FacebookUser");
            
            azureService = DependencyService.Get<AzureSyncTable>();
        }

        public async void executaSalvarMarcacao() {
            this.IsBusy = true;
            Marcacao marcacao = new Marcacao();
            marcacao.Usuario = this.fb.Id;
            marcacao.Data = data;
            peso = peso.Replace(",", ".");
            marcacao.Peso = float.Parse(peso);

            await azureService.AddReg(marcacao);
            this.IsBusy = false;
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
