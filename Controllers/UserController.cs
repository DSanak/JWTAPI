﻿using JWTAPI.Interface;
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

namespace JWTAPI.Controllers
{
    //[Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _IUser;

        public UserController(IUser IUser)
        {
            _IUser = IUser;
        }

        // GET: api/user>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> Get()
        {
            var data = DateTime.Now;
            var t = new Logs
            {
                userID = Singleton.Instance.saveIdusera,
                Descryption = $"Wyswietlenie usera",
                Timestamp = data
            };
            string pathToHtmlFile = $"{Environment.CurrentDirectory}\\Pages\\index.html";
            if (!System.IO.File.Exists(pathToHtmlFile))
            {
                var a = "<h1>Brak takiej strony</h1>";
                return base.Content(a, "html/txt");
            }
            
            

                _IUser.AddLogs(t);
            
                return await Task.FromResult(_IUser.GetUserDetails());
            
        }
        [Route("page")]
        [HttpGet]
        public ActionResult Strona()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/index.html");
            return base.Content(fileContent, "text/html");

           
        }

        [Route("homepage")]
        [HttpGet]
        public ActionResult HomePage()
        {
            var fileContent = System.IO.File.ReadAllText("Pages/home.html");
            return base.Content(fileContent, "text/html");


        }



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
        // POST api/user
        [HttpPost]
        public async Task<ActionResult<UserInfo>> Post(UserInfo users)
        {
            _IUser.AddUser(users);
            return await Task.FromResult(users);
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserInfo>> Put(int id, UserInfo users)
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

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserInfo>> Delete(int id)
        {
            var users = _IUser.DeleteUser(id);
            return await Task.FromResult(users);
        }

        private bool EmployeeExists(int id)
        {
            return _IUser.CheckUser(id);
        }
    }
}