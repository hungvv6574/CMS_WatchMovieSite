using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class SliderInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }

        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("CategoryId")]
        public int CategoryId { get; set; }

        [NotMapped]
        [DisplayName("CategoryName")]
        public string CategoryName { get; set; }

        [DataMember]
        [DisplayName("PageId")]
        public int PageId { get; set; }

        [DataMember]
        [DisplayName("FilmId")]
        public long FilmId { get; set; }

        [NotMapped]
        [DisplayName("FilmName")]
        public string FilmName { get; set; }

        [DataMember]
        [DisplayName("Images")]
        public string Images { get; set; }

        [DataMember]
        [DisplayName("OrderBy")]
        public int OrderBy { get; set; }
    }

    public class SliderMap : EntityTypeConfiguration<SliderInfo>, IEntityTypeConfiguration
    {
        public SliderMap()
        {
            ToTable("Modules_Slider");
            HasKey(x => x.Id);
            Property(x => x.LanguageCode).HasMaxLength(50).IsRequired();
            Property(x => x.SiteId).IsRequired();
            Property(x => x.CategoryId).IsRequired();
            Property(x => x.PageId).IsRequired();
            Property(x => x.FilmId).IsRequired();
            Property(x => x.Images).IsRequired().HasMaxLength(500);
            Property(x => x.OrderBy).IsRequired();
        }
    }
}