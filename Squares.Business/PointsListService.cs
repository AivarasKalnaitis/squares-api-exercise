using CSharpFunctionalExtensions;
using Serilog;
using Squares.Domain;
using Squares.Domain.Dtos;
using Squares.Domain.Entities;
using Squares.Infrastructure.Persistance.Repositories;

namespace Squares.Business
{
    public class PointsListService(IPointsListRepository pointsListRepository) : IPointsListService
    {
        private readonly IPointsListRepository _pointsListRepository = pointsListRepository;
        private readonly TimeSpan MaxOperationTime = TimeSpan.FromSeconds(5);

        public IEnumerable<PointsList> Get()
        {
            return _pointsListRepository.Get();
        }

        public Result<PointsList> Get(int id)
        {
            var pointsList = _pointsListRepository.Get(id);
            if(pointsList == null)
            {
                return Result.Failure<PointsList>($"Points list with id {id} not fonud");
            }

            return Result.Success(pointsList);
        }

        public PointsList Create(CreatePointsListDto pointsList)
        {
            var newPointsList = new PointsList { Points = [] };
            foreach (var point in pointsList.Points)
            {
                if (!newPointsList.Points.Contains(point))
                {
                    newPointsList.Points.Add(point);
                }
            }

            _pointsListRepository.Create(newPointsList);

            return newPointsList;
        }

        public async Task<Result<IEnumerable<Square>>> GetSquaresAsync(int pointsListId)
        {
            var pointsList = _pointsListRepository.Get(pointsListId);
            if (pointsList is null)
            {
                return Result.Failure<IEnumerable<Square>>($"Points list with id {pointsListId} not found");
            }

            if (!pointsList.ShouldUpdateSquares)
            {
                return Result.Success(pointsList.Squares.AsEnumerable());
            }

            try
            {
                var newSquares = await FindAndUpdateSquaresWithTimeoutAsync(pointsList);

                return Result.Success(newSquares.AsEnumerable());
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<IEnumerable<Square>>("Request timed out");
            }
        }

        private async Task<IEnumerable<Square>> FindAndUpdateSquaresWithTimeoutAsync(PointsList pointsList)
        {
            var cancellationTokenSource = new CancellationTokenSource(MaxOperationTime);
            var cancellationToken = cancellationTokenSource.Token;

            try
            {
                var squareFindingTask = SquareFinder.FindSquaresAsync(pointsList.Points, cancellationToken);
                var timeoutTask = Task.Delay(MaxOperationTime, cancellationToken);

                var completedTask = await Task.WhenAny(squareFindingTask, timeoutTask);

                return await squareFindingTask;
            }
            catch (OperationCanceledException)
            {
                Log.Information("Square finding operation timed out.");
                throw;
            }
        }

        public Result<PointsList> AddPoint(AddPointDto addPointDto)
        {
            var pointsList = _pointsListRepository.Get(addPointDto.PointsListId);
            if (pointsList is null)
            {
                return Result.Failure<PointsList>($"Points list with id {addPointDto.PointsListId} not found");
            }

            var newPoint = new Point(addPointDto.X, addPointDto.Y);
            if (pointsList.Points.Contains(newPoint))
            {
                return Result.Failure<PointsList>("This point already exists in points list");
            }

            pointsList.Points.Add(newPoint);
            pointsList.ShouldUpdateSquares = true;

            _pointsListRepository.Update(pointsList);

            return Result.Success(pointsList);
        }

        public Result<PointsList> RemovePoint(RemovePointDto removePointDto)
        {
            var pointsList = _pointsListRepository.Get(removePointDto.PointsListId);
            if (pointsList is null)
            {
                return Result.Failure<PointsList>($"Points list with id {removePointDto.PointsListId} not found");
            }

            var pointToRemove = new Point(removePointDto.X, removePointDto.Y);
            if (!pointsList.Points.Contains(pointToRemove))
            {
                return Result.Failure<PointsList>($"Point {removePointDto.X};{removePointDto.Y} does not belong to this list");
            }

            pointsList.Points.Remove(pointToRemove);
            pointsList.Squares = SquareFinder.UpdateSquaresAfterRemovingPoint(pointsList.Squares, pointToRemove);

            _pointsListRepository.Update(pointsList);

            return Result.Success(pointsList);
        }

        public Result Delete(int id)
        {
            var pointList = _pointsListRepository.Get(id);
            if(pointList is null)
            {
                return Result.Failure($"Points list with id {id} not found");
            }

            _pointsListRepository.Delete(pointList);

            return Result.Success();
        }
    }
}