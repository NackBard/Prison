using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Post : ICloneable
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? LevelId { get; set; }
        public double Salary { get; set; }
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
