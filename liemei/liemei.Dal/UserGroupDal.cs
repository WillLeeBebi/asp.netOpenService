using liemei.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using liemei.Common.common;

namespace liemei.Dal
{
    public class UserGroupDal
    {
        private static UserGroupDal _Ins;
        public static UserGroupDal Ins
        {
            get
            {
                if (_Ins == null)
                    _Ins = new UserGroupDal();
                return _Ins;
            }
        }
        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="ug"></param>
        /// <returns></returns>
        public string Add(UserGroup ug)
        {
            string id = string.Empty;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.Save(ug);
                transaction.Commit();
                session.Close();
                id = ug.ID;
            } catch (Exception ex)
            {
                ClassLoger.Error("UserGroupDal.Add",ex);
            }
            return id;
        }

        public string Update(UserGroup ug)
        {
            string id = string.Empty;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.SaveOrUpdate(ug);
                transaction.Commit();
                session.Close();
                id = ug.ID;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserGroupDal.Add", ex);
            }
            return id;
        }

        public void Del(UserGroup ug)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.Delete(ug);
                transaction.Commit();
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserGroupDal.Del", ex);
            }
        }
        /// <summary>
        /// 根据分组ID分页获取下级分组信息
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Tuple<IList<UserGroup>,long> GetSubUserGroup(string pid,int pageIndex,int pageSize)
        {
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    long count = session.QueryOver<UserGroup>().Where(x => x.PGroupID == pid).RowCount();
                    var list = session.QueryOver<UserGroup>()
                        .Where(x => x.PGroupID == pid)
                        .OrderBy(x=>x.CreateTime).Desc
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .List();
                    return Tuple.Create(list, count);
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("UserGroupDal.GetSubUserGroup",ex);
            }
            return null;
        }

        /// <summary>
        /// 根据分组ID获取下级分组
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IList<UserGroup> GetSubUserGroup(string pid)
        {
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    return  session.QueryOver<UserGroup>().Where(x => x.PGroupID == pid).OrderBy(x=>x.CreateTime).Desc.List();
                }
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserGroupDal.GetSubUserGroup", ex);
            }
            return null;
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
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    return session.QueryOver<UserGroup>()
                        .Where(x => x.EnterpriseID == EnterpriseID && (x.PGroupID == null || x.PGroupID==""))
                        .List();
                }
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserGroupDal.GetUserGroup", ex);
            }
            return null;
        }
        public UserGroup GetByID(string id)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                UserGroup ug = session.Get<UserGroup>(id);
                session.Close();
                return ug;
            } catch (Exception ex)
            {
                ClassLoger.Error("UserGroupDal.GetByID",ex);
            }
            return null;
        }
    }
}
