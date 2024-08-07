using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPractice.Models;
using WebApiPractice.Repository.IRepository;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApiPractice.Response;

namespace WebApiPractice.Controllers
{
    [Route("api/nationalPark")]
    [ApiController]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;

        public NationalParkController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;  
        }

        [HttpGet()]
        public async Task<IActionResult> GetNationalParks()
        {
            var nationalParks = await _nationalParkRepository.GetNationalParksAsync();
            return Ok(nationalParks); //200
        }

        [HttpGet("getPaginatedNationalParks")]
        public async Task<ActionResult<NationalParkResponseModel>> GetNationalParks([FromQuery] int pageNo = 1, [FromQuery] int pageSize = 10)
        {
            var (nationalParks, totalRecords) = await _nationalParkRepository.GetPaginatedNationalParksAsync(pageNo, pageSize);

            var response = new NationalParkResponseModel
            {
                NationalParks = nationalParks,
                TotalRecords = nationalParks.Count > 0 ? totalRecords : 0,
                PageLimit = nationalParks.Count > 0 ? pageSize : 0,
            };

            return Ok(response);
        }


        [HttpGet("{id}", Name = "GetNationalPark")]
        public async Task<IActionResult> GetNationalPark(int id)
        {
            var nationalParkInDb = await _nationalParkRepository.GetNationalParkAsync(id);
            if (nationalParkInDb == null)
            {
                return NotFound(); //404
            }
            return Ok(nationalParkInDb);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNationalPark([FromBody] NationalPark nationalPark)
        {
            if (nationalPark == null) 
            {
                return BadRequest(); //400
            }

            if (await _nationalParkRepository.NationalParkExistsAsync(nationalPark.Name))
            {
                ModelState.AddModelError("", "National park already exists");
                return StatusCode(StatusCodes.Status409Conflict, ModelState); //409 Conflict
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _nationalParkRepository.CreateNationalParkAsync(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data {nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState); //500 Internal Server Error
            }
            //return Ok("National park created successfully.");
            return CreatedAtRoute("GetNationalPark", new { id = nationalPark.NationalParkId }, nationalPark); //201 Created
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNationalPark([FromBody] NationalPark nationalPark)
        {
            if (nationalPark == null) 
            {
                return BadRequest(); //400
            }

            if (!await _nationalParkRepository.NationalParkExistsAsync(nationalPark.NationalParkId))
            {
                return NotFound(); //404
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _nationalParkRepository.UpdateNationalParkAsync(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while updating data {nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState); //500 Internal Server Error
            }

            return NoContent(); //204 No Content
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNationalPark(int id)
        {
            if (!await _nationalParkRepository.NationalParkExistsAsync(id))
            {
                return NotFound(); //404
            }

            var nationalPark = await _nationalParkRepository.GetNationalParkAsync(id);
            if (nationalPark == null) return NotFound(); //404

            if (!await _nationalParkRepository.DeleteNationalParkAsync(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting data {nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState); //500 Internal Server Error
            }

            return Ok(); //200 OK
        }
    }
}
