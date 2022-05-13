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

namespace MyApp.Controllers.WorkOrder
{
    [UserAuthenticationFilter]
    public class WorkOrderController : Controller
    {
        public ActionResult WorkOrder()
        {
            List<ItemModel> list = DBModel.InStockItems();
            ViewBag.Items = new SelectList(list, "ItemId", "ItemName");
            return View();
        }

        public JsonResult LoadWorkOrder()
        {
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_LoadWorkOrder");
            string json = JsonConvert.SerializeObject(dataTable);
            var jsonResult = Json(json, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult InStockQtyWorkOrder(int ItemId)
        {
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_InStockQtyWorkOrder '" + ItemId + "'");
            string json = JsonConvert.SerializeObject(dataTable);
            var jsonResult = Json(json, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult SaveWorkOrder(string WorkOrderDate, string CustomerName, string Address, int PhoneNo, List<ProductionModel> Production, List<AttachmentModel> Attchments)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            ResponseModel responseModel = new ResponseModel();
            DataTable dataTable = new DataTable();
            DataTable dt = new DataTable();
            if (Production != null)
            {
                DataTable data = new DataTable();
                data.Columns.Add("ItemId");
                data.Columns.Add("ItemName");
                data.Columns.Add("Qty");
                data.Columns.Add("Cutting");
                data.Columns.Add("Polishing");
                data.Columns.Add("Fabrication");
                data.Columns.Add("Toughening");
                data.Columns.Add("DGU");
                data.TableName = "PT_WorkOrderD";
                foreach (ProductionModel production in Production)
                {
                    DataRow row = data.NewRow();
                    row["ItemId"] = production.ItemId;
                    row["ItemName"] = production.ItemName;
                    row["Qty"] = production.Qty;
                    row["Cutting"] = production.Cutting;
                    row["Polishing"] = production.Polishing;
                    row["Fabrication"] = production.Fabrication;
                    row["Toughening"] = production.Toughening;
                    row["DGU"] = production.DGU;
                    data.Rows.Add(row);
                }
                dataTable = DBModel.SaveWorkOrder(WorkOrderDate, CustomerName, Address, PhoneNo, UserId, data);
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
                    string root = Server.MapPath("~/Uploads/Work Order/" + responseModel.Data + "/");
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }
                    string OldFilePath = Attchments[i].FilePath;
                    string NewFilePath = Path.Combine(Server.MapPath("~/Uploads/Work Order/" + responseModel.Data + "/"), Attchments[i].DocName);
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