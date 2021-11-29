using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Warehouse : ICloneable
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }

        public virtual Product Product { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
