using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Cryptor
{
    public class CryptoEngine
    {
        SetAlgoritham AlgorithmID;
        bool bWithKey = false;
        string keyword = "";
        public byte[] Vector;

        public CryptoEngine(SetAlgoritham AlgoID, string key) 
        {
            if (key.Length == 0)
                bWithKey = false;
            else
                bWithKey = true;

            keyword = key;
            AlgorithmID = AlgoID;
        }

    }

    public enum SetAlgoritham 
    {
        DES = 0, RC2, Rijndael, TripleDes
    }
}
