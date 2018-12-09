using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class BankCardInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("BankCode")]
        public string BankCode { get; set; }
        
        [DataMember]
        [DisplayName("BankName")]
        public string BankName { get; set; }
        
        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }
    }
    
    public class BankCardMap : EntityTypeConfiguration<BankCardInfo>, IEntityTypeConfiguration
    {

        public BankCardMap()
        {
            ToTable("Modules_BankCards");
            HasKey(m => m.Id);
            Property(m => m.BankCode).IsRequired().HasMaxLength(50);
            Property(m => m.BankName).IsRequired().HasMaxLength(250);
        }
    }
}
