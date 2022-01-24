namespace MicroMarket.Models
{
    public interface IRepositoryManager
    {
        public Object Get(Type repoType);

        public Guid AddUser(string username);
    }
}
