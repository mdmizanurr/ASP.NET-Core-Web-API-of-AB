using EAPI.Entities;

namespace EAPI.Services
{
    public interface IEmployeeRepository
    {
        Employee GetById(int id);
        Task<List<Employee>> GetEmployees();
    }
}
