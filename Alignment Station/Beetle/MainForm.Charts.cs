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
        private bool chartInitFlag = false;
        private SeriesCollection seriesXY, seriesZ, seriesA, seriesB, seriesC;
        private double[] oldPosition = { 0,0,0,0,0,0 };
        private byte weightValue = 12;
        private double oldLoss = -50;

        private void ChartsInit()
        {
            chartInitFlag = true;
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
            seriesXY[0].Values.Add(new ScatterPoint(x: bt.parameters.position[0]*1000, y: bt.parameters.position[1]*1000, weight: weightValue));
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

            bt.parameters.position.CopyTo(oldPosition, 0);

            seriesA = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Real", // TODO specify which motor
                    FontSize = 15,
                    //FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Values = new ChartValues<int>(),
                    StrokeThickness = 1,
                    Stroke = Brushes.LightPink,
                    Fill = Brushes.LightPink
                },
                new LineSeries
                {
                    Title = "Target", // TODO specify which motor
                    FontSize = 15,
                    //FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Values = new ChartValues<int>(),
                    PointGeometry = null,
                    StrokeThickness = 2,
                    Stroke = Brushes.DarkRed,
                    Fill = Brushes.Transparent,
                    StrokeDashArray = new DoubleCollection{2},
                }
            };
            cartesianChartMotorA.Series = seriesA;
            cartesianChartMotorA.LegendLocation = LegendLocation.Top;
            cartesianChartMotorA.AxisX.Add(
                new Axis
                {
                    Title = "Series",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700),
                }
            ) ;
            cartesianChartMotorA.AxisY.Add(
                new Axis
                {
                    Title = "Encoder Counts",
                    FontSize = 15,
                    FontWeight = FontWeight.FromOpenTypeWeight(700),
                }
            );

            seriesB = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Real", // TODo specify which motor
                    FontSize = 16,
                    //FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Values = new ChartValues<int>(),
                    StrokeThickness = 1,
                    Stroke = Brushes.LightGreen,
                    Fill = Brushes.LightGreen,
                },
                new LineSeries
                {
                    Title = "Target", // TODO specify which motor
                    FontSize = 15,
                    //FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Values = new ChartValues<int>(),
                    PointGeometry = null,
                    StrokeThickness = 2,
                    Stroke = Brushes.DarkRed,
                    Fill = Brushes.Transparent,
                    StrokeDashArray = new DoubleCollection{2},
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
                    Title = "Real", // TODo specify which motor
                    FontSize = 16,
                    //FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Values = new ChartValues<int>(),
                    StrokeThickness = 1,
                    Stroke = Brushes.LightBlue,
                    Fill = Brushes.LightBlue
                },
                new LineSeries
                {
                    Title = "Target", // TODO specify which motor
                    FontSize = 15,
                    //FontWeight = FontWeight.FromOpenTypeWeight(700),
                    Values = new ChartValues<int>(),
                    PointGeometry = null,
                    StrokeThickness = 2,
                    Stroke = Brushes.DarkRed,
                    Fill = Brushes.Transparent,
                    StrokeDashArray = new DoubleCollection{2},
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
            double x = Math.Round(bt.parameters.position[0], 4), y = Math.Round(bt.parameters.position[1], 4);
            if (oldPosition[2] != bt.parameters.position[2] || oldPosition[3] != bt.parameters.position[3] || oldPosition[4] != bt.parameters.position[4] || 
                oldPosition[5] != bt.parameters.position[5])
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
                if (PowerMeter.loss > oldLoss)
                    weightValue += 4;
                else if (PowerMeter.loss < oldLoss)
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
                oldLoss = PowerMeter.loss;
            }
            bt.parameters.position.CopyTo(oldPosition, 0);

            seriesZ[0].Values.Add(Math.Round(bt.parameters.position[2], 4));
            if (seriesZ[0].Values.Count > 20)
                seriesZ[0].Values.RemoveAt(0);
        }

        private void MotorChartsUpdate()
        {
            if (comboBoxMotorSelectTop.SelectedIndex != 6)
            {
                if (bt.beetleControl.onTargetFlag[comboBoxMotorSelectTop.SelectedIndex] == 1)
                    bt.beetleControl.RealCountsFetch((sbyte)comboBoxMotorSelectTop.SelectedIndex);
                seriesA[0].Values.Add(bt.beetleControl.countsReal[comboBoxMotorSelectTop.SelectedIndex]);
                seriesA[1].Values.Add(bt.beetleControl.countsTarget[comboBoxMotorSelectTop.SelectedIndex]);
                if (seriesA[0].Values.Count > 30)
                    seriesA[0].Values.RemoveAt(0);
                if (seriesA[1].Values.Count > 30)
                    seriesA[1].Values.RemoveAt(0);
            }

            if (comboBoxMotorSelectMid.SelectedIndex != 6)
            {
                if (bt.beetleControl.onTargetFlag[comboBoxMotorSelectMid.SelectedIndex] == 1)
                    bt.beetleControl.RealCountsFetch((sbyte)comboBoxMotorSelectMid.SelectedIndex);
                seriesB[0].Values.Add(bt.beetleControl.countsReal[comboBoxMotorSelectMid.SelectedIndex]);
                seriesB[1].Values.Add(bt.beetleControl.countsTarget[comboBoxMotorSelectMid.SelectedIndex]);
                if (seriesB[0].Values.Count > 30)
                    seriesB[0].Values.RemoveAt(0);
                if (seriesB[1].Values.Count > 30)
                    seriesB[1].Values.RemoveAt(0);
            }

            if (comboBoxMotorSelectBot.SelectedIndex != 6)
            {
                if (bt.beetleControl.onTargetFlag[comboBoxMotorSelectBot.SelectedIndex] == 1)
                    bt.beetleControl.RealCountsFetch((sbyte)comboBoxMotorSelectBot.SelectedIndex);
                seriesC[0].Values.Add(bt.beetleControl.countsReal[comboBoxMotorSelectBot.SelectedIndex]);
                seriesC[1].Values.Add(bt.beetleControl.countsTarget[comboBoxMotorSelectBot.SelectedIndex]);
                if (seriesC[0].Values.Count > 30)
                    seriesC[0].Values.RemoveAt(0);
                if (seriesC[1].Values.Count > 30)
                    seriesC[1].Values.RemoveAt(0);
            }
        }

        private void comboBoxMotorSelectTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMotorSelectTop.SelectedIndex != 6)
            {
                if (seriesA == null)
                    ChartsInit();
                seriesA[0].Values.Clear();
                seriesA[1].Values.Clear();
            }
        }

        private void comboBoxMotorSelectMid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMotorSelectMid.SelectedIndex != 6)
            {
                if (seriesB == null)
                    ChartsInit();
                seriesB[0].Values.Clear();
                seriesB[1].Values.Clear();
            }
        }

        private void comboBoxMotorSelectBot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMotorSelectBot.SelectedIndex != 6)
            {
                if (seriesC == null)
                    ChartsInit();
                seriesC[0].Values.Clear();
                seriesC[1].Values.Clear();
            }
        }

    }
}
