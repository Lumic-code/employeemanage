using EmploymentManagement.Core.Entities;
using EmploymentManagement.Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebApi.Controllers
{
    [Authorize]
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: api/employee>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                return await Task.FromResult(_employeeRepository.GetEmployeeDetails());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }

        }

        // GET: api/employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await Task.FromResult(_employeeRepository.GetEmployeeDetails(id));
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Employee Not Found");
            }
                
        }

        // POST: api/employee
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                _employeeRepository.AddEmployee(employee);
                return await Task.FromResult(employee);
                //return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeID }, employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating an employee");
            }

        }

        // PUT: api/employee/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> PutEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.EmployeeID)
                {
                    return BadRequest();
                }
                _employeeRepository.UpdateEmployee(employee);
            }
            catch (Exception)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error updating an employee");
                }
            }
            return await Task.FromResult(employee);

        }

        // DELETE: api/employee/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            try
            {
                var employee = await Task.FromResult(_employeeRepository.DeleteEmployee(id));
                if (employee == null)
                {
                    return NotFound();
                }
                return employee;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting an employee");
            }

        }

        private bool EmployeeExists(int id)
        {
            return _employeeRepository.CheckEmployee(id);
        }
    }
}
