using System;

#nullable disable

namespace Prison.Model
{
    public partial class AccountingDiningVisit : ICloneable
    {
        public int? Id { get; set; }
        public int? SetId { get; set; }
        public int? Prisoner { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
