using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PasswordStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        MainWindowViewModel mainWindowVielModel = new MainWindowViewModel();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = mainWindowVielModel;
        }

        public void program_Save(object sender, RoutedEventArgs e)
        {
            mainWindowVielModel.PlainText = "abc";
            OnPropertyChanged("PlainText");
        }


        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllBytes(saveFileDialog.FileName, new byte[] { }
                // Todo: write data here
                );
        }

        public void about_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Steffen L.", "Contributors");
        }

        public void program_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void program_Open(object sender, RoutedEventArgs e)
        {
            // TODO: open a file and load it
        }

        public void program_SaveAndExit(object sender, RoutedEventArgs e)
        {

        }
    }
}
