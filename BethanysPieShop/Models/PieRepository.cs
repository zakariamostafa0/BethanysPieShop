using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly BethanysPieShopDBContext _bethanysPieShopDBContext;

        public PieRepository(BethanysPieShopDBContext bethanysPieShopDBContext)
        {
            _bethanysPieShopDBContext = bethanysPieShopDBContext;
        }

        public IEnumerable<Pie> AllPies
        {
            get 
            { 
                return _bethanysPieShopDBContext.Pies.Include(c => c.Category); 
            }
        }

        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return _bethanysPieShopDBContext.Pies.Include(c => c.Category).Where(p =>
                    p.IsPieOfTheWeek);
            }
        }

        public Pie? GetPieById(int pieId)
        {
            return _bethanysPieShopDBContext.Pies.FirstOrDefault(p => p.PieId == pieId);
        }

        public IEnumerable<Pie> SearchPies(string searchQuery)
        {
            return _bethanysPieShopDBContext.Pies.Where(p => p.Name.Contains(searchQuery));
        }
    }
}
