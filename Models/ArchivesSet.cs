using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogger.Models
{
    public class ArchivesSet
    {
        public string Month;
        public int Year;
        public int Count;
        public IQueryable<Post> PostList;
    }

    public class Dates 
    {
        public int Month;
        public int Year;
    }
}