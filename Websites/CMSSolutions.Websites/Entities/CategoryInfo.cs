using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class CategoryInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }

        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("ParentId")]
        public int ParentId { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string ParentName { get; set; }

        [DataMember]
        [DisplayName("ShortName")]
        public string ShortName { get; set; }

        [DataMember]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DataMember]
        [DisplayName("Alias")]
        public string Alias { get; set; }

        [DataMember]
        [DisplayName("IsHome")]
        public bool IsHome { get; set; }

        [DataMember]
        [DisplayName("HasChilden")]
        public bool HasChilden { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string ChildenName { get; set; }

        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DataMember]
        [DisplayName("Tags")]
        public string Tags { get; set; }
        
        [DataMember]
        [DisplayName("Url")]
        public string Url { get; set; }

        [DataMember]
        [DisplayName("IsDisplay")]
        public bool IsActived { get; set; }

        [DataMember]
        [DisplayName("OrderBy")]
        public int OrderBy { get; set; }

        [DataMember]
        [DisplayName("IsDeleted")]
        public bool IsDeleted { get; set; }
    }

    public class CategoryMap : EntityTypeConfiguration<CategoryInfo>, IEntityTypeConfiguration
    {
        public CategoryMap()
        {
            ToTable("Modules_Categories");
            HasKey(x => x.Id);
            Property(x => x.SiteId).IsRequired();
            Property(x => x.ParentId);
            Property(x => x.LanguageCode).HasMaxLength(50);
            Property(x => x.ShortName).IsRequired().HasMaxLength(250);
            Property(x => x.Name).IsRequired().HasMaxLength(400);
            Property(x => x.Alias).IsRequired().HasMaxLength(400);
            Property(x => x.CreateDate).IsRequired();
            Property(x => x.Description).HasMaxLength(2000).IsRequired();
            Property(x => x.Tags).HasMaxLength(2000).IsRequired();
            Property(x => x.Url).IsRequired();
            Property(x => x.IsActived).IsRequired();
            Property(x => x.OrderBy).IsRequired();
            Property(x => x.IsDeleted).IsRequired();
            Property(x => x.IsHome).IsRequired();
            Property(x => x.HasChilden).IsRequired();
        }
    }
}