using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class TransactioCustomerLogs
    {
        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }

        [DataMember]
        [DisplayName("Amount")]
        public string Amount { get; set; }

        [DataMember]
        [DisplayName("Type")]
        public string Type { get; set; }

        [DataMember]
        [DisplayName("TransactionCode")]
        public string TransactionCode { get; set; }
    }

    public class NapVipCustomerLogs
    {
        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }

        [DataMember]
        [DisplayName("Amount")]
        public string Amount { get; set; }

        [DataMember]
        [DisplayName("TotalDay")]
        public string TotalDay { get; set; }

        [DataMember]
        [DisplayName("Package")]
        public string Package { get; set; }

        [DataMember]
        [DisplayName("StartDate")]
        public DateTime StartDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextStartDate
        {
            get
            {
                return StartDate.ToString(Extensions.Constants.DateTimeFomatFull);
            }
        }

        [DataMember]
        [DisplayName("EndDate")]
        public DateTime EndDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextEndDate
        {
            get
            {
                return EndDate.ToString(Extensions.Constants.DateTimeFomatFull);
            }
        }
    }
}