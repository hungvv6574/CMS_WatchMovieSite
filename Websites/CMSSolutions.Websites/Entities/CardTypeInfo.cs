using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class CardTypeInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("Code")]
        public string Code { get; set; }
        
        [DataMember]
        [DisplayName("Name")]
        public string Name { get; set; }
        
        [DataMember]
        [DisplayName("HasSerial")]
        public bool HasSerial { get; set; }
        
        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }
    }

    public class CardTypeMap : EntityTypeConfiguration<CardTypeInfo>, IEntityTypeConfiguration
    {

        public CardTypeMap()
        {
            ToTable("Modules_CardTypes");
            HasKey(m => m.Id);
            Property(m => m.Code).HasMaxLength(50);
            Property(m => m.Name).HasMaxLength(250);
        }
    }
}
