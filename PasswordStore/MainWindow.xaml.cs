using System.Windows;
using System.Windows.Input;

namespace PasswordStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
                if (mainWindowViewModel.CheckExit())
                {
                    mainWindowViewModel.CheckIsSaved();
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
            mainWindowViewModel.Save();   
        }

        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.Save();
        }
        public void btnClear_Click(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.CheckClear();
        }

        public void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.Load();
        }
        
        public void about_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Steffen L.", "Contributors");
        }

        public void program_Exit(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.CheckIsSaved();
            Close();
        }

        public void program_Open(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.Load();
        }

        public void program_SaveAndExit(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.Save();
            Close();
        }
    }
}
