using DotNetDllPathPatcherWPF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                if (File.Exists(args[0]))
                {
                    string fileName = args[0];
                    string binPath = "bin";
                    string oldBinPath = "";
                    Console.WriteLine("Start...");
                    Conversion.ConversionDll(fileName, binPath, oldBinPath);
                }
            }


            
        }
    }
}




