using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherDataParser;

namespace WeatherAnalitycs.Utility
{
    /// <summary>
    /// Логика взаимодействия для Rose.xaml
    /// </summary>
    public partial class Rose : UserControl
    {
        double increase = 900;
        double centerX, centerY;
        List<Dictionary<decimal, decimal>> dataList;
        Dictionary<Dictionary<decimal, decimal>, SolidColorBrush> list;
        double zoom = 1;
        double circleSizeDifference = 0.02;
        Dictionary<double, string> AngleLetterDirections = new()
        {
            { 0, "С" },
            { 22.5, "CСВ" },
            { 45, "CВ" },
            { 67.5, "CВВ" },
            { 90, "В" },
            { 112.5, "ЮВВ" },
            { 135, "ЮВ" },
            { 157.5, "ЮЮВ" },
            { 180, "Ю" },
            { 202.5, "ЮЮЗ" },
            { 225, "ЮЗ" },
            { 247.5, "ЮЗЗ" },
            { 270, "З" },
            { 292.5, "СЗЗ" },
            { 315, "СЗ" },
            { 337.5, "СЗЗ" },
        };

        SolidColorBrush[] colors =
        {
            Brushes.Blue, Brushes.Purple, Brushes.Orange, Brushes.Pink, Brushes.Yellow, Brushes.Lime, Brushes.LightGray
        };
        int numberOfDirections;

        public Rose(List<Dictionary<decimal, decimal>> data)
        {
            InitializeComponent();
            dataList = data;
            numberOfDirections = dataList[0].Keys.Count;
            list = new();
            for (int i = 0; i < dataList.Count; i++)
            {
                list.Add(dataList[i], colors[i]);
            }
            Update();
        }

        void Redraw()
        {
            double maxValue = Math.Ceiling((double)dataList[0].Values.Max() / circleSizeDifference) * circleSizeDifference;
            for (double r = maxValue; r >= 0; r -= circleSizeDifference)
            {
                for (double angle = 0; angle < 360; angle += numberOfDirections == 8 ? 45 : 22.5)
                {
                    canvas.Children.Add(DrawSegment(angle, r, Brushes.White, Brushes.LightGray, false));
                }
                TextBlock value = new()
                {
                    Text = Math.Round(r, 2).ToString(),
                    Background = new SolidColorBrush(Colors.White),
                    Foreground = Brushes.Gray,
                    FontSize = 12 + Math.Round(zoom),
                    Margin = new Thickness()
                    {
                        Left = canvas.Width - (centerX + (r + 0.005) * increase),
                        Top = centerY
                    }
                };
                canvas.Children.Add(value);
            }

            foreach (var item in list)
            {
                foreach (var entry in item.Key)
                {
                    canvas.Children.Add(DrawSegment((double)entry.Key, (double)entry.Value, item.Value, Brushes.Black, true));
                }
            }
            for (double angle = 0; angle < 360; angle += numberOfDirections == 8 ? 45 : 22.5)
            {
                double r = (maxValue + 0.02) * increase;
                double radAngle = (angle - 90) * (Math.PI / 180);
                TextBlock direction = new()
                {
                    Text = AngleLetterDirections[angle],
                    Foreground = Brushes.Gray,
                    FontSize = 16 + Math.Round(zoom),
                    Margin = new Thickness()
                    {
                        Left = centerX + (r * Math.Cos(radAngle)) - 4 * zoom,
                        Top = centerY + (r * Math.Sin(radAngle)) - 4 * zoom,
                    },
                    TextAlignment = TextAlignment.Center,
                };
                canvas.Children.Add(direction);
            }
        }

        Path DrawSegment(double angle, double length, SolidColorBrush fillColor, SolidColorBrush outlineColor, bool writeToolTip)
        {
            double diff = numberOfDirections == 8 ? 22.5 : 11.25;
            double r = length * increase;

            double startAngle = (angle + diff-90) * (Math.PI / 180);
            double endAngle = (angle - diff-90) * (Math.PI / 180);

            Path path = new()
            {
                Stroke = outlineColor,
                StrokeThickness = 1,
                Fill = fillColor,
                Data = new PathGeometry()
                {
                    Figures =
                    {
                        new PathFigure()
                        {
                            StartPoint = new Point(centerX, centerY),
                            IsClosed = true,
                            Segments =
                            {
                                new LineSegment()
                                {
                                    Point = new Point()
                                    {
                                        X = centerX + (r * Math.Cos(startAngle)),
                                        Y = centerY + (r * Math.Sin(startAngle)),
                                    }
                                },
                                new ArcSegment()
                                {
                                    Point = new Point()
                                    {
                                        X = centerX + (r * Math.Cos(endAngle)),
                                        Y = centerY + (r * Math.Sin(endAngle)),
                                    },
                                    Size = new Size(r,r)
                                },
                                new LineSegment()
                                {
                                    Point = new Point(centerX, centerY)
                                }
                            },
                        }
                    }
                }
            };
            if (writeToolTip)
                path.ToolTip = $"Направление {AngleLetterDirections[angle]} \n Значение {length}";
            return path;
        }

        void Update()
        {
            canvas.Width = canvas.Height = Math.Min(ActualHeight, ActualWidth);
            centerX = canvas.Width / 2;
            centerY = canvas.Height / 2;
            increase = centerX * 3 * zoom;
            canvas.Children.Clear();
            Redraw();
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            zoom += Math.Sign(e.Delta) * 0.1;
            if (zoom <= 0.5)
                zoom = 0.5;
            Update();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Update();
        }
    }
}
