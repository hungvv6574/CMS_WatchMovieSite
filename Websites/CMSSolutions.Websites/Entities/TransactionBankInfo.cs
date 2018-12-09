using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class TransactionBankInfo : BaseEntity<long>
    {
        [DataMember]
        [DisplayName("CustomerCode")]
        public string CustomerCode { get; set; }

        [DataMember]
        [DisplayName("TransactionCode")]
        public string TransactionCode { get; set; }

        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextStartDate
        {
            get
            {
                return CreateDate.ToString(Extensions.Constants.DateTimeFomatFull);
            }
        }

        [DataMember]
        [DisplayName("BankCode")]
        public string BankCode { get; set; }

        [NotMapped]
        [DisplayName("BankName")]
        public string BankName { get; set; }

        [DataMember]
        [DisplayName("Amount")]
        public string Amount { get; set; }

        [DataMember]
        [DisplayName("ResponCode")]
        public string ResponCode { get; set; }

        [DataMember]
        [DisplayName("Mac")]
        public string Mac { get; set; }

        [DataMember]
        [DisplayName("TransId")]
        public string TransId { get; set; }

        [DataMember]
        [DisplayName("Type")]
        public int Type { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }

        [DataMember]
        [DisplayName("IsLock")]
        public bool IsLock { get; set; }

        [NotMapped]
        [DisplayName("FullName")]
        public string FullName { get; set; }
    }

    public class TransactionBankMap : EntityTypeConfiguration<TransactionBankInfo>, IEntityTypeConfiguration
    {
        public TransactionBankMap()
        {
            ToTable("Modules_TransactionBank");
            HasKey(m => m.Id);
            Property(m => m.CustomerCode).HasMaxLength(50);
            Property(m => m.TransactionCode).HasMaxLength(250);
            Property(m => m.BankCode).HasMaxLength(50);
            Property(m => m.Amount).HasMaxLength(250);
            Property(m => m.ResponCode).HasMaxLength(100);
            Property(m => m.Mac).HasMaxLength(250);
            Property(m => m.TransId).HasMaxLength(250);
            Property(m => m.Description).HasMaxLength(2000);
        }
    }
}