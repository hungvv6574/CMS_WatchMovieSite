using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    /// <summary>
    /// Tập phim
    /// </summary>
    [DataContract]
    public class EpisodeInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }
        
        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }
        
        [DataMember]
        [DisplayName("EpisodeName")]
        public string EpisodeName { get; set; }
        
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
    
    public class EpisodeMap : EntityTypeConfiguration<EpisodeInfo>, IEntityTypeConfiguration
    {
        
        public EpisodeMap()
        {
            ToTable("Modules_Episodes");
            HasKey(m => m.Id);
            Property(m => m.LanguageCode).IsRequired().HasMaxLength(50);
            Property(m => m.SiteId).IsRequired();
            Property(m => m.EpisodeName).HasMaxLength(50);
            Property(m => m.Description).HasMaxLength(2000);
        }
    }
}
