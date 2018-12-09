using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class FilmTypesInfo : BaseEntity<int>
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
        [DisplayName("Description")]
        public string Description { get; set; }
        
        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }

        [DataMember]
        [DisplayName("IsFilm")]
        public bool IsFilm { get; set; }

        [DataMember]
        [DisplayName("IsShow")]
        public bool IsShow { get; set; }

        [DataMember]
        [DisplayName("IsClip")]
        public bool IsClip { get; set; }

        [DataMember]
        [DisplayName("IsJJFilm")]
        public bool IsJJFilm { get; set; }
    }

    public class FilmTypeMap : EntityTypeConfiguration<FilmTypesInfo>, IEntityTypeConfiguration
    {

        public FilmTypeMap()
        {
           ToTable("Modules_FilmTypes");
           HasKey(m => m.Id);
           Property(m => m.LanguageCode).IsRequired().HasMaxLength(50);
           Property(m => m.SiteId).IsRequired();
           Property(m => m.Name).IsRequired().HasMaxLength(250);
           Property(m => m.Description).HasMaxLength(200);
           Property(m => m.Status);
        }
    }
}
