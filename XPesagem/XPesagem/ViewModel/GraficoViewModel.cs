using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPesagem.Model;

namespace XPesagem.ViewModel
{
    public class GraficoViewModel:BaseViewModel
    {
        public PlotModel _Model;
        public PlotModel Model
        {
            get
            {
                return _Model;
            }
            set
            {
                _Model = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Marcacao> ListaDePesos = null;

        public GraficoViewModel() {
            ListaDePesos = (ObservableCollection<Marcacao>)GetApplicationCurrentProperty("ListaPesos");

            montaGrafico(0);
        }

        public void montaGrafico(int TipoGrafico /*0 - Semana, 1 - Mês e 2 - Total*/) {
            int max = 1;
            float menorPeso, maiorPeso, primeiro;
            DateTime hoje = DateTime.Today;

            ObservableCollection<Marcacao> ListaDePesosOrdenado = new ObservableCollection<Marcacao>();
            List<string> listaData = new List<string>();
            
            var model = new PlotModel
            {
                TextColor = OxyColor.FromRgb(155, 89, 182)
            };
            
            var ColumnSeries = new ColumnSeries {
                FillColor = OxyColor.FromRgb(30, 139, 195),
                TextColor = OxyColor.FromRgb(30, 139, 195),
                StrokeColor = OxyColor.FromRgb(30, 139, 195),
                LabelFormatString = "{0:0.0}",
                LabelPlacement = LabelPlacement.Outside
            };

            var lineSeries = new LineSeries
            {
                Smooth = true,
                Color = OxyColor.FromRgb(118, 255, 3),
                LineStyle = LineStyle.Solid,
                //LabelFormatString = "{1:0.0}%",
                TextColor = OxyColor.FromRgb(118, 255, 3)
            };

            switch (TipoGrafico) {
                case 0:
                    max = 7;
                    break;
                case 1:
                    max = 30;
                    break;
                case 2:
                    max = ListaDePesos.Count;
                    break;
            }

            hoje = hoje.AddDays((max) * -1);

            primeiro = ListaDePesos[ListaDePesos.Count - 1].Peso;
            
            foreach (Marcacao marca in ListaDePesos.OrderBy(o => o.Peso)) {
                ListaDePesosOrdenado.Add(marca);
            }
            menorPeso = ListaDePesosOrdenado[0].Peso;
            maiorPeso = ListaDePesosOrdenado[ListaDePesosOrdenado.Count - 1].Peso;

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 80,
                Maximum = 100
            });

            int controle = 1;
            float posicao = 0;
            foreach (Marcacao marca in ListaDePesos.Where(marca => marca.Data >= hoje).OrderBy(o => o.Data) ) {
                listaData.Add(marca.Data.Day.ToString()+"/"+ marca.Data.Month.ToString() + "/"+ marca.Data.Year.ToString());
                posicao = calculaPosVariacao(menorPeso, maiorPeso, primeiro, marca.Peso);
                //posicao = ((marca.Peso / primeiro) - 1) * (-100);
                ColumnSeries.Items.Add(new ColumnItem(marca.Peso, controle));
                lineSeries.Points.Add(new DataPoint(controle, posicao));

                controle++;
            }

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Angle = 90,
                Minimum = 1,
                Maximum = max,
                Key = "Dias",
                ItemsSource = listaData
            });

            model.Series.Add(ColumnSeries);
            model.Series.Add(lineSeries);

            this.Model = model;


        }

        public float calculaPosVariacao(float menorPeso, float maiorPeso, 
                                        float primeiroPeso,float pesoAtual) {
            //float varPeso = (((pesoAtual / primeiroPeso) - 1) * (-1));
            //float varPos = menorPeso * (1+varPeso);//varPeso * ((maiorPeso+3) - (menorPeso-3)); 

            float varPeso = (((pesoAtual / primeiroPeso) - 1) ) * (-100);
            float varPos = ((varPeso/100)*20) + 80;//varPeso * ((maiorPeso+3) - (menorPeso-3)); 

            return varPos;
        }

    }
}
