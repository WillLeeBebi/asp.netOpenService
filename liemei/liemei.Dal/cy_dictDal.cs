using liemei.Common.common;
using liemei.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Dal
{
    public class cy_dictDal
    {
        private static cy_dictDal _Ins;
        public static cy_dictDal Ins
        {
            get
            {
                if (_Ins == null)
                    _Ins = new cy_dictDal();
                return _Ins;
            }
        }

        public IList<cy_dict> GetAllcy_dict()
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                IList<cy_dict> alist = session.QueryOver<cy_dict>().List();
                session.Close();
                return alist;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("cy_dictDal.GetAllcy_dict", ex);
            }
            return null;
        }

    }
}
