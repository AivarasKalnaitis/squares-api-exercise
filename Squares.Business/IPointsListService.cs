using CSharpFunctionalExtensions;
using Squares.Domain.Dtos;
using Squares.Domain.Entities;

namespace Squares.Business
{
    public interface IPointsListService
    {
        IEnumerable<PointsList> Get();
        Result<PointsList> Get(int id);
        PointsList Create(CreatePointsListDto pointsList);
        Task<Result<IEnumerable<Square>>> GetSquaresAsync(int pointsListId);
        Result<PointsList> AddPoint(AddPointDto addPointDto);
        Result<PointsList> RemovePoint(RemovePointDto removePointDto);
        Result Delete(int pointsListId);
    }
}