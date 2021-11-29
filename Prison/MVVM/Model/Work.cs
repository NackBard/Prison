﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Prison.Model
{
    public partial class Work
    {
        public Work()
        {
            AccountingRehabilitationWorks = new HashSet<AccountingRehabilitationWork>();
        }

        public int? Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccountingRehabilitationWork> AccountingRehabilitationWorks { get; set; }
    }
}
