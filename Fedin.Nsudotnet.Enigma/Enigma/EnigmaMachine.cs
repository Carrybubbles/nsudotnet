using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace Fedin.Nsudotnet.Enigma
{
    class EnigmaMachine
    {
        public enum Status
        {
            Encrypt = 0,
            Decrypt = 1,
        };

        private readonly Status _status;        
        private readonly string _algorithm;
        private readonly string _input;
        private readonly string _output;
        private readonly string _key; 
       
        public EnigmaMachine(Status status, string algorithm, string input, string output, string key)
        {
            _status = status;
            _algorithm = algorithm;
            _input = input;
            _output = output;
            _key = key;
        }

        public bool Run()
        {
            if (_status == Status.Encrypt)
            {
                return Encrypt();
            }
            if (_status == Status.Decrypt)
            {
                return Decrypt();
            }
            return false;
        }

        private SymmetricAlgorithm GetSpecificAlgo()
        {
            if (_algorithm.Equals("aes", StringComparison.OrdinalIgnoreCase))
            {
                return Aes.Create();
            }
            if (_algorithm.Equals("des", StringComparison.OrdinalIgnoreCase))
            {
                return DES.Create();
            }
            if (_algorithm.Equals("rc2", StringComparison.OrdinalIgnoreCase))
            {
                return  RC2.Create();
            }
            if (_algorithm.Equals("rijndael", StringComparison.OrdinalIgnoreCase))
            {
                return  Rijndael.Create();
            }
            return null;
        }

        private bool Encrypt()
        {
            if (!File.Exists(_input))
            {
                return false;
            }

            var keyFileNameWithExtension = string.Format("{0}.key.txt", Path.GetFileNameWithoutExtension(_input));
 
            using (var algo = GetSpecificAlgo())
            {
                if (algo == null) return false;

                using (var inputFile = new FileStream(_input, FileMode.Open, FileAccess.Read))
                using (var outputFile = new FileStream(_output, FileMode.Create, FileAccess.Write))
                using (var cstream = new CryptoStream(outputFile, algo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    inputFile.CopyTo(cstream);
                }

                using (var keyFile = new FileStream(keyFileNameWithExtension, FileMode.Create, FileAccess.Write))
                using (var kstream = new StreamWriter(keyFile))
                {
                    kstream.WriteLine(Convert.ToBase64String(algo.Key));
                    kstream.WriteLine(Convert.ToBase64String(algo.IV));
                }

                return true;

            }
        }

        private bool Decrypt()
        {
            if (!File.Exists(_input) && !File.Exists(_key))
            {
                return false;
            }

            using (var algo = GetSpecificAlgo())
            {
                if (algo == null) return false;

                using (var keyFile = new FileStream(_key, FileMode.Open, FileAccess.Read))
                using (var kstream = new StreamReader(keyFile))
                {
                    algo.Key = Convert.FromBase64String(kstream.ReadLine());
                    algo.IV = Convert.FromBase64String(kstream.ReadLine());
                }

                using (var outputFile = new FileStream(_output, FileMode.Create, FileAccess.Write))
                using (var inputFile = new FileStream(_input, FileMode.Open, FileAccess.Read))
                using (var cstream = new CryptoStream(inputFile, algo.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cstream.CopyTo(outputFile);
                }
                return true;
            }
        }    
    }
}
