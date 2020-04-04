using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.DAL;
using EFCore.Model;
using Microsoft.AspNetCore.Mvc;

namespace EFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DataDBContext _context;
        public ValuesController(DataDBContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            #region 新增
            //Province province = new Province
            //{
            //    //int类型的ID默认为主键自增，所以不需要添加。
            //    name="北京",
            //    population=200000
            //};
            //Company company = new Company
            //{
            //    Name = "腾讯",
            //    CreateTime = new DateTime(),
            //    LegalPerson = "小马哥"
            //};
            //_context.AddRange(province, company);
            //_context.SaveChanges();
            #endregion

            #region 查询
            //var province = _context.Provinces.Where(x => x.name == "北京").ToList();
            #endregion

            #region 修改
            var province = _context.Provinces.FirstOrDefault();
            if (province != null)
            {
                province.population += 100;
                _context.Provinces.Add(new Province
                {
                    name = "上海",
                    population = 200000
                });
                _context.SaveChanges();
            }
            #endregion
            #region 删除
            //var province = _context.Provinces.FirstOrDefault();
            //_context.Remove(province);
            //_context.SaveChanges();
            #endregion
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
