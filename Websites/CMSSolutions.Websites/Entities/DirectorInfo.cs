using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    /// <summary>
    /// Đạo diễn
    /// </summary>
    [DataContract]
    public class DirectorInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("FullName")]
        public string FullName { get; set; }
        
        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }
        
        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }
    }
    
    public class DirectorMap : EntityTypeConfiguration<DirectorInfo>, IEntityTypeConfiguration
    {
        
        public DirectorMap()
        {
            ToTable("Modules_Directors");
            HasKey(m => m.Id);
            Property(m => m.FullName).IsRequired().HasMaxLength(250);
            Property(m => m.Description).HasMaxLength(2000);
        }
    }
}
