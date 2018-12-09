using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class DownloadGameInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("Code")]
        public string Code { get; set; }

        [DataMember]
        [DisplayName("UrlBanner")]
        public string UrlBanner { get; set; }

        [DataMember]
        [DisplayName("Logo")]
        public string Logo { get; set; }

        [DataMember]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DataMember]
        [DisplayName("GooglePlayUrl")]
        public string GooglePlayUrl { get; set; }

        [DataMember]
        [DisplayName("WebsiteUrl")]
        public string WebsiteUrl { get; set; }

        [DataMember]
        [DisplayName("VipXu")]
        public int VipXu { get; set; }
        
        [DataMember]
        [DisplayName("IsActived")]
        public bool IsActived { get; set; }
    }

    public class DownloadGameMap : EntityTypeConfiguration<DownloadGameInfo>, IEntityTypeConfiguration
    {
        public DownloadGameMap()
        {
            ToTable("Modules_DownloadGames");
            HasKey(m => m.Id);
            Property(m => m.Code).IsRequired().HasMaxLength(100);
            Property(m => m.UrlBanner).IsRequired().HasMaxLength(500);
            Property(m => m.Logo).IsRequired().HasMaxLength(500);
            Property(m => m.Title).IsRequired().HasMaxLength(250);
            Property(m => m.GooglePlayUrl).IsRequired().HasMaxLength(500);
            Property(m => m.WebsiteUrl).HasMaxLength(500);
            Property(m => m.VipXu).IsRequired();
        }
    }
}