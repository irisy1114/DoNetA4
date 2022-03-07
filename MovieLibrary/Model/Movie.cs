using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Model
{
    public class Movie
    {
        public UInt64 Id { get; set; }
        public string Title { get; set; }
        public string Genres { get; set; }
    }
}
