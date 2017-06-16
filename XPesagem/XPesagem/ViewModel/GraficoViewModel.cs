using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public GraficoViewModel() {
            var model = new PlotModel {
                TextColor = OxyColor.FromRgb(155, 89, 182)
            };

            var lineSeries = new LineSeries {
                Smooth = true,
                Color = OxyColor.FromRgb(30, 139, 195),
                LineStyle = LineStyle.None
            };

            model.Axes.Add(new LinearAxis {
                Position = AxisPosition.Bottom,
                Minimum = 1,
                Maximum = 7,
                TicklineColor = OxyColor.FromRgb(155, 89, 182)
            });
            model.Axes.Add(new LinearAxis {
                Position = AxisPosition.Left,
                Minimum = 60,
                Maximum = 120 });

            lineSeries.Points.Add(new DataPoint(1, 90));
            lineSeries.Points.Add(new DataPoint(2, 92));
            lineSeries.Points.Add(new DataPoint(3, 91));
            lineSeries.Points.Add(new DataPoint(4, 90));
            lineSeries.Points.Add(new DataPoint(5, 95));
            lineSeries.Points.Add(new DataPoint(6, 92));
            lineSeries.Points.Add(new DataPoint(7, 90));
            model.Series.Add(lineSeries);

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "Dias",
                ItemsSource = new[]
                    {
                            "15/06/2017",
                            "15/06/2017",
                            "15/06/2017",
                            "15/06/2017",
                            "15/06/2017",
                            "15/06/2017",
                            "15/06/2017"
                        }
            });

            this.Model = model;
        }

        public static PlotModel BarSeries()
        {
            var model = new PlotModel { Title = "Cake Type Popularity" };
            //generate a random percentage distribution between the 5
            //cake-types (see axis below)
            var rand = new Random();

            double[] cakePopularity = new double[5];
            for (int i = 0; i < 5; ++i)
            {
                cakePopularity[i] = rand.NextDouble();
            }

            var sum = cakePopularity.Sum();
            var barSeries = new BarSeries
            {
                ItemsSource = new List<BarItem>(new[]
                    {
                            new BarItem{ Value = (cakePopularity[0] / sum * 100),
                                         Color = OxyColor.FromRgb(155,89,182) },
                            new BarItem{ Value = (cakePopularity[1] / sum * 100) },
                            new BarItem{ Value = (cakePopularity[2] / sum * 100) },                       
                            new BarItem{ Value = (cakePopularity[3] / sum * 100) },
                            new BarItem{ Value = (cakePopularity[4] / sum * 100) }
                        }),

                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:.00}%"
            };

            model.Series.Add(barSeries);
            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "CakeAxis",
                ItemsSource = new[]
                    {
                            "Apple cake",
                            "Baumkuchen",
                            "Bundt Cake",
                            "Chocolate cake",
                            "Carrot cake"
                        }
            });

            return model;
        }
        
    }
}
