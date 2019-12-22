using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStore
{
    internal class MainWindowViewModel
    {

        private string _plainText = "initial Text";

        private PasswordStoreData passwordStoreData =  new PasswordStoreData();


        public byte[] BytesToStore => passwordStoreData.GetBytesToStore(_plainText);


        public string PlainText
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
