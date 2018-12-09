using System.Collections.Generic;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class PaymentModel
    {
        public List<BankCardInfo> ListBankCards { get; set; }

        public List<CardTypeInfo> ListCardTypes { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string VipXu { get; set; }

        public string Messages { get; set; }

        public string Amount { get; set; }

        public bool Status { get; set; }

        public string Url { get; set; }

        public int PageName { get; set; }

        public List<NapVipCustomerLogs> CustomerNapVipHistories { get; set; }

        public List<TransactioCustomerLogs> CustomerDoiXuHistories { get; set; }
    }
}