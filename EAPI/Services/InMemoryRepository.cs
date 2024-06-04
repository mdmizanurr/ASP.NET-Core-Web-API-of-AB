using EAPI.Entities;

namespace EAPI.Services
{
    public class InMemoryRepository : IEmployeeRepository
    {
        private List<Employee> _employees;
        public InMemoryRepository()
        {
            _employees = new List<Employee>()
            {
                new Employee() {Id = 1, Name= "Employee1", DateOfBirth= DateTime.Now},
                new Employee() {Id = 2, Name= "Employee2", DateOfBirth= DateTime.Now }
            };
        }


        public async Task<List<Employee>> GetEmployees()
        {
            await Task.Delay(1);
            return _employees;
        }

        public Employee GetById(int id)
        {
            return _employees.FirstOrDefault(e => e.Id == id);
        }

    }
}
