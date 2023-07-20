namespace BethanysPieShop.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BethanysPieShopDBContext _bethanysPieShopDBContext;

        public CategoryRepository(BethanysPieShopDBContext bethanysPieShopDBContext)
        {
            _bethanysPieShopDBContext = bethanysPieShopDBContext;
        }

        public IEnumerable<Category> AllCategories
        {
            get 
            {
                return _bethanysPieShopDBContext.Categories.OrderBy(p => p.CategoryName); 
            }
        }
    }
}
