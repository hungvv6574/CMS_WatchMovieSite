using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class FilmInfo : BaseEntity<long>
    {
        [DataMember]
        [DisplayName("FilmCode")]
        public string FilmCode { get; set; }

        [DataMember]
        [DisplayName("FilmNameEnglish")]
        public string FilmNameEnglish { get; set; }

        [DataMember]
        [DisplayName("FilmName")]
        public string FilmName { get; set; }
        
        [DataMember]
        [DisplayName("FilmAlias")]
        public string FilmAlias { get; set; }
        
        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }
        
        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }
        
        [DataMember]
        [DisplayName("CategoryIds")]
        public string CategoryIds { get; set; }

        [NotMapped]
        [DisplayName("CategoryAlias")]
        public string CategoryAlias { get; set; }

        [NotMapped]
        [DisplayName("CategoryNames")]
        public string CategoryNames { get; set; }
        
        [DataMember]
        [DisplayName("FilmTypeIds")]
        public string FilmTypeIds { get; set; }

        [NotMapped]
        [DisplayName("FilmTypeNames")]
        public string FilmTypeNames { get; set; }

        [NotMapped]
        [DisplayName("EpisodeCount")]
        public int EpisodeCount { get; set; }
        
        [DataMember]
        [DisplayName("CountryId")]
        public int CountryId { get; set; }

        [NotMapped]
        [DisplayName("CountryName")]
        public string CountryName { get; set; }

        [DataMember]
        [DisplayName("DirectorId")]
        public int DirectorId { get; set; }

        [NotMapped]
        [DisplayName("DirectorName")]
        public string DirectorName { get; set; }

        [DataMember]
        [DisplayName("ActorIds")]
        public string ActorIds { get; set; }

        [NotMapped]
        [DisplayName("ActorNames")]
        public string ActorNames { get; set; }

        [DataMember]
        [DisplayName("CollectionId")]
        public int CollectionId { get; set; }

        [NotMapped]
        [DisplayName("CollectionName")]
        public string CollectionName { get; set; }

        [DataMember]
        [DisplayName("IsFilmRetail")]
        public bool IsFilmRetail { get; set; }

        [DataMember]
        [DisplayName("IsFilmLengthEpisodes")]
        public bool IsFilmLengthEpisodes { get; set; }

        [DataMember]
        [DisplayName("ArticlesId")]
        public int ArticlesId { get; set; }

        [DataMember]
        [DisplayName("Time")]
        public string Time { get; set; }

        [DataMember]
        [DisplayName("Capacity")]
        public string Capacity { get; set; }

        [DataMember]
        [DisplayName("ReleaseYear")]
        public int ReleaseYear { get; set; }

        [DataMember]
        [DisplayName("CommentCount")]
        public int CommentCount { get; set; }

        [DataMember]
        [DisplayName("ViewCount")]
        public int ViewCount { get; set; }

        [DataMember]
        [DisplayName("CreateByUserId")]
        public int CreateByUserId { get; set; }

        [NotMapped]
        [DisplayName("FullName")]
        public string FullName { get; set; }

        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string DisplayDate { get { return Utilities.DateString(CreateDate); } }

        [DataMember]
        [DisplayName("Contents")]
        public string Contents { get; set; }

        [DataMember]
        [DisplayName("Summary")]
        public string Summary { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DataMember]
        [DisplayName("Tags")]
        public string Tags { get; set; }

        [DataMember]
        [DisplayName("IsPublished")]
        public bool IsPublished { get; set; }

        [DataMember]
        [DisplayName("PublishedDate")]
        public DateTime? PublishedDate { get; set; }

        [DataMember]
        [DisplayName("IsHot")]
        public bool IsHot { get; set; }
        
        [DataMember]
        [DisplayName("IsHome")]
        public bool IsHome { get; set; }
        
        [DataMember]
        [DisplayName("Prices")]
        public decimal Prices { get; set; }
        
        [DataMember]
        [DisplayName("HasCopyright")]
        public bool HasCopyright { get; set; }
        
        [DataMember]
        [DisplayName("StartDate")]
        public DateTime? StartDate { get; set; }
        
        [DataMember]
        [DisplayName("EndDate")]
        public DateTime? EndDate { get; set; }
        
        [DataMember]
        [DisplayName("ImageIcon")]
        public string ImageIcon { get; set; }
        
        [DataMember]
        [DisplayName("ImageThumb")]
        public string ImageThumb { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string EncodeImageThumb
        {
            get
            {
                if (!string.IsNullOrEmpty(ImageThumb))
                {
                    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ImageThumb));
                }

                return string.Empty;
            }
        }
        
        [DataMember]
        [DisplayName("ServerId")]
        public int ServerId { get; set; }
        
        [DataMember]
        [DisplayName("ServerIP")]
        public string ServerIp { get; set; }

        [NotMapped]
        [DisplayName("ServerName")]
        public string ServerName { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }

        [DataMember]
        [DisplayName("IsTheater")]
        public bool IsTheater { get; set; }

        [DataMember]
        [DisplayName("IsShow")]
        public bool IsShow { get; set; }

        [DataMember]
        [DisplayName("IsClip")]
        public bool IsClip { get; set; }

        [DataMember]
        [DisplayName("IsTrailer")]
        public bool IsTrailer { get; set; }

        [DataMember]
        [DisplayName("IsFilm")]
        public bool IsFilm { get; set; }

        #region Videos
        [NotMapped]
        [DisplayName("FullPath")]
        public string FullPath { get; set; }

        [NotMapped]
        [DisplayName("EpisodeId")]
        public int EpisodeId { get; set; }

        [NotMapped]
        [DisplayName("ImageIcon2")]
        public string ImageIcon2 { get; set; }

        [NotMapped]
        [DisplayName("HasTraller")]
        public bool HasTraller { get; set; }

        [NotMapped]
        [DisplayName("UrlSource")]
        public string UrlSource { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string EncodeSourceUrl
        {
            get
            {
                if (string.IsNullOrEmpty(UrlAlbum) && !string.IsNullOrEmpty(UrlSource))
                {
                    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(UrlSource));
                    
                }

                return string.Empty;
            }
        }

        [NotMapped]
        [DisplayName("Subtitle")]
        public string Subtitle { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string EncodeSubtitle
        {
            get
            {
                if (!string.IsNullOrEmpty(Subtitle))
                {
                    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Subtitle));
                }

                return string.Empty;
            }
        }

        [NotMapped]
        [DisplayName("StreamingUrl")]
        public string StreamingUrl { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string EncodeStreamingUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(StreamingUrl))
                {
                    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(StreamingUrl));
                }

                return string.Empty;
            }
        }

        [NotMapped]
        [DisplayName("BaseUrl")]
        public string BaseUrl { get; set; }

        [NotMapped]
        [DisplayName("UrlAlbum")]
        public string UrlAlbum { get; set; }

        [NotMapped]
        [DisplayName("EpisodeName")]
        public string EpisodeName { get; set; }

        [NotMapped]
        [DisplayName("EpisodeIndex")]
        public int EpisodeIndex { get; set; }
        #endregion
    }

    public class FilmMap : EntityTypeConfiguration<FilmInfo>, IEntityTypeConfiguration
    {
        public FilmMap()
        {
            ToTable("Modules_Film");
            HasKey(m => m.Id);
            Property(m => m.FilmCode).IsRequired().HasMaxLength(50);
            Property(m => m.FilmNameEnglish).HasMaxLength(250);
            Property(m => m.FilmName).IsRequired().HasMaxLength(250);
            Property(m => m.FilmAlias).IsRequired().HasMaxLength(250);
            Property(m => m.LanguageCode).IsRequired().HasMaxLength(50);
            Property(m => m.SiteId).IsRequired();
            Property(m => m.CategoryIds).HasMaxLength(250).IsRequired();
            Property(m => m.FilmTypeIds).HasMaxLength(250).IsRequired();
            Property(m => m.CountryId).IsRequired();
            Property(m => m.DirectorId).IsRequired();
            Property(m => m.ActorIds).IsRequired().HasMaxLength(250);
            Property(m => m.Time).IsRequired().HasMaxLength(50);
            Property(m => m.Capacity).IsRequired().HasMaxLength(50);
            Property(m => m.Contents).IsRequired();
            Property(m => m.Summary).IsRequired().HasMaxLength(400);
            Property(m => m.Description).IsRequired().HasMaxLength(400);
            Property(m => m.Tags).IsRequired().HasMaxLength(250);
            Property(m => m.ImageIcon).IsRequired().HasMaxLength(250);
            Property(m => m.ImageThumb).HasMaxLength(250);
            Property(m => m.ServerIp).IsRequired().HasMaxLength(50);
        }
    }
}
