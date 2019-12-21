using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStore
{
    internal class MainWindowView
    {

        private string _plainText = "initial Text";


        internal string PlainText
        {
            get { return _plainText; }
            set
            {
                if (value != _plainText)
                {
                    _plainText = value;
                }
            }
        }


    }
}
