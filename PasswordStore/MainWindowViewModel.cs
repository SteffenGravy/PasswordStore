using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.IO;
using Microsoft.VisualBasic;
using System.Threading;

namespace PasswordStore
{
    internal class MainWindowViewModel: INotifyPropertyChanged
    {
        private string _plainText = "";

        public bool IsSaved { get; private set; } = false;

        private PasswordStoreData passwordStoreData =  new PasswordStoreData();


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save()
        {
            var masterPassword = RunMasterPasswordRequest();

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if(saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName, passwordStoreData.Encrypt(PlainText, masterPassword));
            }

            IsSaved = true;
        }

        public void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".pwdf",
                CheckFileExists = true,

            };
            var bytes = new byte[] { };
            if (openFileDialog.ShowDialog() == true)
            {
                bytes = File.ReadAllBytes(openFileDialog.FileName);
            }

            var masterPassword = RunMasterPasswordRequest();
            PlainText = passwordStoreData.Decrypt(bytes, masterPassword);
            IsSaved = false;
        }

        private string RunMasterPasswordRequest()
        {
            var masterPasswordRequest = new MasterPasswordRequestDialog();
            masterPasswordRequest.Show();
            return masterPasswordRequest.masterPasswordCmd.Password;
        }

        public string PlainText
        {
            get { return _plainText; }
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
