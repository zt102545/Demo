using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WebAPI.Common;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const string fileDownloadName = "EPPlusDemo.xlsx";
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult Export()
        {
            EPPlusTool.EPPlus = EPPlusTool.GetEPPlus();
            MemoryStream stream = EPPlusTool.Export(EPPlusTool.EPPlus);
            return File(stream.ToArray(), contentType, fileDownloadName);
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "导入失败，文件为空"
                    });
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream).ConfigureAwait(false);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//EPPlus 5.0版本以上要收费了,所以要加这句代码表示非商用。https://epplussoftware.com/developers/licenseexception
                        using (var package = new ExcelPackage(memoryStream))
                        {
                            var worksheet = package.Workbook.Worksheets[0]; // Tip: To access the first worksheet, try index 1, not 0
                            ICollection<EPPlus> ePPlus = EPPlusTool.Import(package, worksheet);
                            EPPlusTool.EPPlus = ePPlus;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = $"导入异常:{ex.Message}"
                });
            }
            return new JsonResult(new
            {
                data = EPPlusTool.EPPlus,
                success = true,
                message = "导入成功"
            });
        }
    }
}