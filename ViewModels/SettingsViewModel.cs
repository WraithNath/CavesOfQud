using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace WraithNath.CavesOfQud.ViewModels
{
    /// <summary>
    /// View Model for the Settings Widnow
    /// </summary>
    public class SettingsViewModel : INotifyPropertyChanged
    {
        #region Members

        public delegate string SelectFolderDelegate(string currentFolder);
        public event SelectFolderDelegate SelectFolder;
        public event EventHandler CloseRequestedEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or Sets the Path to the Caves of Qud Save Directory
        /// </summary>
        public string CavesOfQudSavePath
        {
            get
            {
                return Properties.Settings.Default.CavesOfQudSavePath;
            }
            set
            {
                Properties.Settings.Default.CavesOfQudSavePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CavesOfQudSavePath)));
            }
        }

        /// <summary>
        /// Gets or Sets the Path to store the backups
        /// </summary>
        public string BackupsSavePath
        {
            get
            {
                return Properties.Settings.Default.BackupSavePath;
            }
            set
            {
                Properties.Settings.Default.BackupSavePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackupsSavePath)));
            }
        }

        /// <summary>
        /// Gets or Sets the Backup Interval
        /// </summary>
        public int BackupInterval
        {
            get
            {
                return Properties.Settings.Default.BackupInterval;
            }
            set
            {
                Properties.Settings.Default.BackupInterval = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackupIntervalText)));
            }
        }

        /// <summary>
        /// Gets the Display Text for the Backup Interval
        /// </summary>
        public string BackupIntervalText
        {
            get
            {
                return $"Backup Interval ({BackupInterval} minute{(BackupInterval > 1 ? "s" : "")}):";
            }
        }

        /// <summary>
        /// Gets or Sets the Number of backups to make before overwriting old backups
        /// </summary>
        public int NumberOfBackups
        {
            get
            {
                return Properties.Settings.Default.NumberOfBackups;
            }
            set
            {
                Properties.Settings.Default.NumberOfBackups = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumberOfBackupsText)));
            }
        }

        /// <summary>
        /// Gets the Display Text for the Number of Backups
        /// </summary>
        public string NumberOfBackupsText
        {
            get
            {
                return $"Number of Backups ({NumberOfBackups}):";
            }
        }

        /// <summary>
        /// Gets the Close Command
        /// </summary>
        public Command CloseCommand
        {
            get
            {
                return new Command(e =>
                {
                    this.CloseRequestedEvent?.Invoke(this, new EventArgs());
                });
            }
        }

        /// <summary>
        /// Gets the Save Command
        /// </summary>
        public Command SaveCommand
        {
            get
            {
                return new Command(e =>
                {
                    Properties.Settings.Default.Save();

                    this.CloseRequestedEvent?.Invoke(this, new EventArgs());
                });
            }
        }

        /// <summary>
        /// Gets the Browse Save Path Command
        /// </summary>
        public Command BrowseSavePathCommand
        {
            get
            {
                return new Command(e =>
                {
                    if (SelectFolder != null)
                        this.CavesOfQudSavePath = SelectFolder(this.CavesOfQudSavePath);
                });
            }
        }

        /// <summary>
        /// Gets the Browse Backups Path Command
        /// </summary>
        public Command BrowseBackupsPathCommand
        {
            get
            {
                return new Command(e =>
                {
                    if (SelectFolder != null)
                        this.BackupsSavePath = SelectFolder(this.BackupsSavePath);
                });
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsViewModel()
        {
            if (this.CavesOfQudSavePath == string.Empty)
                this.CavesOfQudSavePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}Low\Freehold Games\CavesOfQud\Saves";
        }

        #endregion Constructor
    }
}
