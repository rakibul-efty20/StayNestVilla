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
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaDTO>>>> GetVillas()
        {
            var villas = await _db.Villas.ToListAsync();
            var dtoResponseVilla = _mapper.Map<List<VillaDTO>>(villas);
            var response = ApiResponse<IEnumerable<VillaDTO>>.
                Ok(dtoResponseVilla, "Villas retrieved successfully");
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> GetVillaById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Villa ID must be greater than 0"));
                }

                var villaId = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
                if (villaId == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with ID {id} not found"));
                }
                    return Ok(ApiResponse<VillaDTO>.Ok(_mapper.Map<VillaDTO>(villaId),"Record retrived successfully"));
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while retrieving villa with ID{id} :", ex.Message);
                return StatusCode(500, errorResponse);
                
            }
        }
        [HttpPost]
        public async Task<ActionResult<VillaDTO>> CreateVilla(VillaCreateDTO villaDto)
        {
            try
            {
                if (villaDto == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa data is required"));
                }

                var duplicateVilla = await _db.Villas.FirstOrDefaultAsync(u =>
                    u.Name.ToLower() == villaDto.Name.ToLower());
                if (duplicateVilla != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A villa with the name '{villaDto.Name}' already exist"));
                }

                Villa villa = _mapper.Map<Villa>(villaDto);
              

                await _db.Villas.AddAsync(villa);
                await _db.SaveChangesAsync();

                var response =
                    ApiResponse<VillaDTO>.CreatedAt(_mapper.Map<VillaDTO>(villa), "Villa Created successfully");
                // Return 201 Created with a Location header pointing to the newly created resource
                return CreatedAtAction(nameof(GetVillaById), new { id = villa.Id },response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while creating the villa :", ex.Message);
                return StatusCode(500,errorResponse);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> UpdateVilla(int id,VillaUpdateDTO villaDto)
        {
            try
            {
                if (villaDto == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa data is required"));
                }

                if (id != villaDto.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa ID in URL does not match Villa ID in request body"));
                }

                var existingVilla = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with ID {id} not found"));
                }

                var duplicateVilla = await _db.Villas.FirstOrDefaultAsync(u =>
                    u.Name.ToLower() == villaDto.Name.ToLower() && u.Id != id);
                
                if (duplicateVilla != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A villa with the name '{villaDto.Name}' already exist"));
                }
                _mapper.Map(villaDto,existingVilla);
                existingVilla.UpdatedDate = DateTime.Now;
               
                await _db.SaveChangesAsync();

                var response = ApiResponse<VillaDTO>.Ok(_mapper.Map<VillaDTO>(villaDto),"Villa update successfully");
                return Ok(villaDto);
                
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while updating the villa :", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVilla(int id)
        {
            try
            {
                

                var existingVilla = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with ID {id} not found"));
                   
                }

                _db.Villas.Remove(existingVilla);
                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent("Villa deleted successfully");
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while deleting the villa :", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}
