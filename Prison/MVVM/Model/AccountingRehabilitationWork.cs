using System;

#nullable disable

namespace Prison.Model
{
    public partial class AccountingRehabilitationWork : ICloneable
    {
        public int? Id { get; set; }
        public int? WorkId { get; set; }
        public int? PrisonerId { get; set; }
        public double Salary { get; set; }
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
