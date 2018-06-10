using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebApiSample.Models;
using WebApiSample.Helpers;

namespace WebApiSample.Controllers
{
    public class HomeController : ApiController
    {
        SqlConnection connection;
        List<DeviceLog> devlogs = new List<DeviceLog>();
        HomeController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["LoggingDB"].ConnectionString;
            connection = new SqlConnection(connectionString);

        }

        [HttpPost]
        public void CreateLog(DeviceLog deviceLog)
        {
            string query = "INSERT INTO dbo.DeviceLog (deviceId, userId, logtime) " +
                   "VALUES (@deviceId, @userId, @logtime) ";

            // create connection and command
            //using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // define parameters and their values
                cmd.Parameters.Add("@deviceId", SqlDbType.VarChar, 50).Value = deviceLog.DeviceId;
                cmd.Parameters.Add("@userId", SqlDbType.VarChar, 50).Value = deviceLog.UserId;
                cmd.Parameters.Add("@logtime", SqlDbType.DateTime).Value = DateTime.Now;

                // open connection, execute INSERT, close connection
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                //String sSQLCommand = "INSERT INTO DeviceLog(deviceId, userId,logtime) VALUES(\"" + deviceLog.DeviceId + "\",\"" + deviceLog.UserId + "\"," + deviceLog.LogTime+")";
                ////var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text, CommandText = sSQLCommand };
                //connection.Open();
                //int nNoAdded = command.ExecuteNonQuery();
                //connection.Close();
            }
        }
        [HttpGet]
        public IEnumerable<DeviceLog> GetDeviceLogs()
        {
            String sSQLCommand = "Select * from DeviceLog ";
            var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text, CommandText = sSQLCommand };
            SqlDataAdapter da = new SqlDataAdapter { SelectCommand = command };
            DataSet ds = new DataSet();
            connection.Open();
            da.Fill(ds);
            IList<DeviceLog> deviceLogs = ds.Tables[0].DataTableToList<DeviceLog>();
            connection.Close();
            return deviceLogs;
        }

    }
}
