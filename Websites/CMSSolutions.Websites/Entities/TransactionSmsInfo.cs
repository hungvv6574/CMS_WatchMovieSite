using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class TransactionSmsInfo : BaseEntity<long>
    {
        [DataMember]
        [DisplayName("PartnerId")]
        public string PartnerId { get; set; }

        [DataMember]
        [DisplayName("MoId")]
        public string MoId { get; set; }

        [DataMember]
        [DisplayName("MtId")]
        public string MtId { get; set; }

        [DataMember]
        [DisplayName("UserId")]
        public string UserId { get; set; }

        [DataMember]
        [DisplayName("ShortCode")]
        public string ShortCode { get; set; }

        [DataMember]
        [DisplayName("Keyword")]
        public string Keyword { get; set; }

        [DataMember]
        [DisplayName("Subkeyword")]
        public string Subkeyword { get; set; }

        [DataMember]
        [DisplayName("Content")]
        public string Content { get; set; }

        [DataMember]
        [DisplayName("TransDate")]
        public string TransDate { get; set; }

        [DataMember]
        [DisplayName("CheckSum")]
        public string CheckSum { get; set; }

        [DataMember]
        [DisplayName("Amount")]
        public string Amount { get; set; }

        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }

        [DataMember]
        [DisplayName("Type")]
        public int Type { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }

        [DataMember]
        [DisplayName("CustomerCode")]
        public string CustomerCode { get; set; }

        [NotMapped]
        [DisplayName("FullName")]
        public string FullName { get; set; }

        [DataMember]
        [DisplayName("TotalDay")]
        public int TotalDay { get; set; }

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
                    return string.Empty;
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
                    return string.Empty;
                }
                return EndDate.Value.ToString(Extensions.Constants.DateTimeFomatFull);
            }
        }

        [DataMember]
        [DisplayName("TransactionCode")]
        public string TransactionCode { get; set; }

        [DataMember]
        [DisplayName("IsLock")]
        public bool IsLock { get; set; }

        [DataMember]
        [DisplayName("IsClosed")]
        public bool IsClosed { get; set; }
    }

    public class TransactionSmsMap : EntityTypeConfiguration<TransactionSmsInfo>, IEntityTypeConfiguration
    {
        public TransactionSmsMap()
        {
            ToTable("Modules_TransactionSms");
            HasKey(m => m.Id);
            Property(m => m.MoId).HasMaxLength(50);
            Property(m => m.UserId).HasMaxLength(250);
            Property(m => m.ShortCode).HasMaxLength(50);
            Property(m => m.Keyword).HasMaxLength(250);
            Property(m => m.Subkeyword).HasMaxLength(250);
            Property(m => m.TransDate).HasMaxLength(100);
            Property(m => m.Amount).HasMaxLength(250);
            Property(m => m.CustomerCode).HasMaxLength(250);
            Property(m => m.TransactionCode).HasMaxLength(50);
        }  
    }
}