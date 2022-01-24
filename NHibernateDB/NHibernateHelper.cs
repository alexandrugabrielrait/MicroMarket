using NHibernate;
using ISession = NHibernate.ISession;

namespace MicroMarket.NHibernateDB
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    _sessionFactory = CreateSessionFactory();

                return _sessionFactory;
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            NHibernate.Cfg.Configuration _configuration = new NHibernate.Cfg.Configuration();
            _configuration.Configure();
            return _configuration.BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
