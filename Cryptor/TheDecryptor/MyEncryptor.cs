﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TheDecryptor
{
    class MyEncryptor
    {
        private string _Phrase = "";
        //contains input file path and name
        private string _inputFile = "";
        //contains output file path and name
        private string _outputFile = "";
        enum TransformType { ENCRYPT = 0, DECRYPT = 1 }

        /// <value>Set the phrase used to generate the secret key.</value>
        public string Phrase
        {
            set
            {
                this._Phrase = value;
                this.GenerateKey(this._Phrase);
            }
        }

        // Internal initialization vector value to 
        // encrypt/decrypt the first block
        private byte[] _IV;

        // Internal secret key value
        private byte[] _Key;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SecretPhrase">Secret phrase to generate key</param>
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
                //Exception comes here if the secret key doesnt match
                Console.WriteLine("------------------------------------");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("------------------------------------");
                throw new Exception("Either The usb drive you selected is not the one that was used to encrypt the file or the password is not correct.");
                //throw ex;
            }
        }

        private void GenerateKey(string SecretPhrase)
        {
            // Initialize internal values
            this._Key = new byte[24];
            this._IV = new byte[16];

            // Perform a hash operation using the phrase.  This will 
            // generate a unique 32 character value to be used as the key.
            byte[] bytePhrase = Encoding.ASCII.GetBytes(SecretPhrase);
            SHA384Managed sha384 = new SHA384Managed();
            sha384.ComputeHash(bytePhrase);
            byte[] result = sha384.Hash;

            // Transfer the first 24 characters of the hashed value to the key
            // and the remaining 8 characters to the intialization vector.
            for (int loop = 0; loop < 24; loop++) this._Key[loop] = result[loop];
            for (int loop = 24; loop < 40; loop++) this._IV[loop - 24] = result[loop];
        }

        /*****************************************************************
         * Transform one form to anoter based on CryptoTransform
         * It is used to encrypt to decrypt as well as decrypt to encrypt
         * Parameters:  input [byte array] - which needs to be transform 
         *              transformType - encrypt/decrypt transform
         * 
         * Return Val:  byte array - transformed value.
         ***************************************************************/
        private byte[] Transform(byte[] input, TransformType transformType)
        {
            CryptoStream cryptoStream = null;      // Stream used to encrypt
            RijndaelManaged rijndael = null;        // Rijndael provider
            ICryptoTransform rijndaelTransform = null;// Encrypting object            
            FileStream fsIn = null;                 //input file
            FileStream fsOut = null;                //output file
            MemoryStream memStream = null;          // Stream to contain data
            try
            {
                // Create the crypto objects
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
                    // First we are going to open the file streams 
                    fsIn = new FileStream(_inputFile,
                                FileMode.Open, FileAccess.Read);

                    fsOut = new FileStream(_outputFile,
                                FileMode.OpenOrCreate, FileAccess.Write);

                    memStream = new MemoryStream();

                    cryptoStream = new CryptoStream(fsOut, rijndaelTransform, CryptoStreamMode.Write);
                    //cryptoStream = new CryptoStream(memStream, rijndaelTransform, CryptoStreamMode.Write);


                    // Now will will initialize a buffer and will be 
                    // processing the input file in chunks. 
                    // This is done to avoid reading the whole file (which can be
                    // huge) into memory. 
                    int bufferLen = 4096;
                    byte[] buffer = new byte[bufferLen];
                    int bytesRead;
                    do
                    {
                        // read a chunk of data from the input file 
                        bytesRead = fsIn.Read(buffer, 0, bufferLen);
                        // Encrypt it 
                        cryptoStream.Write(buffer, 0, bytesRead);

                    } while (bytesRead != 0);

                    cryptoStream.FlushFinalBlock();
                }
                return null;

            }
            catch (CryptographicException e)
            {
                //throw new Exception("Crypt Lol");
                throw e;
            }
            catch (Exception e)
            {
                //throw new Exception("LOL");
                throw e;
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
                        //fsOut = new FileStream(_outputFile,
                        //        FileMode.OpenOrCreate, FileAccess.Write);
                        //memStream.WriteTo(fsOut);  // file was written if used abive even key mismatch. so the file was corupt. writing here happens only key is correct.
                    }


                }
                catch (Exception)
                {
                    File.Delete(_outputFile);
                }


            }
        }
    }
}
