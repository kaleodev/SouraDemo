using OfficeOpenXml;
using SouraDemo.Data;
using SouraDemo.Helpers;
using SouraDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SouraDemo.Controllers
{
    public class HomeController : Controller
    {
        public AdventureWorks2017Entities advContext;
        public HomeController()
        {
            advContext = new AdventureWorks2017Entities();
        }
        public ActionResult Index()
        {
            var today = DateTime.Now;
            ReportQuery rq = new ReportQuery
            {
                StartDate = new DateTime(today.Year, today.Month,1).AddMonths(-1),
                EndDate = new DateTime(today.Year, today.Month, 1).AddDays(-1)
            };
            return View(rq);
        }

        [HttpPost]
        public ActionResult Index(ReportQuery query)
        {
            var results = advContext.SalesOrderDetails.Where(x => x.SalesOrderHeader.DueDate >= query.StartDate && x.SalesOrderHeader.DueDate <= query.EndDate).Select(x => new SaleModel
            {
                AccountNumber = x.SalesOrderHeader.Customer.AccountNumber,
                CustomerPO = x.SalesOrderHeader.PurchaseOrderNumber,
                DueDate = x.SalesOrderHeader.DueDate,
                InvoiceNumber = x.SalesOrderHeader.SalesOrderNumber,
                OrderDate = x.SalesOrderHeader.OrderDate,
                SoldAt = x.SalesOrderHeader.Customer.Store.Name,
                SoldTo = x.SalesOrderHeader.Customer.Person.FirstName + " " + x.SalesOrderHeader.Customer.Person.LastName,
                ProductNumber = x.SpecialOfferProduct.Product.ProductNumber,
                OrderQty = x.OrderQty,
                UnitNet = x.UnitPrice,
                LineTotal = x.LineTotal,
                InvoiceTotal = x.SalesOrderHeader.TotalDue
            }).Take(15).ToList();
            this.ViewBag.SalesReport = results;
            return View(query);
        }

        

        public FileResult ExportToExcel(ReportQuery query)
        {
            var results = advContext.SalesOrderDetails.Where(x => x.SalesOrderHeader.DueDate >= query.StartDate && x.SalesOrderHeader.DueDate <= query.EndDate).Select(x => new SaleModel
            {
                AccountNumber = x.SalesOrderHeader.Customer.AccountNumber,
                CustomerPO = x.SalesOrderHeader.PurchaseOrderNumber,
                DueDate = x.SalesOrderHeader.DueDate,
                InvoiceNumber = x.SalesOrderHeader.SalesOrderNumber,
                OrderDate = x.SalesOrderHeader.OrderDate,
                SoldAt = x.SalesOrderHeader.Customer.Store.Name,
                SoldTo = x.SalesOrderHeader.Customer.Person.FirstName + " " + x.SalesOrderHeader.Customer.Person.LastName,
                ProductNumber = x.SpecialOfferProduct.Product.ProductNumber,
                OrderQty = x.OrderQty,
                UnitNet = x.UnitPrice,
                LineTotal = x.LineTotal,
                InvoiceTotal = x.SalesOrderHeader.TotalDue
            }).Take(15).ToList();

            var pkg = new ExcelPackage();
            var wbk = pkg.Workbook;
            var sheet = wbk.Worksheets.Add("Sales Report");

            var normalStyle = "Normal";
            var acctStyle = wbk.CreateAccountingFormat();

            var data = results.ToArray();

            var columns = new[]
            {
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Sold At", Style = normalStyle, Action = i => i.SoldAt, },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Sold To", Style = normalStyle, Action = i => i.SoldTo, },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Account Number", Style = normalStyle, Action = i => i.AccountNumber, },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Invoice Number", Style = normalStyle, Action = i => i.InvoiceNumber, },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Customer PO", Style = normalStyle, Action = i => i.CustomerPO, },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Order Date", Style = normalStyle, Action = i => i.OrderDate.ToString("yyyy-MM-dd"), },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Due Date", Style = normalStyle, Action = i => i.DueDate.ToString("yyyy-MM-dd"), },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Invoice Total", Style = acctStyle, Action = i => i.InvoiceTotal, },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Product Number", Style = normalStyle, Action = i => i.ProductNumber, },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Order Qty", Style = normalStyle, Action = i => i.OrderQty, },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Unit Net", Style = acctStyle, Action = i => i.UnitNet, },
                new SpreadsheetBuilder.ColumnTemplate<SaleModel> { Title = "Line Total", Style = acctStyle, Action = i => i.LineTotal, },
            };

            sheet.SaveData(columns, data);

            var bytes = pkg.GetAsByteArray();
            return File(new MemoryStream(bytes, 0, bytes.Length), "application/octet-stream", "SalesReport-"+DateTime.Now+".xlsx");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}