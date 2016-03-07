using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptDecrypt
{
    public class MyEncryptor
    {
        
        private string _Phrase = "";
        private string _inputFile = "";
        private string _outputFile = "";
        enum TransformType { ENCRYPT = 0, DECRYPT = 1 }
        public string Phrase
        {
            set
            {
                this._Phrase = value;
                this.GenerateKey(this._Phrase);
            }
        }

       
        private byte[] _IV;
        private byte[] _Key;
        public MyEncryptor(string SecretPhrase)
        {
            this.Phrase = SecretPhrase;
        }

        public void Encrypt(string InputFile, string OutputFile)
        {

            try
            {
                if ((InputFile != null) && (InputFile.Length > 0))
                {
                    _inputFile = InputFile;
                }
                if ((OutputFile != null) && (OutputFile.Length > 0))
                {
                    _outputFile = OutputFile;
                }
                Transform(null, TransformType.ENCRYPT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Decrypt(string InputFile, string OutputFile)
        {

            try
            {
                if ((InputFile != null) && (InputFile.Length > 0))
                {
                    _inputFile = InputFile;
                }
                if ((OutputFile != null) && (OutputFile.Length > 0))
                {
                    _outputFile = OutputFile;
                }
                Transform(null, TransformType.DECRYPT);
            }
            catch (Exception ex)
            {
                Console.WriteLine("------------------------------------");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("------------------------------------");
                throw new Exception("The usb drive you selected is not the one that was used to encrypt the file.");
            }
        }

        private void GenerateKey(string SecretPhrase)
        {
            this._Key = new byte[24];
            this._IV = new byte[16];

            byte[] bytePhrase = Encoding.ASCII.GetBytes(SecretPhrase);
            SHA384Managed sha384 = new SHA384Managed();
            sha384.ComputeHash(bytePhrase);
            byte[] result = sha384.Hash;

            for (int loop = 0; loop < 24; loop++) this._Key[loop] = result[loop];
            for (int loop = 24; loop < 40; loop++) this._IV[loop - 24] = result[loop];
        }

 
        private byte[] Transform(byte[] input, TransformType transformType)
        {
            CryptoStream cryptoStream = null;      
            RijndaelManaged rijndael = null;        // Rijndael provider
            ICryptoTransform rijndaelTransform = null;           
            FileStream fsIn = null;                 //input file
            FileStream fsOut = null;                //output file
            MemoryStream memStream = null;         
            try
            {
               
                rijndael = new RijndaelManaged();
                rijndael.Key = this._Key;
                rijndael.IV = this._IV;
                if (transformType == TransformType.ENCRYPT)
                {
                    rijndaelTransform = rijndael.CreateEncryptor();
                }
                else
                {
                    rijndaelTransform = rijndael.CreateDecryptor();
                }

                if ((input != null) && (input.Length > 0))
                {
                    memStream = new MemoryStream();
                    cryptoStream = new CryptoStream(
                         memStream, rijndaelTransform, CryptoStreamMode.Write);

                    cryptoStream.Write(input, 0, input.Length);

                    cryptoStream.FlushFinalBlock();

                    return memStream.ToArray();

                }
                else if ((_inputFile.Length > 0) && (_outputFile.Length > 0))
                {
                    fsIn = new FileStream(_inputFile,
                                FileMode.Open, FileAccess.Read);
                    
                    fsOut = new FileStream(_outputFile,
                                FileMode.OpenOrCreate, FileAccess.Write);

                    memStream = new MemoryStream();

                    cryptoStream = new CryptoStream(fsOut, rijndaelTransform, CryptoStreamMode.Write);

                    int bufferLen = 4096;
                    byte[] buffer = new byte[bufferLen];
                    int bytesRead;
                    do
                    {
                        bytesRead = fsIn.Read(buffer, 0, bufferLen);
                        cryptoStream.Write(buffer, 0, bytesRead);

                    } while (bytesRead != 0);

                    cryptoStream.FlushFinalBlock();
                }
                return null;

            }
            catch (CryptographicException e)
            {
                throw new Exception("Crypt Lol");
                throw e;
            }
            catch (Exception e)
            {
                throw new Exception("LOL");
            }
            finally
            {
                if (rijndael != null) rijndael.Clear();
                if (rijndaelTransform != null) rijndaelTransform.Dispose();
                if (memStream != null) memStream.Close();
                if (fsOut != null) fsOut.Close();
                if (fsIn != null) fsIn.Close();


                try
                {
                    if (cryptoStream != null)
                    {
                        cryptoStream.Close();
                       }
                    
                    
                }catch(Exception)
                {
                    File.Delete(_outputFile);
                }
                
                
            }
        }

    }
}