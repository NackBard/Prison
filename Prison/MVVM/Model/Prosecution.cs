using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Prosecution : ICloneable
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
