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
        /// <summary>
        /// 心跳检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();//只是个200状态码
        }

        /// <summary>
        /// 测试接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            Console.WriteLine(Request.Host.Value);
            return Request.Host.Value;
        }
    }
}