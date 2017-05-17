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
        private readonly string _inputPath;
        private readonly string _outputPath;
        private readonly string _keyPath; 
       
        public EnigmaMachine(Status status, string algorithm, string inputPath, string outputPath, string keyPath)
        {
            _status = status;
            _algorithm = algorithm;
            _inputPath = inputPath;
            _outputPath = outputPath;
            _keyPath = keyPath;
        }

        public bool Run()
        {
            if (_status == Status.Encrypt)
            {
                if (Encrypt())
                {
                    return true;
                }
                return false;
            }
            if (_status == Status.Decrypt)
            {
                if (Decrypt())
                {
                    return true;
                }
                return false;
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
            if (!File.Exists(_inputPath))
            {
                return false;
            }

            var inputFile = new FileStream(_inputPath, FileMode.Open, FileAccess.Read);
            var outputFile = new FileStream(_outputPath, FileMode.Create, FileAccess.Write);

            var pos = _inputPath.LastIndexOf('.');
            var keyFileName = Concat(_inputPath.Substring(0,pos),".key.txt");
            var keyFile = new FileStream(keyFileName, FileMode.Create, FileAccess.Write);

            using (var algo = GetSpecificAlgo())
            {
                if (algo == null) return false;
                using (var cstream = new CryptoStream(outputFile, algo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    inputFile.CopyTo(cstream);
                }
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
            if (!File.Exists(_inputPath) && !File.Exists(_keyPath))
            {
                return false;
            }

            var inputFile = new FileStream(_inputPath, FileMode.Open, FileAccess.Read);
            var outputFile = new FileStream(_outputPath, FileMode.Create, FileAccess.Write);
            var keyFile = new FileStream(_keyPath, FileMode.Open, FileAccess.Read);

            using (var algo = GetSpecificAlgo())
            {
                if (algo == null) return false;
                using (var kstream = new StreamReader(keyFile))
                {
                    algo.Key = Convert.FromBase64String(kstream.ReadLine());
                    algo.IV = Convert.FromBase64String(kstream.ReadLine());
                }

                using (var cstream = new CryptoStream(inputFile, algo.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cstream.CopyTo(outputFile);
                }
                return true;
            }
        }    
    }
}
