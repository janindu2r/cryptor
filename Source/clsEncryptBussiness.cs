using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Encrypt_a_string
{
    public class Encryptor
    {
        EncryptEngine engin;
        public byte[] IV;
        public Encryptor(EncryptionAlgorithm algID, string key)
        {
            engin = new EncryptEngine(algID, key);
        }

        public EncryptEngine EncryptEngine
        {
            get
            {
                return engin;
            }
            set
            {
                engin = value;
            }
        }
    
        public string Encrypt(string MainString)
        {
            MemoryStream memory = new MemoryStream();
            CryptoStream stream = new CryptoStream(memory, engin.GetCryptTransform(), CryptoStreamMode.Write);
            StreamWriter streamwriter = new StreamWriter(stream);
            streamwriter.WriteLine(MainString);
            streamwriter.Close();
            stream.Close();
            IV = engin.Vector;
            byte[] buffer = memory.ToArray();
            memory.Close();
            return Convert.ToBase64String(buffer);

        }
    }
}
