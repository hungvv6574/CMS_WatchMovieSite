using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class FilmVideoInfo : BaseEntity<long>
    {
        [DataMember]
        [DisplayName("FilmId")]
        public long FilmId { get; set; }

        [DataMember]
        [DisplayName("EpisodeId")]
        public int EpisodeId { get; set; }

        [NotMapped]
        [DisplayName("EpisodeName")]
        public string EpisodeName { get; set; }

        [NotMapped]
        [DisplayName("EpisodeCount")]
        public int EpisodeCount { get; set; }
        
        [DataMember]
        [DisplayName("FileId")]
        public long FileId { get; set; }

        [NotMapped]
        [DisplayName("FileName")]
        public string FileName { get; set; }
        
        [DataMember]
        [DisplayName("FullPath")]
        public string FullPath { get; set; }

        [DataMember]
        [DisplayName("ImageIcon")]
        public string ImageIcon { get; set; }

        [DataMember]
        [DisplayName("IsTraller")]
        public bool IsTraller { get; set; }

        [DataMember]
        [DisplayName("IsActived")]
        public bool IsActived { get; set; }

        [DataMember]
        [DisplayName("UrlSource")]
        public string UrlSource { get; set; }

        [DataMember]
        [DisplayName("UrlAlbum")]
        public string UrlAlbum { get; set; }
        
        [DataMember]
        [DisplayName("Subtitle")]
        public string Subtitle { get; set; }
        
        [DataMember]
        [DisplayName("StreamingUrl")]
        public string StreamingUrl { get; set; }

        [DataMember]
        [DisplayName("BaseUrl")]
        public string BaseUrl { get; set; }
    }

    public class FilmVideoMap : EntityTypeConfiguration<FilmVideoInfo>, IEntityTypeConfiguration
    {
        public FilmVideoMap()
        {
            ToTable("Modules_FilmVideos");
            HasKey(m => m.Id);
            Property(m => m.FilmId).IsRequired();
            Property(m => m.FileId).IsRequired();
            Property(m => m.FullPath).HasMaxLength(300);
            Property(m => m.EpisodeId).IsRequired();
            Property(m => m.ImageIcon).HasMaxLength(500);
            Property(m => m.UrlSource).HasMaxLength(500);
            Property(m => m.Subtitle).HasMaxLength(500);
            Property(m => m.BaseUrl).HasMaxLength(500);
            Property(m => m.StreamingUrl).HasMaxLength(500);
            Property(m => m.UrlAlbum).HasMaxLength(500);
        }
    }
}
