using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XPesagem.Model;
using XPesagem.Service;

namespace XPesagem.ViewModel
{
    public class CadAlimentacaoViewModel:BaseViewModel
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
                carregaAlimentacaoDia();
                OnPropertyChanged();
            }
        }

        public TimeSpan _hora;
        public TimeSpan hora
        {
            get
            {
                return _hora;
            }
            set
            {
                _hora = value;
                OnPropertyChanged();
            }
        }

        public string _item;
        public string item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }

        AzureSyncTableAlimentacao azureService;
        public ICommand salvarItem { get; }
        public FacebookUser fb;

        public ObservableCollection<Alimentacao> listaAlimentacao { get; } = new ObservableCollection<Alimentacao>();

        public CadAlimentacaoViewModel()
        {
            this.IsBusy = false;
            this.salvarItem = new Command(executaSalvarMarcacao);

            this.data = DateTime.Today;
            this.hora = DateTime.Now.TimeOfDay;
            
            this.item = "";

            fb = (FacebookUser)GetApplicationCurrentProperty("FacebookUser");

            azureService = DependencyService.Get<AzureSyncTableAlimentacao>();

            carregaAlimentacaoDia();
        }

        public async Task carregaAlimentacaoDia()
        {
            this.IsBusy = true;
            var dieta = await azureService.GetReg(this.data);

            listaAlimentacao.Clear();
            foreach (Alimentacao item in dieta)
            {
                listaAlimentacao.Add(item);
            }

            this.IsBusy = false;
        }

        public async void executaSalvarMarcacao()
        {
            this.IsBusy = true;

            Alimentacao caditem = new Alimentacao();
            caditem.Usuario = this.fb.Id;
            caditem.Data = new DateTime(data.Year, data.Month, data.Day, 
                                        hora.Hours, hora.Minutes, 0);
            caditem.Descricao = item;

            await azureService.AddReg(caditem);

            this.data = DateTime.Today;
            this.hora = DateTime.Today.TimeOfDay; 
            this.item = "";

            await carregaAlimentacaoDia();
            this.IsBusy = false;
        }
    }
}
