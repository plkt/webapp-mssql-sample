using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.Data.SqlClient;
using System.Text;
using SampleWebApplication.Models;


namespace SampleWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Class1> ObjCustomer = new List<Class1>();
            try
            {
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = "<ServerName>.database.windows.net";
                cb.UserID = "<UserName>";
                cb.Password ="<Password>";
                cb.InitialCatalog = "<DB NAME>";

                using (var connection = new SqlConnection(cb.ConnectionString))
                {
                    connection.Open();

                    Submit_Tsql_NonQuery(connection, "3 - Inserts",
                       Build_3_Tsql_Inserts("Home"));


                     ObjCustomer =  Submit_6_Tsql_SelectEmployees(connection);


                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return View(ObjCustomer.ToList());

        }

        private List<Class1> Submit_6_Tsql_SelectEmployees(SqlConnection connection)
        {

            string tsql = Build_6_Tsql_SelectEmployees();
            List<Class1> ObjCustomer = new List<Class1>();

            using (var command = new SqlCommand(tsql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String[] formats = {  @"MM\/dd\/yyyy HH:mm:ss" };

                        string PageName = (string)reader["PageName"];
                        DateTime AccessDate = ((DateTime)reader["AccessDate"]);
                        String date = AccessDate.ToString(formats[0]);

                        Class1 Obj = new Class1();
                        Obj.PageName = PageName;
                        Obj.AccessDate  = date;
                        ObjCustomer.Add(Obj);

                       
                    }
                }
            }
            return ObjCustomer; 
        }

        private string Build_6_Tsql_SelectEmployees()
        {
            return @"
                -- Look at all the final Employees.
                
                    SELECT TOP 1 * 
                    FROM(
                        SELECT TOP 2 * 
                        FROM accessLogs 
                        ORDER BY AccessDate DESC -- here you need to pass column which will provide expected sorting
                        ) t                     
                    ORDER BY AccessDate  ;
                ";
        }

        private string Build_3_Tsql_Inserts(String page)
        {

            return @"
                    -- The company has these departments.
                    INSERT INTO accessLogs
                       (PageName)
                          VALUES
                       ('" + page + "');" ;
        }

        private void Submit_Tsql_NonQuery(SqlConnection connection,
         string tsqlPurpose,
         string tsqlSourceCode,
         string parameterName = null,
         string parameterValue = null)
        {

           {
                Console.WriteLine();
                Console.WriteLine("=================================");
                Console.WriteLine("T-SQL to {0}...", tsqlPurpose);

                using (var command = new SqlCommand(tsqlSourceCode, connection))
                {
                    if (parameterName != null)
                    {
                        command.Parameters.AddWithValue(  // Or, use SqlParameter class.
                           parameterName,
                           parameterValue);
                    }
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " = rows affected.");
                }
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            try
            {
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = "sqlsrvrplkt.database.windows.net";
                cb.UserID = "adminplkt";
                cb.Password = "paSSw0rd";
                cb.InitialCatalog = "sqlsrvrplkt";

                using (var connection = new SqlConnection(cb.ConnectionString))
                {
                    connection.Open();

                    Submit_Tsql_NonQuery(connection, "3 - Inserts",
                       Build_3_Tsql_Inserts("about"));
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }



            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            try
            {
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = "sqlsrvrplkt.database.windows.net";
                cb.UserID = "adminplkt";
                cb.Password = "paSSw0rd";
                cb.InitialCatalog = "sqlsrvrplkt";

                using (var connection = new SqlConnection(cb.ConnectionString))
                {
                    connection.Open();

                    Submit_Tsql_NonQuery(connection, "3 - Inserts",
                       Build_3_Tsql_Inserts("contact"));
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }


            return View();
        }
    }
}