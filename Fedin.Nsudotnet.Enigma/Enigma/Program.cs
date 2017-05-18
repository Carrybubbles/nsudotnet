using System;
using System.Diagnostics.Eventing.Reader;

namespace Fedin.Nsudotnet.Enigma
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 4)
            {
                if (args[0].Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                {
                    var machine = new EnigmaMachine(EnigmaMachine.Status.Encrypt,args[2], args[1], args[3], null);
                    Console.WriteLine(machine.Run() ? "Success!" : "Error");
                }
                else if (args[0].Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                {
                    if (args.Length == 5)
                    {
                        var machine = new EnigmaMachine(EnigmaMachine.Status.Decrypt,args[2], args[1], args[4], args[3]);
                        Console.WriteLine(machine.Run() ? "Success!" : "Error!");
                    }
                }
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Check your input!");
            Console.ReadLine();
        }        
    }
}
