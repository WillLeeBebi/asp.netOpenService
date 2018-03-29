using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Model
{
    /// <summary>
    /// 成语
    /// </summary>
    [Serializable]
    public class cy_dict
    {
        public virtual string id { get; set; }

        public virtual string chengyu { get; set; }
    }
}
