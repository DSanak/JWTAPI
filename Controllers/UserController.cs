using JWTAPI.Interface;
using JWTAPI.Models;
using JWTAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTools_NTS.Util;
using JWTAPI.Pages;
using JWTAPI.Pages.Shared;
using System.Text;
using System.Net;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JWTAPI.Controllers
{
 // [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _IUser;

        public UserController(IUser IUser)
        {
            _IUser = IUser;
        }

        [Route("homepage")]
        [HttpGet]
        public ActionResult HomePage()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/homepage.html");
            return base.Content(fileContent, "text/html");
        }

        [Route("homepage.css")]
        [HttpGet]
        public ActionResult homepagecss()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/homepage.css");
            return base.Content(fileContent, "text/css");
        }


        [Route("logowanie")]
        [HttpGet]
        public ActionResult Strona()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/logowanie.html");
            return base.Content(fileContent, "text/html");


        }

        [Route("stylelogowanie.css")]
        [HttpGet]
        public ActionResult logowaniecss()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/stylelogowanie.css");
            return base.Content(fileContent, "text/css");
        }

       // [Authorize]
        [Route("admin")]
        [HttpGet]
        public ActionResult admin()
        {

            var fileContent = System.IO.File.ReadAllText("Pages/admin.html");
            return base.Content(fileContent, "text/html");


        }

        [Route("styleadmin.css")]
        [HttpGet]
        public ActionResult admincss()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/styleadmin.css");
            return base.Content(fileContent, "text/css");


        }

        [Route("user")]
        [HttpGet]
        public ActionResult Uzytkownik()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/user.html");
            return base.Content(fileContent, "text/html");

        }
        [Route("styleuzytkownik.css")]
        [HttpGet]
        public ActionResult uzytkownikcss()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/styleuzytkownik.css");
            return base.Content(fileContent, "text/css");


        }

        [Authorize]
        // GET: api/user>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> Get()
        {
            var data = DateTime.Now;
            var t = new Logs
            {
                userID = Singleton.Instance.saveIdusera,
                Descryption = $"Wyswietlenie userow",
                Timestamp = data
            };

            _IUser.AddLogs(t);

            return await Task.FromResult(_IUser.GetUserDetails());

        }


        [Authorize]
        // GET api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> Get(int id)
        {
            var users = await Task.FromResult(_IUser.GetUserDetails(id));
            if (users == null)
            {
                return NotFound();
            }
            var data = DateTime.Now;

            var t = new Logs
            {

                userID = Singleton.Instance.saveIdusera,
                Descryption = "Blabla",
                Timestamp = data
            };

            _IUser.AddLogs(t);
            return users;

        }

        [Authorize]
        // POST api/user
        [HttpPost]
        public async Task<ActionResult<UserInfo>> Post(UserInfo users)
        {
            _IUser.AddUser(users);
            return await Task.FromResult(users);
        }

        [Authorize]
        // PUT api/user/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserInfo>> Post(int id, UserInfo users)
        {
            if (id != users.UserId)
            {
                return BadRequest();
            }
            try
            {
                _IUser.UpdateUser(users);
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
            return await Task.FromResult(users);
        }

        [Authorize]
        // DELETE api/user/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserInfo>> Delete(int id)
        {
            var users = _IUser.DeleteUser(id);
            return await Task.FromResult(users);
        }

        [Authorize]
        // PUT api/employee/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserInfo>> Put(int id, UserInfo user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            try
            {
                _IUser.UpdateUser(user);
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
            return await Task.FromResult(user);
        }



        private bool EmployeeExists(int id)
        {
            return _IUser.CheckUser(id);
        }
    }
}