using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Post
    {
        public Post()
        {
            Workers = new HashSet<Worker>();
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public int LevelId { get; set; }
        public double Salary { get; set; }

        public virtual AccessLevel Level { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
