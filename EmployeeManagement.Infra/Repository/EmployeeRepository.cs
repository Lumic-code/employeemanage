using EmployeeManagement.Infra.Context;
using EmploymentManagement.Core.Entities;
using EmploymentManagement.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infra.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
         readonly EmployeeContext _dbcontext = new();

        public EmployeeRepository(EmployeeContext dbcontext)
        {
            _dbcontext = dbcontext;
            
        }

        public void AddEmployee(Employee employee)
        {
            try
            {
                _dbcontext.Employees.Add(employee);
                _dbcontext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckEmployee(int id)
        {
            return _dbcontext.Employees.Any(e => e.EmployeeID == id);
        }

        public Employee DeleteEmployee(int id)
        {
            try
            {
                Employee? employee = _dbcontext.Employees.Find(id);

                if (employee != null)
                {
                    _dbcontext.Employees.Remove(employee);
                    _dbcontext.SaveChanges();
                    return employee;
                }
                else
                {
                    throw new Exception("Employee not found");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Employee> GetEmployeeDetails()
        {
            try
            {
                return _dbcontext.Employees.ToList();
            }
            catch
            {
                throw;
            }
        }

        public Employee GetEmployeeDetails(int id)
        {
            try
            {
                Employee employee = _dbcontext.Employees.Find(id);
                if (employee != null)
                {
                    return employee;
                }
                else
                {
                    throw new Exception("Employee not found");
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            try
            {
                _dbcontext.Entry(employee).State = EntityState.Modified;
                _dbcontext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
