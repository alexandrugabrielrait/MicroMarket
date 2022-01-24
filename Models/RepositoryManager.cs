namespace MicroMarket.Models
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly Dictionary<Type, Object> repoDictionary;
        //public static RepositoryManager? Instance { get; private set; }

        public RepositoryManager(IRepository<Product> productRepository, IRepository<ProductType> productTypeRepository)
        {
            repoDictionary = new Dictionary<Type, Object>();
            repoDictionary.Add(typeof(Product), productRepository);
            repoDictionary.Add(typeof(ProductType), productTypeRepository);
        }

        public Object Get(Type repoType)
        {
            return repoDictionary.GetValueOrDefault(repoType);
        }
    }
}
