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
                FileName = lastFileName
            };

            if (string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }

            saveFileDialog.ShowDialog();

            lastFileName = saveFileDialog.FileName;

            File.WriteAllText(lastFileName, securityController.Encrypt(masterPassword, PlainText));

            IsSaved = true;
        }

        public void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".pwdf",
                CheckFileExists = true,
                FileName = lastFileName
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
            PlainText = securityController.Decrypt(masterPassword, text);
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
