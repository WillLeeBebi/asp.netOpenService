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
    public class WeChatUserBll
    {
        public string AddWeChatUser(WeChatUser user)
        {
            if (user == null)
                return string.Empty;
            string id = WeChatUserDal.Ins.AddWeChatUser(user);
            user.ID = id;
            string key = getGetWeChatUserByUnionIDKey(user.UnionID);
            if (RedisBase.ContainsKey(key))
            {
                RedisBase.Item_Remove(key);
            }
            RedisBase.Item_Set(key, user);

            return id;
        }

        public void UpdateWeChatUser(WeChatUser user)
        {
            WeChatUserDal.Ins.UpdateWeChatUser(user);
            string key = getGetWeChatUserByUnionIDKey(user.UnionID);
            if (RedisBase.ContainsKey(key))
            {
                RedisBase.Item_Remove(key);
            }
            RedisBase.Item_Set(key, user);
        }

        public WeChatUser GetWeChatUserByUnionID(string UnionID)
        {
            string key = getGetWeChatUserByUnionIDKey(UnionID);
            if (RedisBase.ContainsKey(key))
            {
                return RedisBase.Item_Get<WeChatUser>(key);
            }
            else
            {
                WeChatUser wuser = WeChatUserDal.Ins.GetWeChatUserByUnionID(UnionID);
                if (wuser!=null)
                {
                    RedisBase.Item_Set(key, wuser);
                }
                return wuser;
            }
        }

        #region cache key
        string getGetWeChatUserByUnionIDKey(string UnionID)
        {
            return string.Format("getGetWeChatUserByUnionIDKey/{0}", UnionID).MD5();
        }
        #endregion
    }
}
