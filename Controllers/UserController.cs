using JWTAPI.Interface;
using JWTAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTAPI.Controllers
{
    [Authorize]
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
            return await Task.FromResult(_IUser.GetUserDetails());
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