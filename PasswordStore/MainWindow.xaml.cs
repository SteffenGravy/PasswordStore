using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace PasswordStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string AppName = "PasswordStore";
        public const string InitialDirectory = @"c:\passwords";
        public const string Filter = "PasswordStore files (*.pwdf)|*.pwdf| All files (*.*)|*.*";

        internal MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;

            DataObject.AddPastingHandler(tb, OnPaste);

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                if (CheckExit())
                {
                    CheckIsSaved();
                    Close();
                }
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText) return;

            var text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;

            mainWindowViewModel.PlainText = text;
        }

        public void program_Save(object sender, RoutedEventArgs e)
        {
            Save();   
        }

        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
        public void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CheckIsSaved();
            mainWindowViewModel.PlainText = string.Empty;
        }

        public void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            CheckIsSaved();
            Load();
        }
        
        public void about_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Steffen L.", "Contributors");
        }

        public void program_Exit(object sender, RoutedEventArgs e)
        {
            if (CheckExit())
            {
                CheckIsSaved();
                Close();
            }
        }

        public void program_Open(object sender, RoutedEventArgs e)
        {
            CheckIsSaved();
            Load();
        }

        public void program_SaveAndExit(object sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }

        private void CheckIsSaved()
        {
            if (!mainWindowViewModel.IsSaved && !string.IsNullOrEmpty(mainWindowViewModel.PlainText))
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", AppName, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Save();
                }
            }
        }

        private bool CheckExit()
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to exit?", AppName, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }

        private string RunMasterPasswordRequest()
        {
            var masterPasswordRequest = new PasswordRequest();
            masterPasswordRequest.ShowDialog(); 
            string result = masterPasswordRequest.pswCmd.Password;
            return result;
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
                FileName = mainWindowViewModel.lastFileName,
                InitialDirectory = InitialDirectory,
                Filter = Filter
            };
            
            saveFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }

            mainWindowViewModel.lastFileName = saveFileDialog.FileName;

            try
            {
                File.WriteAllText(mainWindowViewModel.lastFileName, SecurityController.Encrypt(masterPassword, mainWindowViewModel.PlainText));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Unknown error occured during encryption: {ex.Message}");
                return;
            }

            mainWindowViewModel.IsSaved = true;
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
                mainWindowViewModel.PlainText = SecurityController.Decrypt(masterPassword, text);
            }
            catch
            {
                System.Windows.MessageBox.Show("Error: wrong master password.");
                return;
            }
        }

        public void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                FileName = mainWindowViewModel.lastFileName,
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
    }
}
