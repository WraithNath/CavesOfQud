using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WraithNath.CavesOfQud.ViewModels;
using static WraithNath.CavesOfQud.ViewModels.SettingsViewModel;

namespace WraithNath.CavesOfQud.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();

            SettingsViewModel viewModel = new SettingsViewModel();

            viewModel.CloseRequestedEvent += ViewModel_CloseRequestedEvent;
            viewModel.SelectFolder += ViewModel_SelectFolder;

            this.DataContext = viewModel;
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Opens a folder browser dialog and returns the result
        /// </summary>
        /// <param name="currentFolder">The Current Folder</param>
        /// <returns>Folder Patj</returns>
        private string ViewModel_SelectFolder(string currentFolder)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = currentFolder;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    return dlg.SelectedPath;
            }

            return currentFolder;
        }

        /// <summary>
        /// Closes the Current Window
        /// </summary>
        /// <param name="sender">View Model</param>
        /// <param name="e">args</param>
        private void ViewModel_CloseRequestedEvent(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion Events
    }
}
