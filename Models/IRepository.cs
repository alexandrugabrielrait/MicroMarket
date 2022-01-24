namespace MicroMarket.Models
{
    public interface IRepository<T> where T : class
    {
        public abstract T Get(int id);
        public abstract IQueryable<T> GetAll();
        public abstract void Save(T entity);
        public abstract void Delete(T entity);
        public abstract void Update(T entity);
        public abstract void SaveOrUpdate(T entity);
    }
}
