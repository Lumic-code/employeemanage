using EmploymentManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentManagement.Core.Repository
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetEmployeeDetails();
        public Employee GetEmployeeDetails(int id);
        public void AddEmployee(Employee employee);
        public void UpdateEmployee(Employee employee);
        public Employee DeleteEmployee(int id);
        public bool CheckEmployee(int id);
    }
}
