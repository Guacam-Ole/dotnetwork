using Microsoft.Extensions.Configuration;

using System;
using System.IO;

namespace CreateNav
{
    // quick and DIRTY MD2Nav
    class Program
    {
  
        static void Main(string[] args)
        {
            if (args.Length<2)
            {
                Console.WriteLine("call with [relative path] [archive title]");
                return;
            }

            var config = new Config();
           

            string relativePath = args[0];
            string archiveTitle = args[1];
            var markdownReader = new ReadMd(config, relativePath);
            var articles=markdownReader.ReadArticlesFromFolderRecursive(relativePath);

            var archiveFunctions = new Archive(config, relativePath, articles);
            archiveFunctions.WriteArchive(archiveTitle);
             
        }
    }
}
