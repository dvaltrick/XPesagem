using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XPesagem.Model;
using XPesagem.Service;

namespace XPesagem.ViewModel
{
    public class DetalhePesoViewModel:BaseViewModel
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

        public string _peso;
        public string peso
        {
            get
            {
                return _peso;
            }
            set
            {
                _peso = value;
                OnPropertyChanged();
            }
        }

        public string _data;
        public string data
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

        public string _variacaoDia;
        public string variacaoDia
        {
            get
            {
                return _variacaoDia;
            }
            set
            {
                _variacaoDia = value;
                OnPropertyChanged();
            }
        }

        public string _variacaoTotal;
        public string variacaoTotal
        {
            get
            {
                return _variacaoTotal;
            }
            set
            {
                _variacaoTotal = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Marcacao> ListaDePesos = null;
        public List<Marcacao> listaResult = null;

        public ObservableCollection<Alimentacao> listaDieta { get; } = new ObservableCollection<Alimentacao>();

        AzureSyncTableAlimentacao azureService;

        public DetalhePesoViewModel(Marcacao selecionado) {
            this.data = "Dia: " + selecionado.Data.Day.ToString() +"/" +
                                  selecionado.Data.Month.ToString() + "/" +
                                  selecionado.Data.Year.ToString();
            this.peso = "Peso: " + selecionado.Peso.ToString() + "Kg";

            ListaDePesos = (ObservableCollection<Marcacao>)GetApplicationCurrentProperty("ListaPesos");

            float fVariacaoTotal = 0.00f;
            float fVariacaoDia = 0.00f;

            fVariacaoTotal = ((selecionado.Peso/ListaDePesos[ListaDePesos.Count-1].Peso)-1)*(-100);

            try
            {
                fVariacaoDia = ((selecionado.Peso / ListaDePesos[ListaDePesos.IndexOf(selecionado) + 1].Peso) - 1) * (-100);
            }
            catch (Exception e){
            }

            this.variacaoTotal = "Variação Total: " + Math.Round(fVariacaoTotal,2).ToString() + "%";
            this.variacaoDia = "Variação Dia: " + Math.Round(fVariacaoDia,2).ToString() + "%";

            azureService = DependencyService.Get<AzureSyncTableAlimentacao>();

            carregaAlimentacaoDia(selecionado.Data);
        }

        public async Task carregaAlimentacaoDia(DateTime dataFiltro)
        {
            this.IsBusy = true;
            var dieta = await azureService.GetReg(dataFiltro);

            listaDieta.Clear();
            foreach (Alimentacao item in dieta)
            {
                listaDieta.Add(item);
            }

            this.IsBusy = false;
        }
    }
}
