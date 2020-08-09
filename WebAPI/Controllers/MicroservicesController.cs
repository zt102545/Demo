using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MicroservicesController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            //dotnet WebAPI.dll --urls="http://*:5005" --ip="127.0.0.1" --port=5005
            return Request.Host.Value;
        }
    }
}