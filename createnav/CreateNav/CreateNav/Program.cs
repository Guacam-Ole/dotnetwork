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
            Console.WriteLine($"writing {markdownReader.Categories.Count} Categories and {markdownReader.Tags.Count} tags from {markdownReader.FileCount} files");

            var archiveFunctions = new Archive(config, relativePath, articles);
            archiveFunctions.WriteArchive(archiveTitle);
             
        }
    }
}
