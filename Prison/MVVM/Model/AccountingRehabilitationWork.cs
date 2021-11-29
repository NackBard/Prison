using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class AccountingRehabilitationWork : ICloneable
    {
        public int? Id { get; set; }
        public int WorkId { get; set; }
        public int PrisonerId { get; set; }
        public double Salary { get; set; }

        public virtual Prisoner Prisoner { get; set; }
        public virtual Work Work { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
