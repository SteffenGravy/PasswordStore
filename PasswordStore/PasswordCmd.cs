using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordStore
{
    public class PasswordCmd : ICommand
    {
        public string Password { get; set; }

        public event EventHandler CanExecuteChanged;

        public event Action PasswordSet;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            PasswordBox passwordBox = (PasswordBox)parameter;
            Password = passwordBox.Password;
            PasswordSet();
        }
    }
}
