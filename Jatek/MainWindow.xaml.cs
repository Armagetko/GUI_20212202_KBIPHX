using Jatek.Controller;
using Jatek.Logic;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
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
        string[] lvls;
        public string selected { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            lvls = Directory.GetFiles(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Saves"), "*.txt");
            if (lvls.Length > 0)
                LoadGameButton.IsEnabled = true;
            MainMenu.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseMenu();
            DifficultySelection.Visibility = Visibility.Visible;
        }
        private void Logic_GamePaused(object sender, EventArgs e)
        {
            dt.Stop();
            seals.Stop();
            Menu.Visibility = Visibility.Visible;
        }
        private void Dt_Tick(object sender, EventArgs e)
        {
            logic.MoveBullets();
            BulletLabel.Content = $"x{logic.BulletNumber}\t remaining: {logic.BulletfishesOnMap}";
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
            var result = MessageBox.Show($"YOU WON!\n\nPOINTS: {logic.BulletNumber + logic.Lives * 2}");
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
            CollapseMenu();
            dt.Start();
            seals.Start();
        }

        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ;
            CollapseMenu();
            MainMenu.Visibility = Visibility.Visible;
            if (lvls.Length > 0)
                LoadGameButton.IsEnabled = true;
            HelpWindow.Children.Clear();
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseMenu();

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
            CollapseMenu();

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
            seals.Interval = TimeSpan.FromMilliseconds(181);
            seals.Tick += seals_Tick;
            seals.Start();

            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(49);
            dt.Tick += Dt_Tick;
            dt.Start();
        }
        private void CollapseMenu()
        {
            DifficultySelection.Visibility = Visibility.Collapsed;
            HelpWindow.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Collapsed;
            MainMenu.Visibility = Visibility.Collapsed;
            if (SavedGamesPanel.Items.Count > 0)
                SavedGamesPanel.Items.Clear();
            SavedGamesPanel.Visibility = Visibility.Collapsed;
        }
        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseMenu();

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

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            HelpWindow.Visibility = Visibility.Visible;
            Label k = new Label();
            k.Content = "CONTROLS";
            k.FontSize = 30;
            k.HorizontalAlignment = HorizontalAlignment.Center;
            k.Margin = new Thickness(0, 30, 0, 30);
            HelpWindow.Children.Add(k);

            Button b = new Button();
            b.Content = "Back to the menu";
            b.Name = "BackToMenuControl";
            b.FontSize = 20;
            b.HorizontalAlignment = HorizontalAlignment.Center;
            b.Width = 300;
            b.Padding = new Thickness(20);
            b.Margin = new Thickness(0,0,0,50);
            b.Click += BackToMenuButton_Click;
            HelpWindow.Children.Add(b);

            if (File.Exists("controls.txt"))
            {
                StreamReader sr = new StreamReader("controls.txt");
                while (!sr.EndOfStream)
                {
                    string[] lines = sr.ReadLine().Split('#');
                    Label l = new Label();
                    l.Content = $"{lines[0]}\t\t{lines[1]}";
                    l.FontSize = 20;
                    l.Margin = new Thickness(630, 0, 0, 0);
                    HelpWindow.Children.Add(l);
                }
            }
        }

        private void LoadGameButton_Click(object sender, RoutedEventArgs e)
        {
            SavedGamesPanel.Visibility = Visibility.Visible;
            foreach (var item in lvls)
            {
                Label tmp = new Label();
                tmp.Content = item.Split('.')[1].Split(@"\").Last();
                tmp.FontSize = 20;
                tmp.Padding = new Thickness(10, 10,10,10);
                tmp.MouseLeftButtonDown += Tmp_MouseLeftButtonDown; 
                SavedGamesPanel.Items.Add(tmp);
            }

        }

        private void Tmp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var orig=sender.ToString().Split(' ')[1]+".txt";
            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(),"Saves",orig);
            CollapseMenu();
            logic = new JatekLogic();
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
            logic.LoadGame(path);
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var a= logic.SaveGame();
            var result = MessageBox.Show("Game saved as "+a);
        }
    }
}
