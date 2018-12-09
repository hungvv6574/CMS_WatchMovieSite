using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{    
    [DataContract]
    public class FilmServerInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }

        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("ServerName")]
        public string ServerName { get; set; }
        
        [DataMember]
        [DisplayName("ServerIP")]
        public string ServerIP { get; set; }

        [DataMember]
        [DisplayName("FolderRoot")]
        public string FolderRoot { get; set; }

        [DataMember]
        [DisplayName("Locations")]
        public string Locations { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DataMember]
        [DisplayName("IsVip")]
        public bool IsVip { get; set; }

        [DataMember]
        [DisplayName("IsDefault")]
        public bool IsDefault { get; set; }

        [DataMember]
        [DisplayName("UserName")]
        public string UserName { get; set; }

        [DataMember]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }
    }

    public class FilmServerMap : EntityTypeConfiguration<FilmServerInfo>, IEntityTypeConfiguration
    {

        public FilmServerMap()
        {
            ToTable("Modules_FilmServers");
            HasKey(m => m.Id);
            Property(m => m.LanguageCode).IsRequired().HasMaxLength(50);
            Property(m => m.SiteId).IsRequired();
            Property(m => m.ServerName).HasMaxLength(250).IsRequired();
            Property(m => m.ServerIP).HasMaxLength(50).IsRequired();
            Property(m => m.Locations).HasMaxLength(250);
            Property(m => m.IsVip).IsRequired();
            Property(m => m.IsDefault).IsRequired();
            Property(m => m.UserName).IsRequired().HasMaxLength(400);
            Property(m => m.Password).IsRequired().HasMaxLength(400);
        }
    }
}
