using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XPesagem.Interface;
using XPesagem.Model;
using XPesagem.Service;

namespace XPesagem.ViewModel
{
    public class DashboardViewModel:BaseViewModel, IPageAppearingEvent
    {
        public bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                OnPropertyChanged();
            }
        }

        AzureSyncTable azureService;
        public ICommand novaMarcacao { get;  }
        public ICommand atualizaLista { get; }

        public string usuario;
        public ObservableCollection<Marcacao> listaDePesos { get; } = new ObservableCollection<Marcacao>();
        FacebookUser fb = null;

        public DashboardViewModel() {
            this.novaMarcacao = new Command(AdicionaMarcacao);
            this.atualizaLista = new Command(ExecutaAtualizaLista);

            azureService = DependencyService.Get<AzureSyncTable>();

            carregaMarcacoesAsync();

            fb = (FacebookUser)GetApplicationCurrentProperty("FacebookUser");
            this.usuario = fb.Id;
        }

        public async void ExecutaAtualizaLista() {
            await carregaMarcacoesAsync();
        }

        public async void AdicionaMarcacao() {
            await PushAsync<NovaMarcacaoViewModel>();
        }

        public async Task carregaMarcacoesAsync()
        {
            this.IsBusy = true;
            var marcacoes = await azureService.GetReg();

            listaDePesos.Clear();
            foreach (Marcacao marca in marcacoes)
            {
                listaDePesos.Add(marca);
            }
            this.IsBusy = false;

        }

        public async void OnAppearing()
        {
            await carregaMarcacoesAsync();
        }

    }
    
}
