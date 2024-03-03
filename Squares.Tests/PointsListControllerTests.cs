using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Squares.Business;
using Squares.Controllers;
using Squares.Domain.Dtos;
using Squares.Domain.Entities;

namespace Squares.Tests
{
    public class PointsListsControllerTests
    {
        [Fact]
        public void GetAll_ReturnsOkResultWithEmptyList()
        {
            var mockService = new Mock<IPointsListService>();
            mockService.Setup(service => service.Get()).Returns(new List<PointsList>());

            var controller = new PointsListsController(mockService.Object);

            var result = controller.GetAll();

            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var pointsLists = Assert.IsType<List<PointsList>>(okResult.Value);
            Assert.Empty(pointsLists);
        }

        [Fact]
        public void GetAll_ReturnsOkResultWithPointsList()
        {
            var pointsList = new PointsList { Id = 1 };
            var mockService = new Mock<IPointsListService>();
            mockService.Setup(service => service.Get()).Returns(new List<PointsList> { pointsList });

            var controller = new PointsListsController(mockService.Object);

            var result = controller.GetAll();

            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var pointsLists = Assert.IsType<List<PointsList>>(okResult.Value);
            Assert.Single(pointsLists);
            Assert.Equal(pointsList, pointsLists[0]);
        }

        [Fact]
        public void Get_ReturnsOkResultWithPointsList()
        {
            var pointsListId = 1;
            var pointsList = new PointsList { Id = pointsListId };
            var mockService = new Mock<IPointsListService>();
            mockService.Setup(service => service.Get(pointsListId)).Returns(Result.Success(pointsList));

            var controller = new PointsListsController(mockService.Object);

            var result = controller.Get(pointsListId);

            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultPointsList = Assert.IsType<PointsList>(okResult.Value);
            Assert.Equal(pointsList, resultPointsList);
        }

        [Fact]
        public void Get_ReturnsBadRequestForFailureResult()
        {
            var pointsListId = 1;
            var mockService = new Mock<IPointsListService>();
            mockService.Setup(service => service.Get(pointsListId)).Returns(Result.Failure<PointsList>("Error"));

            var controller = new PointsListsController(mockService.Object);

            var result = controller.Get(pointsListId);

            Assert.NotNull(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Error", errorMessage);
        }

        [Fact]
        public async Task GetSquaresAsync_ReturnsBadRequestForFailureResult()
        {
            var pointsListId = 1;
            var mockService = new Mock<IPointsListService>();
            mockService.Setup(service => service.GetSquaresAsync(pointsListId)).ReturnsAsync(Result.Failure<IEnumerable<Square>>("Error"));

            var controller = new PointsListsController(mockService.Object);

            var result = await controller.GetSquaresAsync(pointsListId);

            Assert.NotNull(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Error", errorMessage);
        }

        [Fact]
        public void AddPoint_ReturnsOkResultForSuccessResult()
        {
            var addPointDto = new AddPointDto { PointsListId = 1, X = 2, Y = 3 };
            var mockService = new Mock<IPointsListService>();
            mockService.Setup(service => service.AddPoint(addPointDto)).Returns(Result.Success<PointsList>(new PointsList()));

            var controller = new PointsListsController(mockService.Object);

            var result = controller.AddPoint(addPointDto);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddPoint_ReturnsBadRequestForFailureResult()
        {
            var addPointDto = new AddPointDto { PointsListId = 1, X = 2, Y = 3 };
            var mockService = new Mock<IPointsListService>();
            mockService.Setup(service => service.AddPoint(addPointDto)).Returns(Result.Failure<PointsList>("Error"));

            var controller = new PointsListsController(mockService.Object);

            var result = controller.AddPoint(addPointDto);

            Assert.NotNull(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Error", errorMessage);
        }
    }
}
