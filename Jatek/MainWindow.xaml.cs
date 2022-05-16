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
        DispatcherTimer dt;
        DispatcherTimer seals;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
            DifficultySelection.Visibility = Visibility.Visible;

        }
        private void Logic_GamePaused(object sender, EventArgs e)
        {
            dt.Stop();
            seals.Stop();
            Menu.Visibility=Visibility.Visible;
        }
        private void Dt_Tick(object sender, EventArgs e)
        {
            logic.MoveBullets();
            BulletLabel.Content = $"x{logic.BulletNumber}";
            LifeLabel.Content = $"x{logic.Lives}";
        }
        private void seals_Tick(object sender, EventArgs e)
        {
            logic.MoveSeals();
        }

        private void Logic_LifeLost(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Oops, you died");
            logic.RestartLevel();
        }

        private void Logic_GameOver(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Game Over!");
            if (result == MessageBoxResult.OK)
            {
                this.Close();
            }
        }
        private void Logic_GameWon(object sender, EventArgs e)
        {
            dt.Stop();
            seals.Stop();
            var result = MessageBox.Show($"YOU WON!\n\nPOINTS: {logic.BulletNumber+logic.Lives*2}");
            if (result == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            controller.KeyPressed(e.Key);
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
            dt.Start();
            seals.Start();
        }

        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            DifficultySelection.Visibility = Visibility.Hidden;
            MainMenu.Visibility = Visibility.Visible;
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            DifficultySelection.Visibility = Visibility.Hidden;
            logic = new JatekLogic();
            logic.SetupMap(5);
            logic.GameOver += Logic_GameOver;
            logic.LifeLost += Logic_LifeLost;
            logic.GameWon += Logic_GameWon;
            logic.GamePaused += Logic_GamePaused;
            controller = new GameController(logic);
            display.SetupModel(logic);
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));

            seals = new DispatcherTimer();
            seals.Interval = TimeSpan.FromMilliseconds(200);
            seals.Tick += seals_Tick;
            seals.Start();

            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(50);
            dt.Tick += Dt_Tick;
            dt.Start();

        }

        private void NormalButton_Click(object sender, RoutedEventArgs e)
        {

            DifficultySelection.Visibility = Visibility.Hidden;
            logic = new JatekLogic();
            logic.SetupMap(3);
            logic.GameOver += Logic_GameOver;
            logic.LifeLost += Logic_LifeLost;
            logic.GameWon += Logic_GameWon;
            logic.GamePaused += Logic_GamePaused;
            controller = new GameController(logic);
            display.SetupModel(logic);
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));

            seals = new DispatcherTimer();
            seals.Interval = TimeSpan.FromMilliseconds(200);
            seals.Tick += seals_Tick;
            seals.Start();

            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(50);
            dt.Tick += Dt_Tick;
            dt.Start();
        }

        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            DifficultySelection.Visibility = Visibility.Hidden;
            logic = new JatekLogic();
            logic.SetupMap(1);
            logic.GameOver += Logic_GameOver;
            logic.LifeLost += Logic_LifeLost;
            logic.GameWon += Logic_GameWon;
            logic.GamePaused += Logic_GamePaused;
            controller = new GameController(logic);
            display.SetupModel(logic);
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));

            seals = new DispatcherTimer();
            seals.Interval = TimeSpan.FromMilliseconds(200);
            seals.Tick += seals_Tick;
            seals.Start();

            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(50);
            dt.Tick += Dt_Tick;
            dt.Start();

        }
    }
}
