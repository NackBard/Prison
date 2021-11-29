using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Prisoner : ICloneable
    {
        public Prisoner()
        {
            AccountingDiningVisits = new HashSet<AccountingDiningVisit>();
            AccountingPrisoners = new HashSet<AccountingPrisoner>();
            AccountingRehabilitationWorks = new HashSet<AccountingRehabilitationWork>();
            SalesAccountings = new HashSet<SalesAccounting>();
        }

        public int? Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirthday { get; set; }
        public int GenderId { get; set; }
        public DateTime DateOfConclusion { get; set; }
        public int ProsecutionId { get; set; }
        public string Verdict { get; set; }
        public int StatusId { get; set; }
        public string AdditionalInformation { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual Prosecution Prosecution { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<AccountingDiningVisit> AccountingDiningVisits { get; set; }
        public virtual ICollection<AccountingPrisoner> AccountingPrisoners { get; set; }
        public virtual ICollection<AccountingRehabilitationWork> AccountingRehabilitationWorks { get; set; }
        public virtual ICollection<SalesAccounting> SalesAccountings { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
