using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Set
    {
        public Set()
        {
            AccountingDiningVisits = new HashSet<AccountingDiningVisit>();
            Dishes = new HashSet<Dish>();
        }

        public int? Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccountingDiningVisit> AccountingDiningVisits { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
