using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    public enum SearchField
    {
        Id, SiteId, CategoryIds, CategoryNames, Type, SearchId, Title, TitleEnglish, Alias, Sumary, Tags, IsShow, IsClip, IsTrailer, IsFilm, Keyword, KeywordEN
    }

    [DataContract]
    public class SearchInfo : BaseEntity<long>
    {
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
        [DisplayName("CategoryNames")]
        public string CategoryNames { get; set; }
        
        [DataMember]
        [DisplayName("Type")]
        public int Type { get; set; }
        
        [DataMember]
        [DisplayName("SearchId")]
        public string SearchId { get; set; }
        
        [DataMember]
        [DisplayName("Title")]
        public string Title { get; set; }
        
        [DataMember]
        [DisplayName("TitleEnglish")]
        public string TitleEnglish { get; set; }

        [DataMember]
        [DisplayName("Images")]
        public string Images { get; set; }
        
        [DataMember]
        [DisplayName("Alias")]
        public string Alias { get; set; }
        
        [DataMember]
        [DisplayName("Sumary")]
        public string Sumary { get; set; }
        
        [DataMember]
        [DisplayName("Tags")]
        public string Tags { get; set; }
        
        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }
        
        [DataMember]
        [DisplayName("IsBlock")]
        public bool IsBlock { get; set; }

        [DataMember]
        [DisplayName("Processed")]
        public int Processed { get; set; }

        [NotMapped]
        [DisplayName("IsShow")]
        public bool IsShow { get; set; }

        [NotMapped]
        [DisplayName("IsClip")]
        public bool IsClip { get; set; }

        [NotMapped]
        [DisplayName("IsTrailer")]
        public bool IsTrailer { get; set; }

        [NotMapped]
        [DisplayName("IsFilm")]
        public bool IsFilm { get; set; }

        [NotMapped]
        [DisplayName("ViewCount")]
        public string ViewCount { get; set; }
    }
    
    public class SearchMap : EntityTypeConfiguration<SearchInfo>, IEntityTypeConfiguration
    {
        public SearchMap()
        {
            ToTable("Modules_Search");
            HasKey(m => m.Id);
            Property(m => m.LanguageCode).HasMaxLength(50);
            Property(m => m.SiteId);
            Property(m => m.CategoryIds).HasMaxLength(250).IsRequired();
            Property(m => m.Type);
            Property(m => m.SearchId).HasMaxLength(50);
            Property(m => m.Title).HasMaxLength(250);
            Property(m => m.TitleEnglish).IsRequired().HasMaxLength(250);
            Property(m => m.Images).HasMaxLength(500);
            Property(m => m.Alias).HasMaxLength(250);
            Property(m => m.Sumary).HasMaxLength(500);
            Property(m => m.Tags).HasMaxLength(400);
            Property(m => m.CreateDate);
            Property(m => m.IsBlock);
        }
    }
}
