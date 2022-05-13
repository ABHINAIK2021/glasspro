using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace MyApp.Models
{
    public class DBModel
    {
        public static DataTable GetDataTable(string Query)
        {
            SqlConnection connection = new SqlConnection();
            DataTable dataTable = new DataTable();
            try
            {
                DBConnection dBConnection = new DBConnection();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand(Query, connection);
                sqlCommand.CommandTimeout = 500;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return dataTable;
        }

        public static List<DashboardModel> Dashboard()
        {
            List<DashboardModel> list = new List<DashboardModel>();
            SqlConnection connection = new SqlConnection();
            try
            {
                DBConnection dBConnection = new DBConnection();
                DataTable dataTable = new DataTable();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("USP_Dashboard", connection);
                sqlCommand.CommandTimeout = 500;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        DashboardModel data = new DashboardModel();
                        data.Counts = Convert.ToInt32(row["Counts"]);
                        list.Add(data);
                    }
                }
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return list;
        }

        public static List<ItemModel> Items()
        {
            List<ItemModel> list = new List<ItemModel>();
            SqlConnection connection = new SqlConnection();
            try
            {
                DBConnection dBConnection = new DBConnection();
                DataTable dataTable = new DataTable();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("USP_Items", connection);
                sqlCommand.CommandTimeout = 500;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ItemModel data = new ItemModel();
                        data.ItemId = Convert.ToInt32(row["ItemId"]);
                        data.ItemName = Convert.ToString(row["ItemName"]);
                        list.Add(data);
                    }
                }
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return list;
        }

        public static DataTable SavePurchase(string PurchaseDate, decimal NetTotal, decimal GrandTotal, int UserId, DataTable data)
        {
            SqlConnection connection = new SqlConnection();
            DataTable dataTable = new DataTable();
            try
            {
                DBConnection dBConnection = new DBConnection();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("USP_SavePurchase", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@PurchaseDate", PurchaseDate);
                sqlCommand.Parameters.AddWithValue("@NetTotal", NetTotal);
                sqlCommand.Parameters.AddWithValue("@GrandTotal", GrandTotal);
                sqlCommand.Parameters.AddWithValue("@UserId", UserId);

                if (data != null)
                {
                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@PT_PurchaseD";
                    sqlParameter.Value = data;
                    sqlParameter.TypeName = "PT_PurchaseD";
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return dataTable;
        }

        public static List<ItemModel> InStockItems()
        {
            List<ItemModel> list = new List<ItemModel>();
            SqlConnection connection = new SqlConnection();
            try
            {
                DBConnection dBConnection = new DBConnection();
                DataTable dataTable = new DataTable();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("USP_InStockItems", connection);
                sqlCommand.CommandTimeout = 500;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ItemModel data = new ItemModel();
                        data.ItemId = Convert.ToInt32(row["ItemId"]);
                        data.ItemName = Convert.ToString(row["ItemName"]);
                        list.Add(data);
                    }
                }
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return list;
        }

        public static DataTable SaveWorkOrder(string WorkOrderDate, string CustomerName, string Address, int PhoneNo, int UserId, DataTable data)
        {
            SqlConnection connection = new SqlConnection();
            DataTable dataTable = new DataTable();
            try
            {
                DBConnection dBConnection = new DBConnection();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("USP_SaveWorkOrder", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@WorkOrderDate", WorkOrderDate);
                sqlCommand.Parameters.AddWithValue("@CustomerName", CustomerName);
                sqlCommand.Parameters.AddWithValue("@Address", Address);
                sqlCommand.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                sqlCommand.Parameters.AddWithValue("@UserId", UserId);

                if (data != null)
                {
                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@PT_WorkOrderD";
                    sqlParameter.Value = data;
                    sqlParameter.TypeName = "PT_WorkOrderD";
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return dataTable;
        }

        public static List<ProductionModel> Productions(string For)
        {
            List<ProductionModel> list = new List<ProductionModel>();
            SqlConnection connection = new SqlConnection();
            try
            {
                DBConnection dBConnection = new DBConnection();
                DataTable dataTable = new DataTable();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("USP_Productions", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@For", For);
                sqlCommand.CommandTimeout = 500;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ProductionModel data = new ProductionModel();
                        data.WorkOrderNo = Convert.ToString(row["WorkOrderNo"]);
                        data.Count = Convert.ToInt32(row["Count"]);
                        list.Add(data);
                    }
                }
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return list;
        }

        public static List<ProductionModel> ProducedQty(string For, string WorkOrderNo)
        {
            List<ProductionModel> list = new List<ProductionModel>();
            SqlConnection connection = new SqlConnection();
            try
            {
                DBConnection dBConnection = new DBConnection();
                DataTable dataTable = new DataTable();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("USP_ProducedQty", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@For", For);
                sqlCommand.Parameters.AddWithValue("@WorkOrderNo", WorkOrderNo);
                sqlCommand.CommandTimeout = 500;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        i++;
                        ProductionModel data = new ProductionModel();
                        data.SrNo = i;
                        data.ItemId = Convert.ToInt32(row["ItemId"]);
                        data.ItemName = Convert.ToString(row["ItemName"]);
                        data.ActualQty = Convert.ToInt32(row["ActualQty"]);
                        data.BalancedQty = Convert.ToInt32(row["BalancedQty"]);
                        data.ProducedQty = Convert.ToInt32(row["ProducedQty"]);
                        data.ActualBalancedQty = Convert.ToInt32(row["BalancedQty"]);
                        data.ActualProducedQty = Convert.ToInt32(row["ProducedQty"]);
                        list.Add(data);
                    }
                }
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return list;
        }

        public static DataTable SaveProduction(string Query, string WorkOrderNo, int UserId, DataTable data)
        {
            SqlConnection connection = new SqlConnection();
            DataTable dataTable = new DataTable();
            try
            {
                DBConnection dBConnection = new DBConnection();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand(Query, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@WorkOrderNo", WorkOrderNo);
                sqlCommand.Parameters.AddWithValue("@UserId", UserId);

                if (data != null)
                {
                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@PT_Production";
                    sqlParameter.Value = data;
                    sqlParameter.TypeName = "PT_Production";
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return dataTable;
        }

        public static bool SendEmail(string EmailId, int OTP, string Message)
        {
            bool IsSent = false;
            try
            {
                string Email = "";
                string Password = "";
                string Host = "";
                int Port = 0;
                DataTable dataTable = new DataTable();
                dataTable = GetDataTable("USP_Email");
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Email = Convert.ToString(row["Email"]);
                        Password = Convert.ToString(row["Password"]);
                        Host = Convert.ToString(row["Host"]);
                        Port = Convert.ToInt32(row["Port"]);
                    }
                }
                if (EmailId != "")
                {
                    using (MailMessage mail = new MailMessage(Email, EmailId))
                    {
                        mail.Subject = "Login Credentials";
                        mail.Body = Message;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = Host;
                        smtp.EnableSsl = true;
                        NetworkCredential networkCredential = new NetworkCredential(Email, Password);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = networkCredential;
                        smtp.Port = Port;
                        smtp.Send(mail);
                        IsSent = true;
                    }
                }
                return IsSent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SaveAttchment(string InvoiceNo, DataTable data)
        {
            SqlConnection connection = new SqlConnection();
            DataTable dataTable = new DataTable();
            try
            {
                DBConnection dBConnection = new DBConnection();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("USP_SaveAttachment", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);

                if (data != null)
                {
                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@PT_Attachment";
                    sqlParameter.Value = data;
                    sqlParameter.TypeName = "PT_Attachment";
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return dataTable;
        }

        public static DataTable SaveItem(int UserId, DataTable data)
        {
            SqlConnection connection = new SqlConnection();
            DataTable dataTable = new DataTable();
            try
            {
                DBConnection dBConnection = new DBConnection();
                connection = dBConnection.GetConnection;
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("USP_SaveMultipleItems", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserId", UserId);

                if (data != null)
                {
                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@PT_Item";
                    sqlParameter.Value = data;
                    sqlParameter.TypeName = "PT_Item";
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw ex;
            }
            return dataTable;
        }
    }
}