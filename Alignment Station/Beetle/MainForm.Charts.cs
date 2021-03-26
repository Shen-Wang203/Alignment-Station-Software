using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Windows;
using System.Windows.Media;
using System;

namespace Beetle
{
    public partial class MainForm
    {
        private SeriesCollection seriesXY, seriesZ, seriesA, seriesB, seriesC;
        private double[] oldPosition = { 0,0,0,0,0,0 };
        private byte weightValue = 12;
        private double oldLoss = -50;

        private void ChartsInit()
        {
            seriesXY = new SeriesCollection
            {
                new ScatterSeries
                {
                    Title = "Real Time XY",
                    FontSize = 20,
                    FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Foreground = Brushes.Silver,
                    Values = new ChartValues<ScatterPoint>(),
                    //PointGeometry = DefaultGeometries.Diamond,
                    StrokeThickness = 5,
                    //Fill = new SolidColorBrush(Color.FromRgb(redness, 46, 49)),
                    Fill = Brushes.OrangeRed,
                    Stroke = Brushes.RosyBrown
                },
            };
            seriesXY[0].Values.Add(new ScatterPoint(x: Parameters.position[0]*1000, y: Parameters.position[1]*1000, weight: weightValue));
            cartesianChartXY.Series = seriesXY;
            cartesianChartXY.LegendLocation = LegendLocation.Top;
            cartesianChartXY.BackColor = System.Drawing.Color.Transparent;
            cartesianChartXY.AxisX.Add(
                new Axis
                {
                    Title = "X Position (um)",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700),
                    MinValue = -2,
                    MaxValue = 2
                }
            );
            cartesianChartXY.AxisY.Add(
                new Axis
                {
                    Title = "Y Position (um)",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700),
                    MaxValue = 2,
                    MinValue = -2
                }
            );


            seriesZ = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Real Time Z",
                    FontSize = 20,
                    FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Foreground = Brushes.Silver,
                    Values = new ChartValues<double>(),
                    StrokeThickness = 2,
                    Stroke = Brushes.LightSkyBlue,
                    PointGeometrySize = 10
                }
            };
            cartesianChartZ.Series = seriesZ;
            cartesianChartZ.LegendLocation = LegendLocation.Top;
            cartesianChartZ.AxisX.Add(
                new Axis
                {
                    Title = "Counts",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700)
                }
            ); ;
            cartesianChartZ.AxisY.Add(
                new Axis
                {
                    Title = "Z Position (mm)",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700)
                }
            );

            Parameters.position.CopyTo(oldPosition, 0);

            seriesA = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Real Time Motor Position", // TODo specify which motor
                    FontSize = 20,
                    //FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Values = new ChartValues<double>(),
                    StrokeThickness = 2,
                    Stroke = Brushes.LightPink,
                    PointGeometrySize = 10
                }
            };
            cartesianChartMotorA.Series = seriesA;
            cartesianChartMotorA.LegendLocation = LegendLocation.Top;
            cartesianChartMotorA.AxisX.Add(
                new Axis
                {
                    Title = "Series",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700)
                }
            ); ;
            cartesianChartMotorA.AxisY.Add(
                new Axis
                {
                    Title = "Encoder Counts",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700)
                }
            );

            seriesB = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Real Time Motor Position", // TODo specify which motor
                    FontSize = 20,
                    //FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Values = new ChartValues<double>(),
                    StrokeThickness = 2,
                    Stroke = Brushes.LightPink,
                    PointGeometrySize = 10
                }
            };
            cartesianChartMotorB.Series = seriesB;
            cartesianChartMotorB.LegendLocation = LegendLocation.Top;
            cartesianChartMotorB.AxisX.Add(
                new Axis
                {
                    Title = "Series",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700)
                }
            ); ;
            cartesianChartMotorB.AxisY.Add(
                new Axis
                {
                    Title = "Encoder Counts",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700)
                }
            );

            seriesC = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Real Time Motor Position", // TODo specify which motor
                    FontSize = 20,
                    //FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Values = new ChartValues<double>(),
                    StrokeThickness = 2,
                    Stroke = Brushes.LightPink,
                    PointGeometrySize = 10
                }
            };
            cartesianChartMotorC.Series = seriesC;
            cartesianChartMotorC.LegendLocation = LegendLocation.Top;
            cartesianChartMotorC.AxisX.Add(
                new Axis
                {
                    Title = "Series",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700)
                }
            ); ;
            cartesianChartMotorC.AxisY.Add(
                new Axis
                {
                    Title = "Encoder Counts",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700)
                }
            );
        }

        private void ChartsDataUpdate()
        {
            double x = Parameters.position[0], y = Parameters.position[1];
            if (oldPosition[2] != Parameters.position[2] || oldPosition[3] != Parameters.position[3] || oldPosition[4] != Parameters.position[4] || 
                oldPosition[5] != Parameters.position[5])
            {
                if (seriesXY[0].Values.Count > 1)
                {
                    weightValue = 8;
                    seriesXY[0].Values.Clear();
                    seriesXY[0].Values.Add(new ScatterPoint(x: x * 1000, y: y * 1000, weight: weightValue));

                    cartesianChartXY.AxisX[0].MaxValue = x * 1000 >= 0 ? (int)(x * 1000) + 1 : 0.1;
                    cartesianChartXY.AxisX[0].MinValue = x * 1000 < 0 ? (int)(x * 1000) - 1 : -0.1;
                    cartesianChartXY.AxisY[0].MaxValue = y * 1000 >= 0 ? (int)(y * 1000) + 1 : 0.1;
                    cartesianChartXY.AxisY[0].MinValue = y * 1000 < 0 ? (int)(y * 1000) - 1 : -0.1;
                }
            }
            else if (oldPosition[0] != x || oldPosition[1] != y)
            {
                if (Parameters.loss > oldLoss)
                    weightValue += 4;
                else if (Parameters.loss < oldLoss)
                    weightValue -= 4;

                // Adjust axial range
                if (x * 1000 < cartesianChartXY.AxisX[0].MinValue)
                    cartesianChartXY.AxisX[0].MinValue = (int)(x * 1000) - 1;
                else if (x * 1000 > cartesianChartXY.AxisX[0].MaxValue)
                    cartesianChartXY.AxisX[0].MaxValue = (int)(x * 1000) + 1;
                
                if (y * 1000 < cartesianChartXY.AxisY[0].MinValue)
                    cartesianChartXY.AxisY[0].MinValue = (int)(y * 1000) - 1;
                else if (y * 1000 > cartesianChartXY.AxisY[0].MaxValue)
                    cartesianChartXY.AxisY[0].MaxValue = (int)(y * 1000) + 1;

                seriesXY[0].Values.Add(new ScatterPoint(x: x * 1000, y: y * 1000, weight: weightValue));
                oldLoss = Parameters.loss;
            }
            Parameters.position.CopyTo(oldPosition, 0);

            seriesZ[0].Values.Add(Parameters.position[2]);
            if (seriesZ[0].Values.Count > 20)
                seriesZ[0].Values.RemoveAt(0);
        }

        private void MotorChartsUpdate()
        { 
            
        }

    }
}
