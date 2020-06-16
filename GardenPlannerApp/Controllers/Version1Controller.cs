using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GardenPlannerApp.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/versioncontroller")]
    [ApiController]
    public class Version1Controller : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> GetGardens()
        {
            return "1.0";
        }

    }

    [ApiVersion("2.0")]
    [Route("api/versioncontroller")]
    [ApiController]
    public class Version2Controller : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> GetGardens()
        {
            return "2.0";
        }

    }
}