using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class CustomerHistoriesInfo : BaseEntity<long>
    {
        [DataMember]
        [DisplayName("CustomerId")]
        public int CustomerId { get; set; }
        
        [DataMember]
        [DisplayName("CreateDate")]
        public System.DateTime CreateDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextCreateDate
        {
            get
            {
                return CreateDate.ToString(Extensions.Constants.DateTimeFomatFull);
            }
        }
        
        [DataMember]
        [DisplayName("Action")]
        public string Action { get; set; }
        
        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }
        
        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }

        [DataMember]
        [DisplayName("Type")]
        public int Type { get; set; }

        [DataMember]
        [DisplayName("TransactionCode")]
        public string TransactionCode { get; set; }

        [DataMember]
        [DisplayName("Package")]
        public string Package { get; set; }
        
        [DataMember]
        [DisplayName("Amount")]
        public string Amount { get; set; }

        [DataMember]
        [DisplayName("TotalDay")]
        public string TotalDay { get; set; }
        
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
    }

    public class CustomerHistoriesMap : EntityTypeConfiguration<CustomerHistoriesInfo>, IEntityTypeConfiguration
    {

        public CustomerHistoriesMap()
        {
            ToTable("Modules_CustomerHistories");
            HasKey(m => m.Id);
            Property(m => m.CustomerId).IsRequired();
            Property(m => m.CreateDate).IsRequired();
            Property(m => m.Action).IsRequired().HasMaxLength(250);
            Property(m => m.Description).IsRequired().HasMaxLength(2000);
            Property(m => m.TransactionCode).HasMaxLength(50);
            Property(m => m.Package).HasMaxLength(250);
            Property(m => m.Amount).HasMaxLength(50);
            Property(m => m.TotalDay).HasMaxLength(50);
        }
    }
}
