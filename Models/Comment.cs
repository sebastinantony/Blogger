//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Blogger.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public Comment()
        {
            this.Posts = new HashSet<Post>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment1 { get; set; }
        public string Publish { get; set; }
        public string Rating { get; set; }
    
        public virtual ICollection<Post> Posts { get; set; }
    }
}
