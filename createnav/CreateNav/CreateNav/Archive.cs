using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateNav
{
    public class Archive
    {
        private readonly string _documentRoot;
        private List<Article> _articles;

        public Archive(string documentRoot, List<Article> articles)
        {
            _documentRoot = documentRoot;
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
            var months = new List<string>();
            foreach (var article in _articles)
            {
                if (article.Published.Year.ToString() != year) continue;
                string month = article.Published.Month.ToString();
                if (!months.Contains(month)) months.Add(month);
            }
            return months.OrderByDescending(q => q);
        }

        private IEnumerable<Article> GetArticlesInMonth(string year, string month)
        {
            return _articles.Where(q => q.Published.Year.ToString() == year && q.Published.Month.ToString() == month).OrderByDescending(q => q.Published);
        }

        public string CreateArchiveMarkDown(string title)
        {
            string markdown = title+"\n";
            foreach (var year in GetYears())
            {
                markdown += $"  - {year}\n";
                foreach (var month in GetMonthsFromYear(year))
                {
                    markdown += $"    - {month}\n";
                    foreach (var article in GetArticlesInMonth(year, month))
                    {
                        markdown += $"      [{article.Title}]({_documentRoot}{article.Path})\n";
                    }
                }
            }
            return markdown;
        }
    }
}
