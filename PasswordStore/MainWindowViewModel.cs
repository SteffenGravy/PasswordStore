using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.IO;
using System;

namespace PasswordStore
{
    internal class MainWindowViewModel: INotifyPropertyChanged
    {
        private string _plainText = "";

        private const string InitialDirectory = @"c:\passwords";
        private const string Filter = "PasswordStore files (*.pwdf)|*.pwdf| All files (*.*)|*.*";

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

            var masterPassword = RunMasterPasswordRequest();
            if (string.IsNullOrEmpty(masterPassword))
            {
                return;
            }

            string text = File.ReadAllText(openFileDialog.FileName);
            try
            {
                PlainText = securityController.Decrypt(masterPassword, text);
            }
            catch
            {
                System.Windows.MessageBox.Show("Error: wrong master password.");
                return;
            }
            IsSaved = false;
        }

        private string RunMasterPasswordRequest()
        {
            var masterPasswordRequest = new PasswordRequest();
            masterPasswordRequest.ShowDialog(); 
            string result = masterPasswordRequest.pswCmd.Password;
            return result;
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
