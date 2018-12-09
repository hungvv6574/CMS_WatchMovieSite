using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class DownloadCustomerInfo : BaseEntity<long>
    {
        [DataMember]
        [DisplayName("DownloadId")]
        public int DownloadId { get; set; }

        [DataMember]
        [DisplayName("CustomerId")]
        public int CustomerId { get; set; }

        [DataMember]
        [DisplayName("StartDate")]
        public DateTime? StartDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextStartDate
        {
            get
            {
                if (StartDate == null)
                {
                    return "";
                }

                return StartDate.Value.ToString(Extensions.Constants.DateTimeFomatFull);
            }
        }

        [DataMember]
        [DisplayName("EndDate")]
        public DateTime? EndDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextEndDate
        {
            get
            {
                if (EndDate == null)
                {
                    return "";
                }

                return EndDate.Value.ToString(Extensions.Constants.DateTimeFomatFull);
            }
        }

        [DataMember]
        [DisplayName("VipXu")]
        public int VipXu { get; set; }

        [DataMember]
        [DisplayName("Day")]
        public int Day { get; set; }
    }
}