using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStore
{
    public class PasswordStoreData
    {

        public byte[] GetBytesToStore(string plainText)
        {
            var result = Encoding.ASCII.GetBytes(plainText);

            // TODO: encrypt

            return result;
        }
    }
}
