using JWTAPI.Interface;
using JWTAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAPI.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly DatabaseContext _context;
        private readonly IUser _IUser;
        private readonly IEmployees _IEmployees;

        public TokenController(IConfiguration config, DatabaseContext context, IUser user, IEmployees employee)
        {
            _configuration = config;
            _context = context;
            _IUser = user;
            _IEmployees = employee;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo _userData)
        {

            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.Email, _userData.Password);



                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("DisplayName", user.DisplayName),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        signingCredentials: signIn);

                    //Add Timestamp for Last login to DB
                    var data = DateTime.Now;
                    //   var user_id = _userData.UserId;

                    var t = new Logs
                    {
                        userID = user.UserId,
                        Descryption = "Use token by user " + user.Email,
                        Timestamp = data
                    };
                    Singleton.Instance.saveIdusera = user.UserId;
                    _IUser.AddLogs(t);


                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("employee")]
        [HttpPost]
        public async Task<IActionResult> PostEmployee(Employee _employeeData)
        {

            if (_employeeData != null && _employeeData.LoginID != null && _employeeData.Password != null)
            {
                var employee = await GetEmployee(_employeeData.LoginID, _employeeData.Password);
                if (employee != null)
                {
                    return Ok();


                }
                else return BadRequest("Invalid credentials");

            }
            else return BadRequest();
        }


        //generate the original string here
        private static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomString = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }




        [HttpGet("getCaptcha")]
        public ActionResult<string> GetCaptcha()
        {
            var oryginalString = RandomString(8);
            var reverseString = new string(oryginalString.Reverse().ToArray());
            Response.ContentType = "application/json";
            return Ok(reverseString);
        }

        [HttpPost("captcha")]
        public ActionResult<bool> VerifyCaptcha([FromBody] CaptchaDTO captchaDTO)
        {
            var oryginalString = new string(captchaDTO.stringfromAPI.Reverse().ToArray());
            if (captchaDTO.userInput == oryginalString)
            {
                return true;
            }
            else
            {
                return BadRequest("Podany błędny kod!");
            }
        }

        public class CaptchaDTO
        {
            public string userInput { get; set; }
            public string stringfromAPI { get; set; }
        }

        private const string SecretKey = "6LdmUPgjAAAAAO_aSEaxiWaO268qBUSfT_3hJpK3";

        [HttpPost("recaptchagoogle")]
        public async Task<IActionResult> VerifyRecaptcha([FromForm] string token)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={SecretKey}&response={token}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
                if (jsonResponse.Value<bool>("success"))
                {
                    return Ok("reCaptcha solved successfully");
                }
                else
                {
                    return BadRequest("reCaptcha failed");
                }
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error verifying reCaptcha");
            }
        }




        private async Task<UserInfo> GetUser(string email, string password)
        {
            return await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
        private async Task<Employee> GetEmployee(string login, string password)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.LoginID == login && e.Password == password);
        }


    }
}
