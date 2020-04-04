using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZZTCoreAPI.Common;
using ZZTCoreModel;

namespace ZZTCoreAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// GET api/values/5
        /// </summary>
        /// <param name="id">输入ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles ="zzt")]
        public ActionResult<string> Get(int id)
        {
            return "你请求的ID是" + id;
        }

        [HttpGet]
        public ActionResult<string> GetToken(string name, string pwd)
        {
            string jwtStr = string.Empty;
            bool suc = false;
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            //这里直接写死了


            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
            {
                return new JsonResult(new
                {
                    Status = false,
                    message = "用户名或密码不能为空"
                });
            }

            TokenModelJWT tokenModel = new TokenModelJWT();
            tokenModel.Uid = 1;
            tokenModel.Role = name;

            jwtStr = JwtHelper.IssueJWT(tokenModel);
            suc = true;


            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }

        /// <summary>
        /// Post方式提交数据
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> Post(Test test)
        {
            return JsonConvert.SerializeObject(test);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
