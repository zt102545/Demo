using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using WebAPI.Models;

namespace WebAPI.Common
{
    public class EPPlusTool
    {
        private static ICollection<EPPlus> _EPPlus;
        public static ICollection<EPPlus> EPPlus
        {
            get
            {
                return _EPPlus;
            }
            set
            {
                _EPPlus = value;
            }
        }
        public static ICollection<EPPlus> GetEPPlus()
        {
            ICollection<EPPlus> ePPlus = new List<EPPlus>();
            Random r = new Random();//随机范围取值
            ePPlus.Add(new EPPlus { Name = "张三", Age = 12, Gender = 0, Achievement = r.Next(0, 750) });
            ePPlus.Add(new EPPlus { Name = "李四", Age = 18, Gender = Gender.女, Achievement = r.Next(0, 750) });
            ePPlus.Add(new EPPlus { Name = "王五", Age = 12, Gender = 0, Achievement = r.Next(0, 750) });

            _EPPlus = ePPlus;
            return ePPlus;
        }
      
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="ePPlus"></param>
        /// <returns></returns>
        public static MemoryStream Export(ICollection<EPPlus> ePPlus)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//EPPlus 5.0版本以上要收费了,所以要加这句代码表示非商用。https://epplussoftware.com/developers/licenseexception
            MemoryStream stream = new MemoryStream();
            ExcelPackage package = new ExcelPackage(stream);

            package.Workbook.Worksheets.Add("EPPlus");
            ExcelWorksheet sheet = package.Workbook.Worksheets[0];

            #region write header
            sheet.Cells[1, 1].Value = "用户名";
            sheet.Cells[1, 2].Value = "年龄";
            sheet.Cells[1, 3].Value = "性别";
            sheet.Cells[1, 4].Value = "成绩";

            using (ExcelRange range = sheet.Cells[1, 1, 1, 4])
            {
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                range.Style.Border.Bottom.Color.SetColor(Color.Black);
                range.AutoFitColumns(4);
            }
            #endregion

            #region write content
            int pos = 2;
            foreach (EPPlus s in ePPlus)
            {
                sheet.Cells[pos, 1].Value = s.Name;
                sheet.Cells[pos, 2].Value = s.Age;
                sheet.Cells[pos, 3].Value = s.Gender;
                sheet.Cells[pos, 4].Value = s.Achievement;

                using (ExcelRange range = sheet.Cells[pos, 1, pos, 4])
                {
                    range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                }

                pos++;
            }
            #endregion

            package.Save();

            return stream;
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="package"></param>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static ICollection<EPPlus> Import(ExcelPackage package, ExcelWorksheet worksheet)
        {
            ICollection<EPPlus> ePPlus = new List<EPPlus>();

            var rowCount = worksheet.Dimension?.Rows;
            var colCount = worksheet.Dimension?.Columns;
            #region check excel format
            if (!rowCount.HasValue || !colCount.HasValue)
            {
                return ePPlus;
                //return "文档表头或内容为空，请下载对应文档！";
            }
            if(!worksheet.Cells[1,1].Value.Equals("Name") ||
                !worksheet.Cells[1, 2].Value.Equals("Age") ||
                !worksheet.Cells[1, 3].Value.Equals("Gender") ||
                !worksheet.Cells[1, 4].Value.Equals("Achievement") ||
                !worksheet.Cells[2, 1].Value.Equals("用户名") ||
                !worksheet.Cells[2, 2].Value.Equals("年龄") ||
                !worksheet.Cells[2, 3].Value.Equals("性别") ||
                !worksheet.Cells[2, 4].Value.Equals("成绩"))
            {
                return ePPlus;
                //return "文档表头或内容不对应，请下载对应文档！";
            }
            #endregion
            #region  read datas  若读取数据库，可写事务拼接sql执行，前端重新加载页面即可
            for(int i=3; i<= rowCount; i++)
            {
                ePPlus.Add(new EPPlus
                {
                    Name = worksheet.Cells[i, 1].Value.ToString(),
                    Age = Convert.ToInt32(worksheet.Cells[i, 2].Value.ToString()),
                    Gender = (Gender)Enum.Parse(typeof(Gender), worksheet.Cells[i, 3].Value.ToString()),
                    Achievement = Convert.ToDouble(worksheet.Cells[i,4].Value.ToString())
                }) ;
            }
            #endregion
            return ePPlus;
        }
    }
}