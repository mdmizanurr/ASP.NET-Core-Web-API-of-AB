using EAPI.Data;
using EAPI.DTO;
using EAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EAPI.Controllers
{
    [ApiController]
    [Route("api/Employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public EmployeesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get(int pageIndex = 0, int pageSize = 10)
        {
            var query = context.Employees.AsQueryable();

            var recordCount = await query.CountAsync();

            query = context.Employees.Skip(pageIndex * pageSize).Take(pageSize).OrderBy(x => x.Name);

            return await query.ToListAsync();

        }

        [HttpGet("{id:int}", Name = "getEmployee")]
        public async Task<ActionResult<EmployeeDTO>> Get(int id)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(i => i.Id == id);
            if (employee == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "This Id doesn't Exist");
            }

            return StatusCode(StatusCodes.Status200OK, employee);
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmployeeCreateDTO employeeCreateDTO)
        {
            var model = new Employee();

            model.Name = employeeCreateDTO.Name;
            model.Gender = employeeCreateDTO.Gender;
            model.DateOfBirth = employeeCreateDTO.DateOfBirth;
            model.PermanentAddress = employeeCreateDTO.PermanentAddress;
            model.CreatedDate = employeeCreateDTO.CreatedDate = DateTime.Now;

            context.Add(model);
            await context.SaveChangesAsync();

            return NoContent();
        }

        // EDIT
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] EmployeeCreateDTO empDTO)
        {
            var model = new Employee();
            model.Name = empDTO.Name;
            model.Gender = empDTO.Gender;
            model.DateOfBirth = empDTO.DateOfBirth;
            model.PermanentAddress = empDTO.PermanentAddress;
            model.Id = id;

            context.Entry(model).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }


        //DELETE
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(i => i.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            context.Remove(employee);
            await context.SaveChangesAsync();

            return NoContent();
        }




        //End

    }
}
