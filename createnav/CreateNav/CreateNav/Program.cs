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

            var config=new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var repoPath=config.GetSection("LocalRepo").Value;
            var docRoot = config.GetSection("DocumentRoot").Value;
            var version = new Version(config.GetSection("Version").Value);


            string relativePath = args[0];
            string archiveTitle = args[1];
            var markdownReader = new ReadMd(repoPath, relativePath, docRoot, version);
            var articles=markdownReader.ReadArticlesFromFolderRecursive(relativePath);

            var archiveFunctions = new Archive(relativePath, articles);
            string archiveMd = archiveFunctions.CreateArchiveMarkDown(archiveTitle);
            


            Console.WriteLine("Hello World!");
        }
    }
}
