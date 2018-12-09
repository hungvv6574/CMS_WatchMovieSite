using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    /// <summary>
    /// Nước sản xuất
    /// </summary>
    [DataContract]
    public class CountryInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("Code")]
        public string Code { get; set; }
        
        [DataMember]
        [DisplayName("Name")]
        public string Name { get; set; }
    }

    public class CountryMap : EntityTypeConfiguration<CountryInfo>, IEntityTypeConfiguration
    {
        public CountryMap()
        {
            ToTable("Modules_Countries");
            HasKey(m => m.Id);
            Property(m => m.Code).IsRequired().HasMaxLength(50);
            Property(m => m.Name).IsRequired().HasMaxLength(250);
        }
    }
}
