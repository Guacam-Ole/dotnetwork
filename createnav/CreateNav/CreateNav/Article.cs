using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateNav
{
    public class Article
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Categories { get; set; } = new List<string>();
        public Version ModifyVersion { get; set; } = new Version("0.0.0");

        public override string ToString()
        {
            return Path;
        }
    }
}
