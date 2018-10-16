using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servise.Dto
{
    public class Poet
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class JsonPoemDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public Poet Poet { get; set; }
    }
}
