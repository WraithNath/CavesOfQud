using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace WraithNath.CavesOfQud.ViewModels
{
    /// <summary>
    /// View Model for the Main Window
    /// </summary>
    public class MainWindowViewModel
    {
        #region Members

        public event EventHandler RequestExit;
        public event EventHandler RequestSettings;
        private DispatcherTimer _timer = null;
        private ObservableCollection<string> _logMessages = null;
        private int _currentSaveNumber = 1;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the Log Messages
        /// </summary>
        public ObservableCollection<string> LogMessages
        {
            get
            {
                return _logMessages;
            }
        }

        /// <summary>
        /// Gets the Start Command
        /// </summary>
        public Command StartCommand
        {
            get
            {
                return new Command(e =>
                {
                    _timer.Start();
                    this.AddLogMessage("Timer Started.");
                },
                e =>
                {
                    return !_timer.IsEnabled;
                });
            }
        }

        /// <summary>
        /// Gets the Stop Command
        /// </summary>
        public Command StopCommand
        {
            get
            {
                return new Command(e =>
                {
                    _timer.Stop();
                    this.AddLogMessage("Timer Stopped.");
                },
                e =>
                {
                    return _timer.IsEnabled;
                });
            }
        }

        /// <summary>
        /// Gets the Exit Command
        /// </summary>
        public Command ExitCommand
        {
            get
            {
                return new Command(e =>
                {
                    _timer.Stop();
                    this.AddLogMessage("Timer Stopped.");
                    RequestExit?.Invoke(this, new EventArgs());
                });
            }
        }

        /// <summary>
        /// Gets the Settings Command
        /// </summary>
        public Command SettingsCommand
        {
            get
            {
                return new Command(e =>
                {
                    if (StopCommand.CanExecute())
                        StopCommand.Execute();

                    RequestSettings?.Invoke(this, new EventArgs());

                    _timer.Interval = TimeSpan.FromMinutes(Properties.Settings.Default.BackupInterval);
                    this.AddLogMessage($"Backup Interval set to: {Properties.Settings.Default.BackupInterval}");
                });
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _logMessages = new ObservableCollection<string>();

            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;

            _timer.Interval = TimeSpan.FromMinutes(Properties.Settings.Default.BackupInterval);
            this.AddLogMessage($"Backup Interval set to: {Properties.Settings.Default.BackupInterval}");
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Timer Tick Event
        /// </summary>
        /// <param name="sender">timer</param>
        /// <param name="e">args</param>
        private void _timer_Tick(object sender, EventArgs e)
        {
            this.Backup();
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Adds a message to the log
        /// </summary>
        /// <param name="message">The Message to Add</param>
        private void AddLogMessage(string message)
        {
            this.LogMessages.Insert(0, $"{DateTime.Now}: {message}");
        }

        /// <summary>
        /// Performs the Backup
        /// </summary>
        private void Backup()
        {
            string sourceDir = Properties.Settings.Default.CavesOfQudSavePath;
            string destDir = $"{Properties.Settings.Default.BackupSavePath}\\{_currentSaveNumber}";

            this.AddLogMessage($"Backing up Saves to '{destDir}'");

            this.DirectoryCopy(sourceDir, destDir, true);

            this.AddLogMessage("Saves backed up!");

            _currentSaveNumber++;

            if (_currentSaveNumber > Properties.Settings.Default.NumberOfBackups)
                _currentSaveNumber = 1;
        }

        /// <summary>
        /// Copies a directory
        /// </summary>
        /// <param name="sourceDirName">The Source Directory Name</param>
        /// <param name="destDirName">The Destination Directory Name</param>
        /// <param name="copySubDirs">Whether to copy sub directories or not</param>
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            try
            {
                // Get the subdirectories for the specified directory.
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);

                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                DirectoryInfo[] dirs = dir.GetDirectories();
                // If the destination directory doesn't exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, true);
                }

                // If copying subdirectories, copy them and their contents to new location.
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion Methods
    }
}
