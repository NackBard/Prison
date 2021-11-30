using System;

#nullable disable

namespace Prison.Model
{
    public partial class SalesAccounting : ICloneable
    {
        public int? Id { get; set; }
        public DateTime Date { get; set; }
        public int? ProductId { get; set; }
        public int? PrisonerId { get; set; }
        public int Count { get; set; }
        public double Total { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
