using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.IO;
using System;
using System.Windows;

namespace PasswordStore
{
    internal class MainWindowViewModel: INotifyPropertyChanged
    {
        private string _plainText = "";

        private const string InitialDirectory = @"c:\passwords";
        private const string Filter = "PasswordStore files (*.pwdf)|*.pwdf| All files (*.*)|*.*";

        private const string AppName = "PasswordStore";

        public bool IsSaved { get; private set; } = false;

        private string lastFileName = string.Empty;

        private SecurityController securityController =  new SecurityController();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save()
        {
            var masterPassword = RunMasterPasswordRequest();

            if (string.IsNullOrEmpty(masterPassword))
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                DefaultExt = ".pwdf",
                FileName = lastFileName,
                InitialDirectory = InitialDirectory,
                Filter = Filter
            };
            
            saveFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }

            lastFileName = saveFileDialog.FileName;

            try
            {
                File.WriteAllText(lastFileName, securityController.Encrypt(masterPassword, PlainText));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Unknown error occured during encryption: {ex.Message}");
                return;
            }

            IsSaved = true;
        }

        public void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                FileName = lastFileName,
                InitialDirectory = InitialDirectory,
                Filter = Filter
            };
            
            openFileDialog.ShowDialog();
            if (string.IsNullOrEmpty(openFileDialog.FileName))
            {
                return;
            }

            LoadFile(openFileDialog.FileName);
        }

        public void LoadFile(string fileName)
        {
            var masterPassword = RunMasterPasswordRequest();
            if (string.IsNullOrEmpty(masterPassword))
            {
                return;
            }

            string text = File.ReadAllText(fileName);
            try
            {
                PlainText = securityController.Decrypt(masterPassword, text);
            }
            catch
            {
                System.Windows.MessageBox.Show("Error: wrong master password.");
                return;
            }
        }

        private string RunMasterPasswordRequest()
        {
            var masterPasswordRequest = new PasswordRequest();
            masterPasswordRequest.ShowDialog(); 
            string result = masterPasswordRequest.pswCmd.Password;
            return result;
        }

        public void CheckIsSaved()
        {
            if (!IsSaved && !string.IsNullOrEmpty(PlainText))
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save?", AppName, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Save();
                }
            }
        }

        public void CheckClear()
        {
            if (string.IsNullOrEmpty(PlainText))
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show("Do you really want to clear all content?", AppName, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                PlainText = string.Empty;
            }
        }

        public bool CheckExit()
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to exit?", AppName, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }

        public string PlainText
        {
            get => _plainText;
            set
            {
                if (value != _plainText)
                {
                    _plainText = value;
                    IsSaved = false;
                    OnPropertyChanged("PlainText");
                }
            }
        }
    }
}
