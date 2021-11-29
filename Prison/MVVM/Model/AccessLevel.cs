using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class AccessLevel : ICloneable
    {
        public AccessLevel()
        {
            Posts = new HashSet<Post>();
        }

        public int? Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
