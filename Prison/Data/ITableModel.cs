using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prison.Data
{
    public interface ITableModel
    {
        bool CanRecover { get; }
        void Recover();
        void Drop();
    }
}
