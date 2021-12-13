using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Product : ICloneable
    {

        public int? Id { get; set; }
        public string Name { get; set; }
        public int? ProductTypeId { get; set; }
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
