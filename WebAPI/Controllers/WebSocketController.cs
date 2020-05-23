using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.WebSocket;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        [HttpGet]
        public void SendWebSocket()
        {
            WSTestSend.Send("xxxxxxxxxxxxx");
        }
    }
}