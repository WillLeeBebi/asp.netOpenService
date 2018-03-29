using liemei.Dal;
using liemei.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Bll
{
    public class UserGroupBll
    {
        public string Add(UserGroup ug)
        {
            return UserGroupDal.Ins.Add(ug);
        }
        public string Update(UserGroup ug)
        {
            return UserGroupDal.Ins.Update(ug);
        }
        public void Del(UserGroup ug)
        {
             UserGroupDal.Ins.Del(ug);
        }
        /// <summary>
        /// 根据分组ID分页获取下级分组信息
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Tuple<IList<UserGroup>, long> GetSubUserGroup(string pid, int pageIndex, int pageSize)
        {
            return UserGroupDal.Ins.GetSubUserGroup(pid,pageIndex,pageSize);
        }
        /// <summary>
        /// 根据分组ID获取所有的下级分组
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IList<UserGroup> GetSubUserGroup(string pid)
        {
            return UserGroupDal.Ins.GetSubUserGroup(pid);
        }
        /// <summary>
        /// 根据企业ID获取企业直属的分组信息
        /// </summary>
        /// <param name="EnterpriseID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<UserGroup> GetUserGroup(string EnterpriseID)
        {
            return UserGroupDal.Ins.GetUserGroup(EnterpriseID);
        }
        public UserGroup GetByID(string id)
        {
            return UserGroupDal.Ins.GetByID(id);
        }
    }
}
