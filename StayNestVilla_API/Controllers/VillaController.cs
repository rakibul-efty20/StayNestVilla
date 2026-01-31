using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StayNestVilla_API.DataBase;
using StayNestVilla_API.DTO;
using StayNestVilla_API.Models;

namespace StayNestVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Villa>>> GetVillas()
        {
            return Ok(await _db.Villas.ToListAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Villa>> GetVillaById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Villa ID must be greater than 0");
                }

                var villaId = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
                if (villaId == null)
                {
                    return NotFound($"Villa with ID {id} not found");
                }
                else
                {
                    return Ok(villaId);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while retrieving villa with ID{id} : {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<Villa>> CreateVilla(VillaCreateDTO villaDto)
        {
            try
            {
                if (villaDto == null)
                {
                    return BadRequest("Villa data is required");
                }

                Villa villa = _mapper.Map<Villa>(villaDto);
              

                await _db.Villas.AddAsync(villa);
                await _db.SaveChangesAsync();

               // return Ok(villaDto);
                // Return 201 Created with a Location header pointing to the newly created resource
                return CreatedAtAction(nameof(GetVillaById), new { id = villa.Id }, villa);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while create : {ex.Message}");
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Villa>> UpdateVilla(int id,VillaUpdateDTO villaDto)
        {
            try
            {
                if (villaDto == null)
                {
                    return BadRequest("Villa data is required");
                }

                if (id != villaDto.Id)
                {
                    return BadRequest("Villa ID in URL does not match Villa ID in request body");
                }

                var existingVilla = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla == null)
                {
                    return NotFound($"Villa with ID {id} not found");
                }

                _mapper.Map(villaDto,existingVilla);
                existingVilla.UpdatedDate = DateTime.Now;
               
                await _db.SaveChangesAsync();

                return Ok(villaDto);
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while updating the villa : {ex.Message}");
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Villa>> DeleteVilla(int id)
        {
            try
            {
                

                var existingVilla = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla == null)
                {
                    return NotFound($"Villa with ID {id} not found");
                }

                _db.Villas.Remove(existingVilla);
                await _db.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while deleting the villa : {ex.Message}");
            }
        }
    }
}
