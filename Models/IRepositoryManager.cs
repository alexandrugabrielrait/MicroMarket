namespace MicroMarket.Models
{
    public interface IRepositoryManager
    {
        public Object Get(Type repoType);

        public bool AddUser(string username);
    }
}
