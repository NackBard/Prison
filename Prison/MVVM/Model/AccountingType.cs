using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class AccountingType
    {
        public AccountingType()
        {
            JournalArrivalAndDepartures = new HashSet<JournalArrivalAndDeparture>();
        }

        public int? Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<JournalArrivalAndDeparture> JournalArrivalAndDepartures { get; set; }
    }
}
