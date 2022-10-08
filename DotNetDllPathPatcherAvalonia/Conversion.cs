using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNetDllPathPatcherWPF
{
    class Conversion
    {

        private const int MaxPathLength = 1024;

        /// <summary>
        /// 处理.net core的dll依赖
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="dllPathName"></param>
        /// <param name="oldDllPathName"></param>
        public static void ConversionDll(string exePath, string dllPathName, string oldDllPathName)
        {
            exePath = Path.GetFullPath(exePath);
            if (string.IsNullOrWhiteSpace(dllPathName))
            {
                dllPathName = "bin";
            }

            if (!File.Exists(exePath))
            {
                throw new FileNotFoundException($@"{exePath} not found!");
            }

            PatchExe(oldDllPathName, dllPathName, exePath);
            MoveDll(oldDllPathName, dllPathName, exePath);
        }

        public static void MoveDll(string oldDllPathName, string dllPathName, string exePath)
        {
            var root = Path.GetDirectoryName(exePath);
            if (root == null)
            {
                throw new InvalidOperationException(@"Wrong exe path");
            }
            if (string.IsNullOrEmpty(oldDllPathName))
            {
                var tempPath = Path.Combine(Path.GetDirectoryName(root)
                    ?? throw new InvalidOperationException(@"Wrong exe path"), @"tempBin");

                Directory.Move(root, tempPath);

                var newPath = Path.Combine(root, dllPathName);

                if (!string.IsNullOrEmpty(dllPathName))
                {
                    Directory.CreateDirectory(root);
                }

                //所有文件移动到新目录
                Directory.Move(tempPath, newPath);

                //旧exe文件移动到出来
                File.Move(Path.Combine(newPath, Path.GetFileName(exePath)), exePath);

                //需要处理资源文件等等
                if (Directory.Exists(@$"{newPath}\Resources"))
                {
                    Directory.Move(@$"{newPath}\Resources", @$"{root}\Resources");
                }
            }
            else
            {
                Directory.Move(Path.Combine(root, oldDllPathName), Path.Combine(root, dllPathName));
            }



        }

        public static void PatchExe(string oldDllPathName, string dllPathName, string exePath)
        {
            var exeName = Path.GetFileName(exePath);
            var separator = GetSeparator(exeName);

            if (!string.IsNullOrEmpty(oldDllPathName))
            {
                oldDllPathName += separator;
            }
            Span<byte> oldBytes = Encoding.UTF8.GetBytes($"{oldDllPathName}{ChangeExtensionToDll(exeName)}\0");
            if (oldBytes.Length > MaxPathLength)
            {
                throw new PathTooLongException(@"old dll path is too long");
            }

            if (!string.IsNullOrEmpty(dllPathName))
            {
                dllPathName += separator;
            }
            Span<byte> newBytes = Encoding.UTF8.GetBytes($"{dllPathName}{ChangeExtensionToDll(exeName)}\0");
            if (newBytes.Length > MaxPathLength)
            {
                throw new PathTooLongException(@"new dll path is too long");
            }

            Span<byte> bytes = File.ReadAllBytes(exePath);
            var index = bytes.IndexOf(oldBytes);
            if (index < 0)
            {
                throw new InvalidDataException($@"Could not find old dll path {oldDllPathName}");
            }

            if (index + newBytes.Length > bytes.Length)
            {
                throw new PathTooLongException(@"new dll path is too long");
            }

            using var fs = File.OpenWrite(exePath);
            fs.Write(bytes.Slice(0, index));
            fs.Write(newBytes);
            fs.Write(bytes.Slice(index + newBytes.Length));
        }

        private static bool IsWindowsExe(string str)
        {
            return str.EndsWith(@".exe", StringComparison.OrdinalIgnoreCase);
        }

        private static string ChangeExtensionToDll(string exeName)
        {
            return IsWindowsExe(exeName) ? Path.ChangeExtension(exeName, @".dll") : $@"{exeName}.dll";
        }

        private static string GetSeparator(string exeName)
        {
            return IsWindowsExe(exeName) ? @"\" : @"/";
        }
    }
}
