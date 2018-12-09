using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class PromotionCustomerInfo : BaseEntity<long>
    {
        [DataMember]
        [DisplayName("PromotionId")]
        public int PromotionId { get; set; }

        [NotMapped]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DataMember]
        [DisplayName("CustomerId")]
        public int CustomerId { get; set; }

        [DataMember]
        [DisplayName("CustomerCode")]
        public string CustomerCode { get; set; }

        [NotMapped]
        [DisplayName("FullName")]
        public string FullName { get; set; }

        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }

        [DataMember]
        [DisplayName("Code")]
        public string Code { get; set; }

        [DataMember]
        [DisplayName("Value")]
        public string Value { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DataMember]
        [DisplayName("IsUsed")]
        public bool IsUsed { get; set; }

        [DataMember]
        [DisplayName("UsedDate")]
        public DateTime? UsedDate { get; set; }

        [DataMember]
        [DisplayName("IsSendMessages")]
        public bool IsSendMessages { get; set; }

        [DataMember]
        [DisplayName("SendDate")]
        public DateTime? SendDate { get; set; }
    }

    public class PromotionCustomerMap : EntityTypeConfiguration<PromotionCustomerInfo>, IEntityTypeConfiguration
    {
        public PromotionCustomerMap()
        {
            ToTable("Modules_PromotionCustomers");
            HasKey(m => m.Id);
            Property(m => m.CustomerCode).IsRequired().HasMaxLength(50);
            Property(m => m.Code).IsRequired().HasMaxLength(50);
            Property(m => m.Value).IsRequired().HasMaxLength(50);
            Property(m => m.PromotionId).IsRequired();
            Property(m => m.CustomerId).IsRequired();
        }
    }
}