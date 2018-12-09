using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class LogInfo : BaseEntity<long>
    {
        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextCreateDate
        {
            get
            {
                return CreateDate.ToString(Extensions.Constants.DateTimeFomatFull);
            }
        }

        [DataMember]
        [DisplayName("Type")]
        public int Type { get; set; }

        [DataMember]
        [DisplayName("Messages")]
        public string Messages { get; set; }

        [DataMember]
        [DisplayName("Keyword")]
        public string Keyword { get; set; }

        [DataMember]
        [DisplayName("Contents")]
        public string Contents { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }
    }

    public class LogMap : EntityTypeConfiguration<LogInfo>, IEntityTypeConfiguration
    {

        public LogMap()
        {
            ToTable("Modules_Logs");
            HasKey(m => m.Id);
            Property(m => m.CreateDate).IsRequired();
            Property(m => m.Messages).IsRequired();
            Property(m => m.Keyword).HasMaxLength(250);
            Property(m => m.Contents).HasMaxLength(250);
        }
    }
}