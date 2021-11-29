using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class AccountingPrisoner : ICloneable
    {
        public int? Id { get; set; }
        public int WorkerId { get; set; }
        public int PrisonerId { get; set; }
        public int AssessmentId { get; set; }
        public string Content { get; set; }
        public DateTime DateOfEntry { get; set; }

        public virtual BehaviorAssessment Assessment { get; set; }
        public virtual Prisoner Prisoner { get; set; }
        public virtual Worker Worker { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
