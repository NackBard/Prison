using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Worker
    {
        public Worker()
        {
            AccountingPrisoners = new HashSet<AccountingPrisoner>();
            JournalArrivalAndDepartures = new HashSet<JournalArrivalAndDeparture>();
        }

        public int? Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int GenderId { get; set; }
        public int PostId { get; set; }
        public string AdditionalInformation { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual Post Post { get; set; }
        public virtual ICollection<AccountingPrisoner> AccountingPrisoners { get; set; }
        public virtual ICollection<JournalArrivalAndDeparture> JournalArrivalAndDepartures { get; set; }
    }
}
