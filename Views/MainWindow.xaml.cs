using System;
using System.Collections.Generic;
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
using WraithNath.CavesOfQud.ViewModels;
using WraithNath.CavesOfQud.Views;

namespace WraithNath.CavesOfQud.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel viewModel = new MainWindowViewModel();

            viewModel.RequestSettings += ViewModel_RequestSettings;
            viewModel.RequestExit += ViewModel_RequestExit;

            this.DataContext = viewModel;
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Opens the settings Window
        /// </summary>
        /// <param name="sender">View Model</param>
        /// <param name="e">args</param>
        private void ViewModel_RequestSettings(object sender, EventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        /// <summary>
        /// Exits the application
        /// </summary>
        /// <param name="sender">View Model</param>
        /// <param name="e">args</param>
        private void ViewModel_RequestExit(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion Events
    }
}
