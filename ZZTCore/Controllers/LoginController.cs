using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ZZTCore.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (MySqlConnection con = new MySqlConnection("Data Source=localhost;User ID=root;Password=zt102545;Database=employees;Allow User Variables=True;Charset=utf8;"))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    MySqlDataAdapter sda = new MySqlDataAdapter("select * from titles", con);
                    sda.Fill(ds);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return View();
        }
    }
}