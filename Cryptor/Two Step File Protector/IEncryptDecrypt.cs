using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Two_Step_File_Protector
{
    interface IEncryptDecrypt
    {
        void Encrypt(string InputFile, string OutputFile);
        void Decrypt(string InputFile, string OutputFile);
    }
}
