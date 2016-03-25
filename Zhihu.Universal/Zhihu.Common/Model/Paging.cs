using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhihu.Common.Model
{
    public interface IPaging
    {
        Paging Paging { get; set; }
        Object[] GetItems();
    }
}
