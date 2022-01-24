using MicroMarket.NHibernateDB;

namespace MicroMarket.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private NHibernate.ISession GetSession()
        {
            return NHibernateHelper.OpenSession();
        }

        public void Save(T entity)
        {
            GetSession().Save(entity);
        }

        public T Get(int id)
        {
            return GetSession().Get<T>(id);
        }

        public IQueryable<T> GetAll()
        {
            return GetSession().Query<T>();
        }

        public void Delete(T entity)
        {
            GetSession().Delete(entity);
        }

        public void Update(T entity)
        {
            GetSession().Update(entity);
        }

        public void SaveOrUpdate(T entity)
        {
            GetSession().SaveOrUpdate(entity);
        }
    }
}
