using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateNav
{
    public class Archive
    {
        private readonly Config _config;
        private readonly string _relativePath;
        private readonly List<Article> _articles;

        public Archive( Config config, string relativePath, List<Article> articles)
        {
            _config = config;
            _relativePath = relativePath;
            _articles = articles;
        }

        private IEnumerable<string> GetYears()
        {
            var years = new List<string>();
            foreach (var article in _articles)
            {
                string year = article.Published.Year.ToString();
                if (!years.Contains(year)) years.Add(year);
            }
            return years.OrderByDescending(q => q);
        }

        private IEnumerable<string> GetMonthsFromYear(string year)
        {
            var months = new List<int>();
            foreach (var article in _articles)
            {
                if (article.Published.Year.ToString() != year) continue;
                var month = article.Published.Month;
                if (!months.Contains(month)) months.Add(month);
            }
            return months.OrderByDescending(q => q).Select(q => q.ToString());
        }

        private IEnumerable<Article> GetArticlesInMonth(string year, string month)
        {
            return _articles.Where(q => q.Published.Year.ToString() == year && q.Published.Month.ToString() == month).OrderByDescending(q => q.Published);
        }

        public string CreateArchiveMarkDown(string title)
        {
            string markdown = $"# {title} \n\n";
            foreach (var year in GetYears())
            {
                markdown += $"## {year}\n\n";
                foreach (var month in GetMonthsFromYear(year))
                {
                    markdown += $"### {month}\n\n";
                    foreach (var article in GetArticlesInMonth(year, month))
                    {
                        markdown += $"- [{article.Title}]({article.Path})\n";
                    }
                }
            }
            return markdown;
        }

        public void WriteArchive(string title)
        {
            string archiveContent = CreateArchiveMarkDown(title);
            File.WriteAllText(Path.Combine(_config.RepoPath, _relativePath, "index.md"), archiveContent);
        }
    }
}
