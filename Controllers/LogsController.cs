using JWTAPI.Interface;
using JWTAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWTAPI.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController :ControllerBase
    {
        private readonly ILogs _ILogs;

        public LogsController(ILogs ILogs)
        {
            _ILogs = ILogs;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Logs>>> Get()
        {

            return await Task.FromResult(_ILogs.GetLogs());

        }



    }
}
