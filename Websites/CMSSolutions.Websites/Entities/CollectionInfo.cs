using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    /// <summary>
    /// Bộ phim
    /// </summary>
    [DataContract]
    public class CollectionInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }
        
        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }
        
        [DataMember]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DataMember]
        [DisplayName("IsHost")]
        public bool IsHot { get; set; }

        [DataMember]
        [DisplayName("OrderBy")]
        public int OrderBy { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }
        
        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }
    }

    public class CollectionMap : EntityTypeConfiguration<CollectionInfo>, IEntityTypeConfiguration
    {

        public CollectionMap()
        {
            ToTable("Modules_Collections");
            HasKey(m => m.Id);
            Property(m => m.LanguageCode).IsRequired().HasMaxLength(50);
            Property(m => m.SiteId).IsRequired();
            Property(m => m.Name).IsRequired().HasMaxLength(250);
            Property(m => m.Description).HasMaxLength(2000);
        }
    }
}
