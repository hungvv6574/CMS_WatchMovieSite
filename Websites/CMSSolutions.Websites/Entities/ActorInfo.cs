using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    /// <summary>
    /// Diễn viên
    /// </summary>
    [DataContract]
    public class ActorInfo : BaseEntity<int>
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
    
    public class ActorMap : EntityTypeConfiguration<ActorInfo>, IEntityTypeConfiguration
    {
        public ActorMap()
        {
            ToTable("Modules_Actors");
            HasKey(m => m.Id);
            Property(m => m.FullName).IsRequired().HasMaxLength(250);
            Property(m => m.Description).HasMaxLength(2000);
            Property(m => m.Status).IsRequired();
        }
    }
}
