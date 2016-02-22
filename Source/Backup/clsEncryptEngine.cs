using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Encrypt_a_string
{
    public enum EncryptionAlgorithm
    {
         DES = 0, Rc2, Rijndael, TripleDes 
    }
    public class EncryptEngine
    {
        bool bWithKey = false;
        EncryptionAlgorithm AlgoritmID;
        string keyword = "";
        public byte[] Vector;
        public EncryptEngine(EncryptionAlgorithm AlgID, string Key)
        {
            if (Key.Length == 0)
                bWithKey = false;
            else
                bWithKey = true;

            keyword = Key;
            AlgoritmID = AlgID;
        }

        public EncryptionAlgorithm EncryptionAlgorithm
        {
            get
            {
                return AlgoritmID;
            }
            set
            {
                AlgoritmID = value;
            }
        }
    
        public ICryptoTransform GetCryptTransform()
        {
            byte[] key = Encoding.ASCII.GetBytes(keyword);

            switch (AlgoritmID)
            {
                case EncryptionAlgorithm.DES:
                    DES des = new DESCryptoServiceProvider();
                    des.Mode = CipherMode.CBC;
                    if (bWithKey) des.Key = key;
                    Vector = des.IV;
                    return des.CreateEncryptor();
                case EncryptionAlgorithm.Rc2:
                    RC2 rc =new RC2CryptoServiceProvider();
                    rc.Mode = CipherMode.CBC;
                    if (bWithKey) rc.Key = key;
                    Vector = rc.IV;
                    return rc.CreateEncryptor();
                case EncryptionAlgorithm.Rijndael:
                    Rijndael rj = new RijndaelManaged();
                    rj.Mode = CipherMode.CBC;
                    if (bWithKey) rj.Key = key;
                    Vector = rj.IV;
                    return rj.CreateEncryptor();
                case EncryptionAlgorithm.TripleDes:
                    TripleDES tDes = new TripleDESCryptoServiceProvider();
                    tDes.Mode = CipherMode.CBC;
                    if (bWithKey) tDes.Key = key;
                    Vector = tDes.IV;
                    return tDes.CreateEncryptor();
                default:
                    throw new CryptographicException("Algorithm " + AlgoritmID + " Not Supported!");
            }
        }
        public static bool ValidateKeySize(EncryptionAlgorithm algID,int Lenght)
        {
            switch (algID)
            {
                case EncryptionAlgorithm.DES:
                    DES des = new DESCryptoServiceProvider();
                    return des.ValidKeySize(Lenght);
                case EncryptionAlgorithm.Rc2:
                    RC2 rc = new RC2CryptoServiceProvider();
                    return rc.ValidKeySize(Lenght);
                case EncryptionAlgorithm.Rijndael:
                    Rijndael rj = new RijndaelManaged();
                    return rj.ValidKeySize(Lenght);
                case EncryptionAlgorithm.TripleDes:
                    TripleDES tDes = new TripleDESCryptoServiceProvider();
                    return tDes.ValidKeySize(Lenght);
                default:
                    throw new CryptographicException("Algorithm " + algID + " Not Supported!");
            }
        }
    }

   
}
