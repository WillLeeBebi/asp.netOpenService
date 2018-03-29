using liemei.Common.cache;
using liemei.Common.common;
using liemei.Dal;
using liemei.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Bll
{
    public class cy_dictBll
    {
        /// <summary>
        /// 获取所有的成语
        /// </summary>
        /// <returns></returns>
        public IList<cy_dict> GetAllcy_dict()
        {
            string key = GetAllKey();
            if (RedisBase.ContainsKey(key))
            {
                IList<cy_dict> cylist = RedisBase.List_GetList<cy_dict>(key);
                if (cylist != null && cylist.Count > 0)
                    return cylist;
            }
            IList<cy_dict> cys =  cy_dictDal.Ins.GetAllcy_dict();
            if (cys!=null && cys.Count>0)
            {
                foreach (var cy in cys)
                {
                    if (cy != null)
                        RedisBase.List_Add(key,cy);
                }
            }
            return cys;
        }

        #region cache
        string GetAllKey()
        {
            return "cy_dictBll.GetAllcy_dict".MD5();
        }
        #endregion
    }
}
