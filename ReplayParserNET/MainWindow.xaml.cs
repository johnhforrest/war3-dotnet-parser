using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReplayParserNET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tabControl.SelectedIndex = 1;
        }

        private void LoadSettings()
        {
            Height = Properties.Settings.Default.AppHeight;
            //grid_Main.Height = Properties.Settings.Default.AppHeight;
            //Width = Properties.Settings.Default.AppWidth;
            //grid_Main.Left = Properties.Settings.Default.XPos;
            //grid_Main.Top = Properties.Settings.Default.YPos;

            //if (Properties.Settings.Default.AppState != WindowState.Minimized)
            //    WindowState = Properties.Settings.Default.AppState;
        }

        //private void SaveSettings()
        //{
        //    Properties.Settings.Default.AppHeight = Height;
        //    Properties.Settings.Default.AppWidth = Width;
        //    //Settings.Default.Tab = MainWindow.
        //    Properties.Settings.Default.XPos = Left;
        //    Properties.Settings.Default.YPos = Top;
        //    Properties.Settings.Default.AppState = WindowState;
        //    Properties.Settings.Default.Save();
        //}
    }
}
