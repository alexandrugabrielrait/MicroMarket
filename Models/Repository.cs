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
            var session = GetSession();
            session.Save(entity);
            try
            {
                session.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            var session = GetSession();
            session.Delete(entity);
            session.Flush();
        }

        public void Update(T entity)
        {
            var session = GetSession();
            session.Update(entity);
            session.Flush();
        }

        public void SaveOrUpdate(T entity)
        {
            var session = GetSession();
            session.SaveOrUpdate(entity);
            session.Flush();
        }
    }
}
