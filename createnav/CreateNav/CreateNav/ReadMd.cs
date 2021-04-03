using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CreateNav
{
    public class ReadMd
    {
        private readonly string _rootPath;
        private readonly string _relativePath;
        private readonly string _documentRoot;
        private readonly Version _currentVersion;


        private readonly Dictionary<string, List<Article>> _categories = new();
        private readonly Dictionary<string, List<Article>> _tags = new();

        public ReadMd(string rootPath, string relativePath, string documentRoot, Version currentVersion)
        {
            _rootPath = rootPath;
            _relativePath = relativePath;
            _documentRoot = documentRoot;
            _currentVersion = currentVersion;
        }

        public List<Article> ReadArticlesFromFolderRecursive(string subPath, string pattern = "*.md")
        {
            string absolutePath = Path.Combine(_rootPath, subPath);
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
                UpdateArticle(Path.Combine(absolutePath, file), article);
                articles.Add(article);

            }

            WriteCategoryMarkdownFile();
            WriteTagMarkdownFile();
            return articles;
        }

        private DateTime GetDateFromText(string content)
        {
            string textContent = GetTextInQuotes(content);
            if (textContent == null) return new DateTime(1973, 12, 14);
            var dateContents = textContent.Split("-");
            return new DateTime(int.Parse(dateContents[0]), int.Parse(dateContents[1]), int.Parse(dateContents[2]));
        }


        private void UpdateArticle(string path, Article article)
        {
            if (article.ModifyVersion == _currentVersion) return;


            var newFile = new List<string>();


            // Check old version if new versions arrive...
            {

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
                    newFile.Add($"_{article.Published}_");
                    int maxLength = article.Categories.Count;
                    if (article.Tags.Count > maxLength) maxLength = article.Tags.Count;

                    if (maxLength > 0)
                    {
                        //newFile.Add($"Categories|Tags");
                        //newFile.Add("-|-");
                        if (article.Categories.Count > 0)
                        {
                            newFile.Add("|Categories|");
                            newFile.Add("|-|");

                            string catLine = "|";
                            bool isFirst = true;
                            foreach (var category in article.Categories)
                            {
                                if (!isFirst) catLine += " :black_small_square: ";
                                isFirst = false;
                                string categoryName = category.ToLower();
                                catLine += $"[{categoryName}]({_documentRoot}/{_relativePath}/categories#{categoryName})";
                                if (!_categories.ContainsKey(categoryName)) _categories.Add(categoryName, new List<Article>());
                                _categories[categoryName].Add(article);
                            }
                            catLine += "|";
                            newFile.Add(catLine);
                        }
                        newFile.Add(string.Empty);
                        if (article.Tags.Count > 0)
                        {
                            newFile.Add("|Tags|");
                            newFile.Add("|-|");

                            string tagLine = "|";
                            bool isFirst = true;
                            foreach (var tag in article.Tags)
                            {
                                if (!isFirst) tagLine += " :black_small_square: ";
                                isFirst = false;
                                string tagName = tag.ToLower();
                                tagLine += $"[{tagName}]({_documentRoot}/{_relativePath}/tags#{tagName})";
                                if (!_categories.ContainsKey(tagName)) _categories.Add(tagName, new List<Article>());
                                _categories[tagName].Add(article);
                            }
                            tagLine += "|";
                            newFile.Add(tagLine);
                        }
                        newFile.Add("\n");
                    }
                    finished = true;
                }

                File.WriteAllLines(path, newFile);
            }
        }

        private void WriteCategoryMarkdownFile()
        {
            WriteMarkDownFileForGroupedArticles("Categories", "categories.md", _categories);
        }
        private void WriteTagMarkdownFile()
        {
            WriteMarkDownFileForGroupedArticles("Tags", "tags.md", _tags);
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
            }

            File.WriteAllText(Path.Combine(_rootPath, _relativePath, markdownFile), markdownContent);
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
            article.Path = Path.Combine(_documentRoot, relativePath, new FileInfo(filename).Name).Replace("\\", "/").Replace(".md", "");
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