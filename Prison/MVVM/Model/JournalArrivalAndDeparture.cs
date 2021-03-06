using System;

#nullable disable

namespace Prison.Model
{
    public partial class JournalArrivalAndDeparture : ICloneable
    {
        public int? Id { get; set; }
        public int? WorkerId { get; set; }
        public int? AccountingTypeId { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
