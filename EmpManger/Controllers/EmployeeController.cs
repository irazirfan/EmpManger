using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmpManger.Models;

namespace EmpManger.Controllers
{
    [RoutePrefix("Api/v1")]
    public class EmployeeController : ApiController
    {
        Entities entities = new Entities();

        [HttpGet]
        [Route("GetEmployees")]
        public IQueryable<Employee> GetEmployees()
        {
            try
            {
                return entities.Employees;
            }
            catch(Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public IHttpActionResult GetEmployeeById(int id)
        {
            Employee employee = new Employee();
            try
            {
                employee = entities.Employees.Find(id);
                if(employee == null)
                {
                    return NotFound();
                }
            }
            catch(Exception)
            {
                throw;
            }
            return Ok(employee);
        }

        [HttpPost]
        [Route("AddEmployee")]
        public IHttpActionResult AddEmployee(Employee data)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                entities.Employees.Add(data);
                entities.SaveChanges();
            }
            catch(Exception)
            {
                throw;
            }
            return Ok(data);
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public IHttpActionResult UpdateEmployee(Employee emp)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Employee employee = new Employee();
                employee = entities.Employees.Find(emp.Id);
                if(employee != null)
                {
                    employee.Name = emp.Name;
                    employee.Email = emp.Email;
                    employee.Address = emp.Address;
                    employee.Phone = emp.Phone;
                }
                int i = this.entities.SaveChanges();
            }
            catch(Exception)
            {
                throw;
            }
            return Ok(emp);
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = entities.Employees.Find(id);
            if(employee == null)
            {
                return NotFound();
            }

            entities.Employees.Remove(employee);
            entities.SaveChanges();

            return Ok(employee);
        }

        [HttpPost]
        [Route("DeleteEmployees")]
        public IHttpActionResult DeleteEmployees(int[] ids)
        {
            List<Employee> employees = new List<Employee>();
            foreach( var id in ids)
            {
                var selectedEmployees = entities.Employees.Find(id);
                employees.Add(selectedEmployees);
            }
            entities.Employees.RemoveRange(employees);
            entities.SaveChanges();

            return Ok(employees);
        }
    }
}
