using DotNetDllPathPatcherCMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDllPathPatcherCMD // Note: actual namespace depends on the project name.
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
                    Console.WriteLine($"Move dll to ‘{binPath}’");
                    try
                    {
                        Conversion.ConversionDll(fileName, binPath, oldBinPath);
                        Console.WriteLine("Success！");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    

                }
                else 
                {
                    Console.WriteLine("File not exist.");
                }
            }
            else
            {
                Console.WriteLine("Missing exe file path.");
            }


            
        }
    }
}




