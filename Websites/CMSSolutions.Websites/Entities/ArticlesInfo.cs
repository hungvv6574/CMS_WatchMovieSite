using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class ArticlesInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }

        [DataMember]
        [DisplayName("ParentId")]
        public int ParentId { get; set; }

        [DataMember]
        [DisplayName("CategoryId")]
        public int CategoryId { get; set; }

        [NotMapped]
        [DisplayName("CategoryAlias")]
        public string CategoryAlias { get; set; }

        [NotMapped]
        [DisplayName("CategoryName")]
        public string CategoryName { get; set; }

        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("Icon")]
        public string Icon { get; set; }

        [DataMember]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DataMember]
        [DisplayName("Alias")]
        public string Alias { get; set; }

        [DataMember]
        [DisplayName("Summary")]
        public string Summary { get; set; }

        [DataMember]
        [DisplayName("Contents")]
        public string Contents { get; set; }

        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }

        [DataMember]
        [DisplayName("CreateByUser")]
        public int CreateByUser { get; set; }

        [DataMember]
        [DisplayName("IsPublished")]
        public bool IsPublished { get; set; }

        [DataMember]
        [DisplayName("PublishedDate")]
        public DateTime? PublishedDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextPublishedDate
        {
            get
            {
                if (PublishedDate != null)
                {
                    return PublishedDate.Value.ToString(Extensions.Constants.DateTimeFomat2);
                }
                return CreateDate.ToString(Extensions.Constants.DateTimeFomat2);
            }
        }

        [DataMember]
        [DisplayName("IsHome")]
        public bool IsHome { get; set; }

        [DataMember]
        [DisplayName("IsVideo")]
        public bool IsVideo { get; set; }

        [DataMember]
        [DisplayName("VideosUrl")]
        public string VideosUrl { get; set; }

        [DataMember]
        [DisplayName("ViewCount")]
        public int ViewCount { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DataMember]
        [DisplayName("Tags")]
        public string Tags { get; set; }

        [DataMember]
        [DisplayName("IsDeleted")]
        public bool IsDeleted { get; set; }
    }

    public class ArticlesMap : EntityTypeConfiguration<ArticlesInfo>, IEntityTypeConfiguration
    {
        public ArticlesMap()
        {
            ToTable("Modules_Articles");
            HasKey(x => x.Id);
            Property(x => x.SiteId).IsRequired();
            Property(x => x.CategoryId).IsRequired();
            Property(x => x.Icon).HasMaxLength(300);
            Property(x => x.LanguageCode).HasMaxLength(50);
            Property(x => x.Title).IsRequired().HasMaxLength(200);
            Property(x => x.Alias).IsRequired().HasMaxLength(200);
            Property(x => x.Summary).IsRequired().HasMaxLength(400);
            Property(x => x.Contents).IsRequired();
            Property(x => x.CreateDate).IsRequired();
            Property(x => x.CreateByUser).IsRequired();
            Property(x => x.IsPublished).IsRequired();
            Property(x => x.IsHome).IsRequired();
            Property(x => x.IsVideo).IsRequired();
            Property(x => x.ViewCount).IsRequired();
            Property(x => x.Description).HasMaxLength(400);
            Property(x => x.Tags).HasMaxLength(500);
            Property(x => x.IsDeleted).IsRequired();
        }
    }
}