using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{   
    [DataContract]
    public class VIPCardInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }

        [DataMember]
        [DisplayName("ServerId")]
        public int ServerId { get; set; }

        [NotMapped]
        [DisplayName("ServerName")]
        public string ServerName { get; set; }

        [DataMember]
        [DisplayName("VIPCode")]
        public string VIPCode { get; set; }

        [DataMember]
        [DisplayName("VIPName")]
        public string VIPName { get; set; }

        [DataMember]
        [DisplayName("VIPValue")]
        public decimal VIPValue { get; set; }
    }

    public class VIPCardMap : EntityTypeConfiguration<VIPCardInfo>, IEntityTypeConfiguration
    {
        public VIPCardMap()
        {
            ToTable("Modules_VIPCards");
            HasKey(m => m.Id);
            Property(m => m.LanguageCode).HasMaxLength(50).IsRequired();
            Property(m => m.SiteId).IsRequired();
            Property(m => m.VIPCode).HasMaxLength(50).IsRequired();
            Property(m => m.VIPName).HasMaxLength(250);
            Property(m => m.VIPValue).IsRequired();
            Property(m => m.ServerId).IsRequired();
        }
    }
}
