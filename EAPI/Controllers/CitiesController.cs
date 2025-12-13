using EAPI.Data;
using EAPI.DTO;
using EAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EAPI.Controllers
{
    [Authorize]
    [Route("api/Cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Cities
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ApiResult<City>>> GetCities(int pageIndex = 0, int pageSize = 10, string? sortColumn = null,
                                                                    string? sortOrder = null, string? filterColumn = null, string? filterQuery = null)
        {
            return await ApiResult<City>.CreateAsync(_context.Cities
                                                    .AsNoTracking(),
                                                        pageIndex,
                                                        pageSize,
                                                        sortColumn,
                                                        sortOrder,
                                                        filterColumn,
                                                        filterQuery);
        }

        // GET: api/Cities/5      
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [Authorize("RegisteredUser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int id, City city)
        {
            if (id != city.Id)
            {
                return BadRequest();
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [Authorize("RegisteredUser")]
        [HttpPost]
        [Route("AddCity")]
        public async Task<ActionResult<City>> AddCity(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.Id }, city);
        }

        // DELETE: api/Cities/5

        [Authorize("Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }

        // Duplicate Check

        [HttpPost]
        [Route("IsDuplicateCity")]
        public bool IsDuplicateCity(City city)
        {
            return _context.Cities.Any(e =>
            e.Name == city.Name
            && e.Lat == city.Lat
            && e.Lon == city.Lon
            && e.CountryId == city.CountryId
            && e.Id != city.Id
            );
        }


    }
}
