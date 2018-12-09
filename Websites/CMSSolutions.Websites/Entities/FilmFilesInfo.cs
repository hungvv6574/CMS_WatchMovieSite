using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Entities
{ 
    [DataContract]
    public class FilmFilesInfo : BaseEntity<long>
    {
        [DataMember]
        [DisplayName("FileCode")]
        public string FileCode { get; set; }

        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }

        [DataMember]
        [DisplayName("ServerId")]
        public int ServerId { get; set; }

        [NotMapped]
        [DisplayName("ServerName")]
        public string ServerName { get; set; }

        [DataMember]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DataMember]
        [DisplayName("FolderRoot")]
        public string FolderRoot { get; set; }

        [DataMember]
        [DisplayName("FolderName")]
        public string FolderName { get; set; }

        [DataMember]
        [DisplayName("FolderDay")]
        public string FolderDay { get; set; }

        [DataMember]
        [DisplayName("FolderPath")]
        public string FolderPath { get; set; }

        [DataMember]
        [DisplayName("FileName")]
        public string FileName { get; set; }

        [DataMember]
        [DisplayName("Extentions")]
        public string Extentions { get; set; }

        [DataMember]
        [DisplayName("FullPath")]
        public string FullPath { get; set; }

        [DataMember]
        [DisplayName("Size")]
        public string Size { get; set; }

        [DataMember]
        [DisplayName("CreateDate")]
        public System.DateTime CreateDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string DisplayDate { get { return Utilities.DateString(CreateDate); } }

        [DataMember]
        [DisplayName("HasUse")]
        public bool HasUse { get; set; }
    }

    public class FilmFilesMap : EntityTypeConfiguration<FilmFilesInfo>, IEntityTypeConfiguration
    {
        public FilmFilesMap()
        {
            ToTable("Modules_FilmFiles");
            HasKey(m => m.Id);
            Property(m => m.FileCode).IsRequired().HasMaxLength(50);
            Property(m => m.LanguageCode).HasMaxLength(50);
            Property(m => m.Name).IsRequired().HasMaxLength(250);
            Property(m => m.FolderRoot).HasMaxLength(250);
            Property(m => m.FolderName).HasMaxLength(100);
            Property(m => m.FolderPath).IsRequired().HasMaxLength(300);
            Property(m => m.FileName).IsRequired().HasMaxLength(100);
            Property(m => m.Extentions).HasMaxLength(50);
            Property(m => m.FullPath).IsRequired().HasMaxLength(500);
            Property(m => m.CreateDate).IsRequired();
            Property(m => m.HasUse).IsRequired();
            Property(m => m.Size).HasMaxLength(250);
            Property(m => m.ServerId).IsRequired();
            Property(m => m.FolderDay).HasMaxLength(100);
        }
    }
}
