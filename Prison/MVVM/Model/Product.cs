using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Product : ICloneable
    {
        public Product()
        {
            SalesAccountings = new HashSet<SalesAccounting>();
            Warehouses = new HashSet<Warehouse>();
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }

        public virtual TypeProduct ProductType { get; set; }
        public virtual ICollection<SalesAccounting> SalesAccountings { get; set; }
        public virtual ICollection<Warehouse> Warehouses { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
