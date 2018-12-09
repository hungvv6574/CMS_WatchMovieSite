using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class SupportInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }

        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DataMember]
        [DisplayName("Messages")]
        public string Messages { get; set; }

        [DataMember]
        [DisplayName("IsGroup")]
        public bool IsGroup { get; set; }

        [DataMember]
        [DisplayName("OrderBy")]
        public int OrderBy { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }

        [DataMember]
        [DisplayName("ParentId")]
        public int ParentId { get; set; }
    }

    public class SupportMap : EntityTypeConfiguration<SupportInfo>, IEntityTypeConfiguration
    {
        public SupportMap()
        {
            ToTable("Modules_Supports");
            HasKey(x => x.Id);
            Property(x => x.LanguageCode).HasMaxLength(50).IsRequired();
            Property(x => x.Title).IsRequired().HasMaxLength(250);
            Property(x => x.Messages).HasMaxLength(2000);
        }
    }
}