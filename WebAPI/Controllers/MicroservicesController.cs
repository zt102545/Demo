using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MicroservicesController : ControllerBase
    {
        private IConfiguration _iConfiguration;

        public MicroservicesController(IConfiguration configuration)
        {
            this._iConfiguration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Console.WriteLine($"This is HealthController  {this._iConfiguration["port"]} Invoke");

            return Ok();//只是个200 
        }

        [HttpGet]
        public string Get()
        {
            //dotnet WebAPI.dll --urls="http://*:5005" --ip="127.0.0.1" --port=5005
            Console.WriteLine(Request.Host.Value);
            return Request.Host.Value;
        }
    }
}