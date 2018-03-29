using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Dal
{
    public class NHibernateSessionFactory
    {
        private static readonly ISessionFactory sessionFactory;

        private static string HibernateHbmXmlFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase , "hibernate.cfg.xml");

        static NHibernateSessionFactory()
        {
            if (sessionFactory==null)
            {
                sessionFactory = new Configuration().Configure(HibernateHbmXmlFileName).BuildSessionFactory();
            }
         }
        public static ISessionFactory getSessionFactory()
        {

            return sessionFactory;

        }
        public static ISession getSession()
        {
            return sessionFactory.OpenSession();

        }
        public static void closeSessionFactory()
        {
            sessionFactory.Close();
            sessionFactory.Dispose();
        }
    }
}
