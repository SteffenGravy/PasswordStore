using System.Windows;
using System.Windows.Input;

namespace PasswordStore
{
    /// <summary>
    /// Interaction logic for PasswordBox.xaml
    /// </summary>
    public partial class PasswordRequest : Window
    {
        public PasswordCmd pswCmd { get; set; }

        public PasswordRequest()
        {
            InitializeComponent();
            DataContext = this;
            pswCmd = new PasswordCmd();
            pswCmd.PasswordSet += CloseIfPasswordIsNotEmpty;

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void CloseIfPasswordIsNotEmpty()
        {
            if (string.IsNullOrEmpty(pswCmd.Password))
            {
                MessageBox.Show("The master password cannot be empty.", "Invalid master password");
                return;
            }
            
            Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
