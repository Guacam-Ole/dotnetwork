using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CreateNav
{
    public class ReadMd
    {
        private readonly Config _config;
        private readonly string _relativePath;


        public readonly Dictionary<string, List<Article>> Categories = new();
        public readonly Dictionary<string, List<Article>> Tags = new();
        public int FileCount = 0;

        public ReadMd(Config config, string relativePath)
        {
            _config = config;
            _relativePath = relativePath;
        }

        public List<Article> ReadArticlesFromFolderRecursive(string subPath, string pattern = "*.md")
        {
            string absolutePath = Path.Combine(_config.RepoPath, subPath);
            var articles = new List<Article>();
            var subFolders = new DirectoryInfo(absolutePath).GetDirectories();
            foreach (var subfolder in subFolders)
            {
                articles.AddRange(ReadArticlesFromFolderRecursive(Path.Combine(subPath, subfolder.Name), pattern));
            }
            var files = Directory.GetFiles(absolutePath, pattern);

            foreach (var file in files)
            {
                var article = GetArticleDataFromMarkdownFile(subPath, file);
                if (article == null) continue;
                string path = Path.Combine(absolutePath, file);
                bool tagsAdded=UpdateArticle(path, article);
                bool breadCrumbAdded=AddBreadCrumb(path, article);

                if (tagsAdded || breadCrumbAdded) UpdateVersionTag(path);
                articles.Add(article);
                FileCount++;
            }
		

            WriteCategoryMarkdownFile();
            WriteTagMarkdownFile();
            return articles;
        }

        private void UpdateVersionTag(string path)
        {
            var newFile = new List<string>();
            var content = File.ReadAllLines(path);
            bool headerFound= false;
            foreach (var line in content)
            {
                if (!line.StartsWith("createnav:")) newFile.Add(line);
                if (line == "---" && !headerFound)
                {
                    headerFound = true;
                    newFile.Add($"createnav: \"{_config.Currentversion}\"");
                }
            }
            File.WriteAllLines(path, newFile);
        }

        private DateTime GetDateFromText(string content)
        {
            string textContent = GetTextInQuotes(content);
            if (textContent == null) return new DateTime(1973, 12, 14);
            var dateContents = textContent.Split("-");
            return new DateTime(int.Parse(dateContents[0]), int.Parse(dateContents[1]), int.Parse(dateContents[2]));
        }

        private string CreateArticleTagLine(Article article, string prefix, string path, List<string> elements, Dictionary<string, List<Article>> globalElementList)
        {
            if (elements == null || elements.Count == 0) return null;
            string line = $"_{prefix}_: ";
            bool isFirstElement = true;
            foreach (var element in elements)
            {
                if (!isFirstElement) line += " - ";
                isFirstElement = false;
                string elementLower = element.ToLower();
                line += $"[{elementLower}]({_config.DocumentRoot}/{_relativePath}/{path}#{elementLower})";
                if (!globalElementList.ContainsKey(elementLower)) globalElementList.Add(elementLower, new List<Article>());
                globalElementList[elementLower].Add(article);
            }
            return line;
        }

        private bool AddBreadCrumb(string path, Article article)
        {
            if (article.ModifyVersion >= new Version(0, 0, 2)) return false;
            var newFile = new List<string>();
            var content = File.ReadAllLines(path);

            bool inHeader = false;
            bool afterHeader = false;

            string breadcrumb = $"■ [{_config.BlogName}]({_config.DocumentRoot}) » [{_relativePath}]({_config.DocumentRoot}{_relativePath}) » [{article.Published.Year}]({_config.DocumentRoot}{_relativePath}#{article.Published.Year})  » {article.Published.Month} » {article.Title}" ;

            foreach (var line in content)
            {
                newFile.Add(line);
                if (!inHeader && !afterHeader && line == "---")
                {
                    inHeader = true; 
                    newFile.Add($"createnav: \"{_config.Currentversion}\"");
                }
                else if (inHeader && !afterHeader && line == "---")
                {
                    afterHeader = true;
                    inHeader = false;
                    newFile.Add(breadcrumb);
                    newFile.Add(string.Empty);
                }
            }
            File.WriteAllLines(path, newFile);
            return true;
        }
       
        private bool UpdateArticle(string path, Article article)
        {
            bool updateContents = article.ModifyVersion < new Version(0, 0, 1);
                        
            var newFile = new List<string>();

            var content = File.ReadAllLines(path);
            bool inHeader = false;
            bool afterHeader = false;
            bool finished = false;

            foreach (var line in content)
            {
                newFile.Add(line);
                if (!inHeader && !afterHeader && line == "---")
                {
                    inHeader = true;
                }
                else if (inHeader && !afterHeader && line == "---")
                {
                    afterHeader = true;
                    inHeader = false;
                }
                if (!afterHeader || finished) continue;

                newFile.Add($"# {article.Title}");
                newFile.Add($"_Published:_ {article.Published}");
                newFile.Add(string.Empty);
                string categoryLine = CreateArticleTagLine(article, "Categories", "categories", article.Categories, Categories);
                string tagLine = CreateArticleTagLine(article, "Tags", "tags", article.Tags, Tags);

                if (categoryLine != null)
                {
                    newFile.Add(categoryLine);
                    newFile.Add(string.Empty);
                }
                if (tagLine != null)
                {
                    newFile.Add(tagLine);
                    newFile.Add(string.Empty);
                }

                finished = true;
            }

            if (updateContents)
            {
                File.WriteAllLines(path, newFile);
                return true;
            }
            return false;
        }

        private void WriteCategoryMarkdownFile()
        {
		
            WriteMarkDownFileForGroupedArticles("Categories", "categories.md", Categories);
        }
        private void WriteTagMarkdownFile()
        {
			
            WriteMarkDownFileForGroupedArticles("Tags", "tags.md", Tags);
        }

        private void WriteMarkDownFileForGroupedArticles(string title, string markdownFile, Dictionary<string, List<Article>> groupedArticles)
        {
            string markdownContent = $"# {title}\n\n";
            foreach (var group in groupedArticles.OrderBy(q => q.Key))
            {
                markdownContent += $"## {group.Key}\n";
                foreach (var article in group.Value.OrderBy(q => q.Title))
                {
                    markdownContent += $"- [{article.Title}]({article.Path})\n";
                }
                markdownContent += "\n";
            }

            File.WriteAllText(Path.Combine(_config.RepoPath, _relativePath, markdownFile), markdownContent);
        }

        private string GetTextInQuotes(string content)
        {
            int firstQuote = content.IndexOf('"');
            int secondQuote = content.IndexOf('"', firstQuote + 1);

            if (secondQuote < firstQuote || firstQuote < 0) return null;
            return content.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
        }

        private Article GetArticleDataFromMarkdownFile(string relativePath, string filename)
        {
            var article = new Article();
            var allLines = File.ReadAllLines(filename);
            bool inHeader = false;
            bool inCategories = false;
            bool inTags = false;

            foreach (var line in allLines)
            {
                if (inHeader)
                {
                    if (line == "---") break;   // End of Header
                    if (!line.StartsWith("  -"))  // Categories and tags are displayed as lists
                    {
                        inCategories = false;
                        inTags = false;
                    }
                    if (inCategories)
                    {
                        article.Categories.Add(GetTextInQuotes(line));
                    }
                    else if (inTags)
                    {
                        article.Tags.Add(GetTextInQuotes(line));
                    }
                    else
                    {
                        if (line.StartsWith("createnav:")) article.ModifyVersion = new Version(GetTextInQuotes(line));
                        if (line.StartsWith("title:")) article.Title = GetTextInQuotes(line);
                        if (line.StartsWith("date:")) article.Published = GetDateFromText(line);
                        if (line.StartsWith("categories:")) inCategories = true;
                        if (line.StartsWith("tags:")) inTags = true;
                    }
                }

                if (!inHeader && line == "---") inHeader = true;
            }
            article.Path = Path.Combine(_config.DocumentRoot, relativePath, new FileInfo(filename).Name).Replace("\\", "/").Replace(".md", "");
            if (article.Title == null)
            {
                Console.WriteLine($"No title found:{article.Path}");
                return null;
            }
            //article.Path = Path.Combine(_documentRoot, relativePath, new FileInfo(filename).Name).Replace("\\", "/").Replace(".md", "");
            return article;
        }
    }
}