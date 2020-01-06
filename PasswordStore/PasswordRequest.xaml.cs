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
using System.Windows.Shapes;

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pswCmd.Password))
            {
                MessageBox.Show("The master password cannot be empty.", "Invalid master password");
                return;
            }
            
            Close();
        }
    }
}
