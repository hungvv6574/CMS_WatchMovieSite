using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class TransactionCardInfo : BaseEntity<long>
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
        [DisplayName("CardCode")]
        public string CardCode { get; set; }

        [NotMapped]
        [DisplayName("CardName")]
        public string CardName { get; set; }

        [DataMember]
        [DisplayName("SeriNumber")]
        public string SeriNumber { get; set; }

        [DataMember]
        [DisplayName("PinCode")]
        public string PinCode { get; set; }

        [DataMember]
        [DisplayName("Amount")]
        public string Amount { get; set; }

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

    public class TransactionCardMap : EntityTypeConfiguration<TransactionCardInfo>, IEntityTypeConfiguration
    {
        public TransactionCardMap()
        {
            ToTable("Modules_TransactionCard");
            HasKey(m => m.Id);
            Property(m => m.CustomerCode).IsRequired().HasMaxLength(50);
            Property(m => m.TransactionCode).IsRequired().HasMaxLength(50);
            Property(m => m.SeriNumber).HasMaxLength(250);
            Property(m => m.PinCode).IsRequired().HasMaxLength(250);
            Property(m => m.Description).HasMaxLength(2000);
            Property(m => m.Amount).HasMaxLength(250);
        }
    }
}