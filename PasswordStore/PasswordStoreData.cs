using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStore
{
    public class PasswordStoreData
    {
        public byte[] Encrypt(string plainText, string masterPassword)
        {
            var result = Encoding.ASCII.GetBytes(plainText);

            // TODO: encrypt

            return result;
        }

        public string Decrypt(byte[] bytes, string masterPassword)
        {
            string result = "";
            // TODO: encrypt
            return result;
        }

    }
}
