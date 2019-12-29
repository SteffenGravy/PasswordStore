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
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if(saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName, passwordStoreData.Encrypt(PlainText, RunMasterPasswordRequest()));
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
            if (openFileDialog.ShowDialog() == true)
            {
                var bytes = File.ReadAllBytes(openFileDialog.FileName);
                PlainText = passwordStoreData.Decrypt(bytes, RunMasterPasswordRequest());
            }

            IsSaved = false;
        }

        private string RunMasterPasswordRequest()
        {
            var result = MessageBox.Show("Please enter the master password", "Enter master password");
            return result.ToString();
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
