using MyApp.Filters;
using MyApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyApp.Controllers.Purchase
{
    [UserAuthenticationFilter]
    public class PurchaseController : Controller
    {
        public ActionResult Purchase()
        {
            List<ItemModel> list = DBModel.Items();
            ViewBag.Items = new SelectList(list, "ItemId", "ItemName");
            return View();
        }

        public JsonResult LoadPurchase()
        {
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_LoadPurchase");
            string json = JsonConvert.SerializeObject(dataTable);
            var jsonResult = Json(json, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult RatePurchase(int ItemId)
        {
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_RatePurchase '" + ItemId + "'");
            string json = JsonConvert.SerializeObject(dataTable);
            var jsonResult = Json(json, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult SavePurchase(string PurchaseDate, decimal NetTotal, decimal GrandTotal, List<ItemModel> Items, List<AttachmentModel> Attchments)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            ResponseModel responseModel = new ResponseModel();
            DataTable dataTable = new DataTable();
            DataTable dt = new DataTable();
            if (Items != null)
            {
                DataTable data = new DataTable();
                data.Columns.Add("ItemId");
                data.Columns.Add("ItemName");
                data.Columns.Add("Rate");
                data.Columns.Add("Qty");
                data.Columns.Add("Amount");
                data.Columns.Add("DiscountPercentage");
                data.Columns.Add("DiscountAmount");
                data.Columns.Add("NetAmount");
                data.Columns.Add("GSTPercentage");
                data.Columns.Add("GSTAmount");
                data.Columns.Add("GrossAmount");
                data.TableName = "PT_PurchaseD";
                foreach (ItemModel item in Items)
                {
                    DataRow row = data.NewRow();
                    row["ItemId"] = item.ItemId;
                    row["ItemName"] = item.ItemName;
                    row["Rate"] = item.Rate;
                    row["Qty"] = item.Qty;
                    row["Amount"] = item.Amount;
                    row["DiscountPercentage"] = item.DiscountPercentage;
                    row["DiscountAmount"] = item.DiscountAmount;
                    row["NetAmount"] = item.NetAmount;
                    row["GSTPercentage"] = item.GSTPercentage;
                    row["GSTAmount"] = item.GSTAmount;
                    row["GrossAmount"] = item.GrossAmount;
                    data.Rows.Add(row);
                }
                dataTable = DBModel.SavePurchase(PurchaseDate, NetTotal, GrandTotal, UserId, data);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        responseModel.Status = Convert.ToInt32(row["Status"]);
                        responseModel.Message = Convert.ToString(row["Message"]);
                        responseModel.Data = Convert.ToString(row["Data"]);
                    }
                }
            }
            if (Attchments != null)
            {
                for (int i = 0; i < Attchments.Count; i++)
                {
                    string root = Server.MapPath("~/Uploads/Purchase/" + responseModel.Data + "/");
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }
                    string OldFilePath = Attchments[i].FilePath;
                    string NewFilePath = Path.Combine(Server.MapPath("~/Uploads/Purchase/" + responseModel.Data + "/"), Attchments[i].DocName);
                    if (!System.IO.File.Exists(NewFilePath))
                    {
                        System.IO.File.Move(OldFilePath, NewFilePath);
                    }
                    Attchments[i].FilePath = NewFilePath;
                }

                DataTable data = new DataTable();
                data.Columns.Add("Type");
                data.Columns.Add("DocName");
                data.Columns.Add("FilePath");
                data.Columns.Add("ContentType");
                data.TableName = "PT_Attachment";
                foreach (AttachmentModel item in Attchments)
                {
                    DataRow row = data.NewRow();
                    row["Type"] = item.Type;
                    row["DocName"] = item.DocName;
                    row["FilePath"] = item.FilePath;
                    row["ContentType"] = item.ContentType;
                    data.Rows.Add(row);
                }
                dt = DBModel.SaveAttchment(responseModel.Data, data);
            }
            var jsonResult = Json(responseModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}