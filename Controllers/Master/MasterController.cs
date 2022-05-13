using MyApp.Filters;
using MyApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyApp.Controllers.Master
{
    [UserAuthenticationFilter]
    public class MasterController : Controller
    {
        public ActionResult Item()
        {
            return View();
        }

        public JsonResult LoadItem()
        {
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_LoadItem");
            string json = JsonConvert.SerializeObject(dataTable);
            var jsonResult = Json(json, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult SaveItem(int ItemId, string ItemName, decimal Rate, string Unit, int Height, int Width)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            ResponseModel responseModel = new ResponseModel();
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_SaveItem '" + ItemId + "','" + ItemName + "','" + Rate + "','" + Unit + "','" + Height + "','" + Width + "','" + UserId + "'");
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    responseModel.Status = Convert.ToInt32(row["Status"]);
                    responseModel.Message = Convert.ToString(row["Message"]);
                }
            }
            var jsonResult = Json(responseModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult UpdateItem(int ItemId)
        {
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_UpdateItem '" + ItemId + "'");
            string json = JsonConvert.SerializeObject(dataTable);
            var jsonResult = Json(json, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult DeleteItem(int ItemId)
        {
            ResponseModel responseModel = new ResponseModel();
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_DeleteItem '" + ItemId + "'");
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    responseModel.Status = Convert.ToInt32(row["Status"]);
                    responseModel.Message = Convert.ToString(row["Message"]);
                }
            }
            var jsonResult = Json(responseModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult Inventory()
        {
            return View();
        }

        public JsonResult LoadInventory()
        {
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_LoadInventory");
            string json = JsonConvert.SerializeObject(dataTable);
            var jsonResult = Json(json, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult Settings()
        {
            return View();
        }

        public JsonResult LoadSettings()
        {
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_LoadSettings");
            string json = JsonConvert.SerializeObject(dataTable);
            var jsonResult = Json(json, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult Reset()
        {
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_Reset");
            string json = JsonConvert.SerializeObject(dataTable);
            var jsonResult = Json(json, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult Attachment()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult UploadAttachment()
        {
            AttachmentModel attachmentModel = new AttachmentModel();
            if (Request.Files.Count > 0)
            {
                try
                {
                    int UserId = Convert.ToInt32(Session["UserId"]);
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string FileName;
                        string root = Server.MapPath("~/Uploads/Temp/");
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            FileName = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            FileName = file.FileName;
                        }
                        string ContentType;
                        ContentType = MimeMapping.GetMimeMapping(FileName);
                        if (!Directory.Exists(root))
                        {
                            Directory.CreateDirectory(root);
                        }
                        string FilePath = Path.Combine(Server.MapPath("~/Uploads/Temp/"), FileName);
                        if (System.IO.File.Exists(FileName))
                        {
                            System.IO.File.Delete(FileName);
                        }
                        file.SaveAs(FilePath);
                        attachmentModel.DocName = FileName;
                        attachmentModel.FilePath = FilePath;
                        attachmentModel.ContentType = ContentType;
                    }
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            return Json(attachmentModel);
        }

        [HttpPost]
        public ActionResult StoreAttachment(string DocName, string FilePath, string ContentType)
        {
            TempData["DocName"] = DocName;
            TempData["FilePath"] = FilePath;
            TempData["ContentType"] = ContentType;
            int i = 1;
            return Json(i);
        }

        public FileResult DownloadAttachment(int Id)
        {
            string DocName = Convert.ToString(TempData["DocName"]);
            string FilePath = Convert.ToString(TempData["FilePath"]);
            string ContentType = Convert.ToString(TempData["ContentType"]);
            byte[] fileBytes = System.IO.File.ReadAllBytes(FilePath);
            return File(fileBytes, ContentType, DocName);
        }

        public JsonResult GetAttachment(string InvoiceNo)
        {
            List<AttachmentModel> list = new List<AttachmentModel>();
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_ViewAttachment '" + InvoiceNo + "'");
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    AttachmentModel data = new AttachmentModel();
                    data.Type = Convert.ToString(row["Type"]);
                    data.DocName = Convert.ToString(row["DocName"]);
                    data.FilePath = Convert.ToString(row["FilePath"]);
                    data.ContentType = Convert.ToString(row["ContentType"]);
                    list.Add(data);
                }
            }
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ViewAttachment()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult UploadExcel()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            ResponseModel responseModel = new ResponseModel();
            DataTable dataTable = new DataTable();
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string FileName;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            FileName = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            FileName = file.FileName;
                        }
                        string root = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(root))
                        {
                            Directory.CreateDirectory(root);
                        }
                        string FilePath = Path.Combine(Server.MapPath("~/Uploads/"), FileName);
                        file.SaveAs(FilePath);
                        string extension = Path.GetExtension(FilePath);
                        string conString = string.Empty;
                        switch (extension)
                        {
                            case ".xls":
                                conString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'", FilePath);
                                break;
                            case ".xlsx":
                                conString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'", FilePath);
                                break;
                        }
                        using (OleDbConnection oleDbConnection = new OleDbConnection(conString))
                        {
                            using (OleDbCommand oleDbCommand = new OleDbCommand())
                            {
                                using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter())
                                {
                                    DataTable dt = new DataTable();
                                    oleDbCommand.Connection = oleDbConnection;
                                    oleDbConnection.Open();
                                    DataTable table;
                                    table = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string SheetName = table.Rows[0]["TABLE_NAME"].ToString();
                                    oleDbConnection.Close();
                                    oleDbConnection.Open();
                                    oleDbCommand.CommandText = "SELECT * FROM [" + SheetName + "]";
                                    oleDbDataAdapter.SelectCommand = oleDbCommand;
                                    oleDbDataAdapter.Fill(dt);
                                    oleDbConnection.Close();
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataTable data = new DataTable();
                                        data.Columns.Add("ItemName");
                                        data.Columns.Add("Rate");
                                        data.Columns.Add("Unit");
                                        data.Columns.Add("Height");
                                        data.Columns.Add("Width");
                                        data.TableName = "PT_Item";
                                        foreach (DataRow dataRow in dt.Rows)
                                        {
                                            if (dt.Rows.Count > 1)
                                            {
                                                DataRow row = data.NewRow();
                                                row["ItemName"] = Convert.ToString(dataRow["Item Name"]);
                                                row["Rate"] = Convert.ToString(dataRow["Rate"]);
                                                row["Unit"] = Convert.ToString(dataRow["Unit"]);
                                                row["Height"] = Convert.ToString(dataRow["Height"]);
                                                row["Width"] = Convert.ToString(dataRow["Width"]);
                                                data.Rows.Add(row);
                                            }
                                        }
                                        dataTable = DBModel.SaveItem(UserId, data);
                                        if (dataTable.Rows.Count > 0)
                                        {
                                            foreach (DataRow row in dataTable.Rows)
                                            {
                                                responseModel.Status = Convert.ToInt32(row["Status"]);
                                                responseModel.Message = Convert.ToString(row["Message"]);
                                            }
                                        }
                                        if (FilePath != null || FilePath != string.Empty)
                                        {
                                            if ((System.IO.File.Exists(FilePath)))
                                            {
                                                System.IO.File.Delete(FilePath);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    var jsonResult = Json(responseModel, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public ActionResult DownloadExcel()
        {
            string FilePath = "~/Downloads/Item/Item.xlsx";
            if (FilePath != "")
            {
                Response.ContentType = "doc/docx";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + FilePath + "\"");
                Response.TransmitFile(Server.MapPath(FilePath));
                Response.End();
            }
            return View();
        }
    }
}