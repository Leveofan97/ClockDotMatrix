using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClockDotMatrix
{
    public partial class MainPage : ContentPage
    {
        // Всего точек по горизонтали и вертикали.
        const int horzDots = 41;
        const int vertDots = 7;

        // Шаблоны точечной матрицы 5 x 7 от 0 до 9.
        static readonly int[,,] numberPatterns = new int[10, 7, 5]
        {
            {
                { 0, 1, 1, 1, 0}, { 1, 0, 0, 0, 1}, { 1, 0, 0, 1, 1}, { 1, 0, 1, 0, 1},
                { 1, 1, 0, 0, 1}, { 1, 0, 0, 0, 1}, { 0, 1, 1, 1, 0}
            },
            {
                { 0, 0, 1, 0, 0}, { 0, 1, 1, 0, 0}, { 0, 0, 1, 0, 0}, { 0, 0, 1, 0, 0},
                { 0, 0, 1, 0, 0}, { 0, 0, 1, 0, 0}, { 0, 1, 1, 1, 0}
            },
            {
                { 0, 1, 1, 1, 0}, { 1, 0, 0, 0, 1}, { 0, 0, 0, 0, 1}, { 0, 0, 0, 1, 0},
                { 0, 0, 1, 0, 0}, { 0, 1, 0, 0, 0}, { 1, 1, 1, 1, 1}
            },
            {
                { 1, 1, 1, 1, 1}, { 0, 0, 0, 1, 0}, { 0, 0, 1, 0, 0}, { 0, 0, 0, 1, 0},
                { 0, 0, 0, 0, 1}, { 1, 0, 0, 0, 1}, { 0, 1, 1, 1, 0}
            },
            {
                { 0, 0, 0, 1, 0}, { 0, 0, 1, 1, 0}, { 0, 1, 0, 1, 0}, { 1, 0, 0, 1, 0},
                { 1, 1, 1, 1, 1}, { 0, 0, 0, 1, 0}, { 0, 0, 0, 1, 0}
            },
            {
                { 1, 1, 1, 1, 1}, { 1, 0, 0, 0, 0}, { 1, 1, 1, 1, 0}, { 0, 0, 0, 0, 1},
                { 0, 0, 0, 0, 1}, { 1, 0, 0, 0, 1}, { 0, 1, 1, 1, 0}
            },
            {
                { 0, 0, 1, 1, 0}, { 0, 1, 0, 0, 0}, { 1, 0, 0, 0, 0}, { 1, 1, 1, 1, 0},
                { 1, 0, 0, 0, 1}, { 1, 0, 0, 0, 1}, { 0, 1, 1, 1, 0}
            },
            {
                { 1, 1, 1, 1, 1}, { 0, 0, 0, 0, 1}, { 0, 0, 0, 1, 0}, { 0, 0, 1, 0, 0},
                { 0, 1, 0, 0, 0}, { 0, 1, 0, 0, 0}, { 0, 1, 0, 0, 0}
            },
            {
                { 0, 1, 1, 1, 0}, { 1, 0, 0, 0, 1}, { 1, 0, 0, 0, 1}, { 0, 1, 1, 1, 0},
                { 1, 0, 0, 0, 1}, { 1, 0, 0, 0, 1}, { 0, 1, 1, 1, 0}
            },
            {
                { 0, 1, 1, 1, 0}, { 1, 0, 0, 0, 1}, { 1, 0, 0, 0, 1}, { 0, 1, 1, 1, 1},
                { 0, 0, 0, 0, 1}, { 0, 0, 0, 1, 0}, { 0, 1, 1, 0, 0}
            },
        };

        // Двоеточие между чч:мм:сс
        static readonly int[,] colonPattern = new int[7, 2]
        {
            { 0, 0 }, { 1, 1 }, { 1, 1 }, { 0, 0 }, { 1, 1 }, { 1, 1 }, { 0, 0 }
        };

        // Цвет точек
        static readonly Color colorOn = Color.Blue;
        static readonly Color colorOff = new Color(0.5, 0.5, 0.5, 0.25);

        // Box views для 6 цифр, 7 строк и 5 столбцов.
        BoxView[,,] digitBoxViews = new BoxView[6, 7, 5];

        public MainPage()
        {
            InitializeComponent();

            // Размер точек BoxView
            double height = 0.85 / vertDots;
            double width = 0.85 / horzDots;

            //Инициализация точек BoxView
            double xIncrement = 1.0 / (horzDots - 1);
            double yIncrement = 1.0 / (vertDots - 1);
            double x = 0;

            for (int digit = 0; digit < 6; digit++)
            {
                for (int col = 0; col < 5; col++)
                {
                    double y = 0;

                    for (int row = 0; row < 7; row++)
                    {
                        BoxView boxView = new BoxView();
                        digitBoxViews[digit, row, col] = boxView;
                        absoluteLayout.Children.Add(boxView,
                                                    new Rectangle(x, y, width, height),
                                                    AbsoluteLayoutFlags.All);
                        y += yIncrement;
                    }
                    x += xIncrement;
                }
                x += xIncrement;

                if (digit == 1 || digit == 3)
                {
                    int colon = digit / 2;

                    for (int col = 0; col < 2; col++)
                    {
                        double y = 0;

                        for (int row = 0; row < 7; row++)
                        {
                            BoxView boxView = new BoxView
                            {
                                Color = colonPattern[row, col] == 1 ?
                                                colorOn : colorOff
                            };
                            absoluteLayout.Children.Add(boxView,
                                                        new Rectangle(x, y, width, height),
                                                        AbsoluteLayoutFlags.All);
                            y += yIncrement;
                        }
                        x += xIncrement;
                    }
                    x += xIncrement;
                }
            }

            // Инициализация времени
            Device.StartTimer(TimeSpan.FromSeconds(1), OnTimer);
            OnTimer();
        }

        void OnPageSizeChanged(object sender, EventArgs args)
        {
            absoluteLayout.HeightRequest = vertDots * Width / horzDots;
        }

        bool OnTimer()
        {
            DateTime dateTime = DateTime.Now;

            int hour = (dateTime.Hour + 11) % 12 + 1;

            SetDotMatrix(0, hour / 10);
            SetDotMatrix(1, hour % 10);
            SetDotMatrix(2, dateTime.Minute / 10);
            SetDotMatrix(3, dateTime.Minute % 10);
            SetDotMatrix(4, dateTime.Second / 10);
            SetDotMatrix(5, dateTime.Second % 10);
            return true;
        }

        void SetDotMatrix(int index, int digit)
        {
            for (int row = 0; row < 7; row++)
                for (int col = 0; col < 5; col++)
                {
                    bool isOn = numberPatterns[digit, row, col] == 1;
                    Color color = isOn ? colorOn : colorOff;
                    digitBoxViews[index, row, col].Color = color;
                }
        }
    }
}
