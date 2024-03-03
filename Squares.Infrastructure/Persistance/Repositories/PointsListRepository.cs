using Squares.Domain.Entities;

namespace Squares.Infrastructure.Persistance.Repositories
{
    public class PointsListRepository(DataContext context) : IPointsListRepository
    {
        private DataContext _context = context;

        public IEnumerable<PointsList> Get()
        {
            return _context.PointsLists;
        }

        public PointsList Get(int id)
        {
            return _context.PointsLists.FirstOrDefault(x => x.Id == id);
        }

        public void Create(PointsList newPointsList)
        {
            _context.PointsLists.Add(newPointsList);
            _context.SaveChanges();
        }

        public void Update(PointsList updatedPointsList)
        {
            _context.PointsLists.Update(updatedPointsList);
            _context.SaveChanges();
        }

        public void Delete(PointsList pointsList)
        {
            _context.PointsLists.Remove(pointsList);
            _context.SaveChanges();
        }
    }
}