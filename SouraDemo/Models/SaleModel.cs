using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SouraDemo.Models
{
    public class SaleModel
    {
        public string SoldAt { get; set; }
        public string SoldTo { get; set; }
        public string AccountNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerPO { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal InvoiceTotal { get; set; }
        public string ProductNumber { get; set; }
        public int OrderQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal UnitNet { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal LineTotal { get; set; }

    }
}