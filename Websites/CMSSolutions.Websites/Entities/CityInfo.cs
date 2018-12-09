using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class CityInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("Code")]
        public string Code { get; set; }

        [DataMember]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DataMember]
        [DisplayName("CountryId")]
        public int CountryId { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }
    }

    public class CityMap : EntityTypeConfiguration<CityInfo>, IEntityTypeConfiguration
    {
        public CityMap()
        {
            ToTable("Modules_City");
            HasKey(x => x.Id);
            Property(x => x.Code).HasMaxLength(50);
            Property(x => x.Name).IsRequired().HasMaxLength(250);
            Property(x => x.CountryId).IsRequired();
        }
    }
}