using Squares.Domain.Entities;

namespace Squares.Infrastructure.Persistance.Repositories
{
    public interface IPointsListRepository
    {
        IEnumerable<PointsList> Get();
        PointsList Get(int id);
        void Create(PointsList newPointsList);
        void Update(PointsList updatedPointsList);
        void Delete(PointsList pointsList);
    }
}