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
    public partial class MasterPasswordRequestDialog : Window
    {
        public MasterPasswordCmd masterPasswordCmd { get; set; }

        public MasterPasswordRequestDialog()
        {
            InitializeComponent();
            DataContext = this;
            masterPasswordCmd = new MasterPasswordCmd();
        }
    }
}
