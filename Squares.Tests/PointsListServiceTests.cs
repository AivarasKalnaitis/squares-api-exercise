using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using Squares.Business;
using Squares.Domain.Dtos;
using Squares.Domain.Entities;
using Squares.Infrastructure.Persistance.Repositories;

namespace Squares.Tests
{
    public class PointsListServiceTests
    {
        [Fact]
        public void Get_ReturnsAllPointsLists()
        {
            var mockRepository = new Mock<IPointsListRepository>();
            mockRepository.Setup(repo => repo.Get()).Returns(new List<PointsList>());

            var service = new PointsListService(mockRepository.Object);

            var result = service.Get();

            result.Should().NotBeNull().And.BeEmpty();
        }


        [Fact]
        public void Get_ReturnsPointsListById()
        {
            var pointsListId = 1;
            var mockRepository = new Mock<IPointsListRepository>();
            mockRepository.Setup(repo => repo.Get(pointsListId)).Returns(new PointsList { Id = pointsListId });

            var service = new PointsListService(mockRepository.Object);

            var result = service.Get(pointsListId);

            result.Should()
                .NotBeNull()
                .And.BeOfType<Result<PointsList>>()
                .Which.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(pointsListId);
        }

        [Fact]
        public void Get_ReturnsFailureForNonexistentPointsList()
        {
            var pointsListId = 1;
            var mockRepository = new Mock<IPointsListRepository>();
            mockRepository.Setup(repo => repo.Get(pointsListId)).Returns((PointsList)null);

            var service = new PointsListService(mockRepository.Object);

            var result = service.Get(pointsListId);

            result.Should()
                .NotBeNull()
                .And.BeOfType<Result<PointsList>>()
                .Which.IsFailure.Should().BeTrue();

            result.Error.Should()
                .NotBeNullOrEmpty()
                .And.Contain(pointsListId.ToString());
        }

        [Fact]
        public void Create_ReturnsCreatedPointsList()
        {
            var mockRepository = new Mock<IPointsListRepository>();
            var service = new PointsListService(mockRepository.Object);
            var createDto = new CreatePointsListDto { Points = [new Point(1, 1), new Point(2, 2)] };

            var result = service.Create(createDto);

            result.Should()
                .NotBeNull()
                .And.BeOfType<PointsList>()
                .Which.Points.Should().NotBeNullOrEmpty().And.HaveCount(createDto.Points.Count);
        }


        [Fact]
        public async Task GetSquaresAsync_ReturnsSquares()
        {
            var pointsListId = 1;
            var mockRepository = new Mock<IPointsListRepository>();
            mockRepository.Setup(repo => repo.Get(pointsListId)).Returns(new PointsList { Id = pointsListId, ShouldUpdateSquares = true });

            var service = new PointsListService(mockRepository.Object);

            var result = await service.GetSquaresAsync(pointsListId);

            result.Should()
                .NotBeNull()
                .And.BeOfType<Result<IEnumerable<Square>>>()
                .Which.IsSuccess.Should().BeTrue();
        }

    }
}