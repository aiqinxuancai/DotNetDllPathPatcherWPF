using DotNetDllPathPatcherCMD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                        //完成
                        //获取获取是否要运行某个EXE，运行后等待15秒，然后执行删除不引用的dll，然后结束进程

                        string myLibraryShrink = args.First(a => a.StartsWith("--MyLibraryShrink"));
                        if (!string.IsNullOrWhiteSpace(myLibraryShrink))
                        {
                            var myLibraryShrinks = myLibraryShrink.Split(":");

                            if (myLibraryShrinks.Length == 2)
                            {
                                RunLibraryShrink(fileName, binPath);
                            }

                        }
                        


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

        private static void RunLibraryShrink(string fileName, string binPath)
        {
            if (File.Exists(fileName))
            {
                Console.WriteLine($"Run {fileName}");

                var process = Process.Start(fileName);

                Console.WriteLine("Waiting...");

                Thread.Sleep(15 * 1000);


                //执行删除
                var dllFiles = Directory.GetFiles($@"{Path.GetDirectoryName(fileName)}\{binPath}", "*.dll", SearchOption.TopDirectoryOnly);

                foreach (var file in dllFiles)
                {
                    try
                    {
                        File.Delete(file);
                        Console.WriteLine($"Deleted {file}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                }

                process.Kill();
                process.WaitForExit();

                Thread.Sleep(3 * 1000);

                //清除临时生成的文件？
                var files = Directory.GetFiles(Path.GetDirectoryName(fileName), "*.*", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    if (file != fileName)
                    {
                        File.Delete(file);
                    }
                }

            }
        }
    }
}




