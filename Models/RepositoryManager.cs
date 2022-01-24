namespace MicroMarket.Models
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly Dictionary<Type, Object> repoDictionary;
        //public static RepositoryManager? Instance { get; private set; }

        public RepositoryManager(IRepository<Product> productRepository,
            IRepository<ProductType> productTypeRepository,
            IRepository<UserInfo> userInfoRepository
            )
        {
            repoDictionary = new Dictionary<Type, Object>();
            repoDictionary.Add(typeof(Product), productRepository);
            repoDictionary.Add(typeof(ProductType), productTypeRepository);
            repoDictionary.Add(typeof(UserInfo), userInfoRepository);
        }

        public Object Get(Type repoType)
        {
            return repoDictionary.GetValueOrDefault(repoType);
        }

        public bool AddUser(string email)
        {
            var userInfoRepository = ((IRepository<UserInfo>)Get(typeof(UserInfo)));
            var user = userInfoRepository.GetAll().Where(x => x.Email == email).FirstOrDefault();
            if (user != null)
                return false;

            userInfoRepository.Save(new UserInfo() { UserId = Guid.NewGuid(), Email = email });
            return true;
        }
    }
}
