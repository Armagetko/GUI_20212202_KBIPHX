using Jatek.Controller;
using Jatek.Logic;
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
using System.Windows.Threading;

namespace Jatek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameController controller;
        JatekLogic logic;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ;
            logic = new JatekLogic();
            logic.SetupMap();
            controller = new GameController(logic);
            display.SetupModel(logic);
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(100);
            dt.Tick += Dt_Tick;
            dt.Start();
        }
        private void Dt_Tick(object sender, EventArgs e)
        {
            logic.MoveGameItems();
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
            //display.InvalidateVisual();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            controller.KeyPressed(e.Key);
            //display.InvalidateVisual();
        }
    }
}
