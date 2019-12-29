using System.Windows;

namespace PasswordStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;
        }

        public void program_Save(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.Save();   
        }

        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.Save();
        }

        public void about_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Steffen L.", "Contributors");
        }

        public void program_Exit(object sender, RoutedEventArgs e)
        {
            if (!mainWindowViewModel.IsSaved)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to save?", "Save to file", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    mainWindowViewModel.Save();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
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
