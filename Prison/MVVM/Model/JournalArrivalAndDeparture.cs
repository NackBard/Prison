using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class JournalArrivalAndDeparture
    {
        public int? Id { get; set; }
        public int WorkerId { get; set; }
        public int AccountingTypeId { get; set; }
        public DateTime Date { get; set; }

        public virtual AccountingType AccountingType { get; set; }
        public virtual Worker Worker { get; set; }
    }
}
