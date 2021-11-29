using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class BehaviorAssessment : ICloneable
    {
        public BehaviorAssessment()
        {
            AccountingPrisoners = new HashSet<AccountingPrisoner>();
        }

        public int? Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccountingPrisoner> AccountingPrisoners { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
