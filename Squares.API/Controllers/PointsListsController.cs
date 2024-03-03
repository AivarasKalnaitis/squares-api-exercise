using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Squares.Business;
using Squares.Domain.Dtos;
using Squares.Domain.Entities;
using System.Diagnostics;

namespace Squares.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointsListsController(IPointsListService pointsListService): ControllerBase
    {
        private readonly IPointsListService _pointsListService = pointsListService;
        private readonly TimeSpan MaxResponseTime = TimeSpan.FromSeconds(5);

        [HttpGet]
        public IActionResult GetAll()
        {
            var pointsLists = _pointsListService.Get();

            return Ok(pointsLists);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var pointsListResult = _pointsListService.Get(id);
            if (pointsListResult.IsFailure)
            {
                return BadRequest(pointsListResult.Error);
            }

            return Ok(pointsListResult.Value);
        }

        [HttpPost]
        public ActionResult<PointsList> Create(CreatePointsListDto pointsListDto)
        {
            var createdPointList = _pointsListService.Create(pointsListDto);

            return Created($"players/{createdPointList.Id}", createdPointList);
        }

        [HttpGet("{id}/squares")]
        public async Task<IActionResult> GetSquaresAsync(int id)
        {
            var stopwatch = Stopwatch.StartNew();
            var squaresResult = await _pointsListService.GetSquaresAsync(id);

            if (squaresResult.IsFailure)
            {
                return BadRequest(squaresResult.Error);
            }


            Metrics.CreateHistogram("get_squares_duration_seconds", "Duration of GetSquares operation", new HistogramConfiguration
            {
                LabelNames = ["status"]
            })
             .WithLabels(squaresResult.IsFailure ? "failure" : "success")
             .Observe(stopwatch.Elapsed.TotalSeconds);


            return Ok(squaresResult.Value);
        }


        [HttpPost("addpoint")]
        public IActionResult AddPoint(AddPointDto addPointDto)
        {
            var addPointResult = _pointsListService.AddPoint(addPointDto);
            if (addPointResult.IsFailure)
            {
                return BadRequest(addPointResult.Error);
            }

            return Ok(addPointResult.Value);
        }

        [HttpPost("removepoint")]
        public IActionResult RemovePoint(RemovePointDto removePointDto)
        {
            var removePointResult = _pointsListService.RemovePoint(removePointDto);
            if (removePointResult.IsFailure)
            {
                return BadRequest(removePointResult.Error);
            }

            return Ok(removePointResult.Value);
        }

        [HttpDelete]
        public IActionResult Delete(int pointsListId)
        {
            var deleteResult = _pointsListService.Delete(pointsListId);
            if (deleteResult.IsFailure)
            {
                return BadRequest(deleteResult.Error);
            }

            return NoContent();
        }
    }
}