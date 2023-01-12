using JWTAPI.Interface;
using JWTAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTAPI.Controllers
{
   // [Authorize]
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployees _IEmployee;
        private readonly IUser? _IUser;

        public EmployeeController(IEmployees IEmployee, IUser IUser)
        {
            _IEmployee = IEmployee;
            _IUser = IUser;
        }

        // GET: api/employee>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            var data = DateTime.Now;
            var t = new Logs
            {
                userID = Singleton.Instance.saveIdusera,
                Descryption = $"Wyświetlenie pracowników",
                Timestamp = data
            };

            _IUser.AddLogs(t);

            return await Task.FromResult(_IEmployee.GetEmployeeDetails());



        }

        // GET api/employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            var employees = await Task.FromResult(_IEmployee.GetEmployeeDetails(id));
            if (employees == null)
            {
                return NotFound();
            }
            var data = DateTime.Now;

            var t = new Logs
            {
                userID = Singleton.Instance.saveIdusera,
                Descryption = $"Wyświetlenie pracownika o id: "+id,
                Timestamp = data
            };

            _IUser.AddLogs(t);

            return employees;
        }

        // POST api/employee
        [HttpPost]
        public async Task<ActionResult<Employee>> Post(Employee employee)
        {
            _IEmployee.AddEmployee(employee);

            var data = DateTime.Now;

            var t = new Logs
            {
                userID = Singleton.Instance.saveIdusera,
                Descryption = $"Dodanie pracownika o id: " + employee.EmployeeID,
                Timestamp = data
            };

            _IUser.AddLogs(t);



            return await Task.FromResult(employee);
        }

        // PUT api/employee/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> Put(int id, Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }
            try
            {
                _IEmployee.UpdateEmployee(employee);
                var data = DateTime.Now;

                var t = new Logs
                {
                    userID = Singleton.Instance.saveIdusera,
                    Descryption = $"Update pracownika o id: " + employee.EmployeeID,
                    Timestamp = data
                };

                _IUser.AddLogs(t);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return await Task.FromResult(employee);
        }

        // DELETE api/employee/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> Delete(int id)
        {
            var employee = _IEmployee.DeleteEmployee(id);

            if (employee != null)
            {
                var data = DateTime.Now;

                var t = new Logs
                {
                    userID = Singleton.Instance.saveIdusera,
                    Descryption = $"Usunięcie pracownika o id: " + employee.EmployeeID,
                    Timestamp = data
                };

                _IUser.AddLogs(t);
            return await Task.FromResult(employee);
            }else return NotFound();

        }


        [Route("logowanie")]
        [HttpGet]
        public ActionResult Strona()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/userLogin.html");
            return base.Content(fileContent, "text/html");


        }

        [Route("stylelogowanie.css")]
        [HttpGet]
        public ActionResult LogowanieCss()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/stylelogowanie.css");
            return base.Content(fileContent, "text/css");


        }



        private bool EmployeeExists(int id)
        {
            return _IEmployee.CheckEmployee(id);
        }
    }
}
