using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    public class RateInfo : BaseEntity<long>
    {
        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }

        [DataMember]
        [DisplayName("FilmId")]
        public long FilmId { get; set; }

        [NotMapped]
        [DisplayName("FilmName")]
        public string FilmName { get; set; }

        [DataMember]
        [DisplayName("Rate")]
        public int Rate { get; set; }

        [DataMember]
        [DisplayName("CustomerId")]
        public int CustomerId { get; set; }

        [DataMember]
        [DisplayName("CustomerCode")]
        public string CustomerCode { get; set; }

        [NotMapped]
        [DisplayName("CustomerName")]
        public string CustomerName { get; set; }

        [DataMember]
        [DisplayName("AlertError")]
        public string AlertError { get; set; }

        [DataMember]
        [DisplayName("AlertLag")]
        public string AlertLag { get; set; }

        [DataMember]
        [DisplayName("Messages")]
        public string Messages { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }
        
        [DataMember]
        [DisplayName("LikeFilm")]
        public bool LikeFilm { get; set; }
    }

    public class RateMap : EntityTypeConfiguration<RateInfo>, IEntityTypeConfiguration
    {

        public RateMap()
        {
            ToTable("Modules_Rates");
            HasKey(m => m.Id);
            Property(m => m.LanguageCode).HasMaxLength(50);
            Property(m => m.CustomerCode).HasMaxLength(250);
            Property(m => m.AlertError).HasMaxLength(250);
            Property(m => m.AlertLag).HasMaxLength(250);
            Property(m => m.Messages).HasMaxLength(2000);
        }
    }
}
