using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class SiteInfo : BaseEntity<int>
    {
         [DataMember]
         [DisplayName("Name")]
         public string Name { get; set; }

         [DataMember]
         [DisplayName("LanguageCode")]
         public string LanguageCode { get; set; }

         [DataMember]
         [DisplayName("Url")]
         public string Url { get; set; }

         [DataMember]
         [DisplayName("Domain")]
         public string Domain { get; set; }

         [DataMember]
         [DisplayName("IsActived")]
         public bool IsActived { get; set; }

         [DataMember]
         [DisplayName("Description")]
         public string Description { get; set; }
    }

    public class SiteMap : EntityTypeConfiguration<SiteInfo>, IEntityTypeConfiguration
    {
        public SiteMap()
        {
            ToTable("Modules_Sites");
            HasKey(x => x.Id);
            Property(x => x.LanguageCode).HasMaxLength(50).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(250);
            Property(x => x.Url).HasMaxLength(250);
            Property(x => x.Domain).HasMaxLength(50);
            Property(x => x.IsActived).IsRequired();
            Property(x => x.Description).HasMaxLength(2000);
        }
    }
}
