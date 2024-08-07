using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPractice.Models;
using WebApiPractice.Repository.IRepository;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApiPractice.Controllers
{
    [Route("api/trail")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;

        public TrailController(ITrailRepository trailRepository)
        {
            _trailRepository = trailRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrails([FromQuery] int nationalParkId)
        {
            ICollection<Trail> trails;
            if (nationalParkId > 0)
            {
                trails = await _trailRepository.GetTrailsInNationalParkAsync(nationalParkId);
            }
            else
            {
                trails = await _trailRepository.GetTrailsAsync();
            }

            return Ok(trails);
        }

        [HttpGet("{id}", Name = "GetTrail")]
        public async Task<IActionResult> GetTrail(int id)
        {
            var trail = await _trailRepository.GetTrailAsync(id);
            if (trail == null) return NotFound();
            return Ok(trail);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrail(int id)
        {
            if (!await _trailRepository.TrailExistsAsync(id))
                return NotFound();

            var trail = await _trailRepository.GetTrailAsync(id);
            if (trail == null) return NotFound();

            if (!await _trailRepository.DeleteTrailAsync(trail))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the trail: {trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrail([FromBody] Trail trail)
        {
            if (trail == null) return BadRequest("Trail object is null."); // 400 Bad Request

            if (await _trailRepository.TrailExistsAsync(trail.Name))
            {
                ModelState.AddModelError("", $"Trail already in DB: {trail.Name}");
                return StatusCode(StatusCodes.Status409Conflict, ModelState); // 409 Conflict
            }

            if (!await _trailRepository.CreateTrailAsync(trail))
            {
                ModelState.AddModelError("", $"Something went wrong while saving the trail: {trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState); // 500 Internal Server Error
            }

            return CreatedAtRoute("GetTrail", new { id = trail.Id }, trail); // 201 Created
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTrail([FromBody] Trail trail)
        {
            if (trail == null) return BadRequest("Trail object is null."); // 400 Bad Request

            if (!await _trailRepository.UpdateTrailAsync(trail))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the trail: {trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError); // 500 Internal Server Error
            }

            return Ok();
        }
    }
}
