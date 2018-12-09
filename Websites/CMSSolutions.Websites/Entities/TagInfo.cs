using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class TagInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DataMember]
        [DisplayName("Alias")]
        public string Alias { get; set; }

        [DataMember]
        [DisplayName("IsDisplay")]
        public bool IsDisplay { get; set; }
    }

    public class TagMap : EntityTypeConfiguration<TagInfo>, IEntityTypeConfiguration
    {
        public TagMap()
        {
            ToTable("Modules_Tags");
            HasKey(x => x.Id);
            Property(x => x.Alias).HasMaxLength(250).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(250);
            Property(x => x.IsDisplay).IsRequired();
        }
    }
}