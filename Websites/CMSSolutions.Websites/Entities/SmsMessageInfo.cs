using System.ComponentModel;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class SmsMessageInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("Code")]
        public string Code { get; set; }

        [DataMember]
        [DisplayName("Message")]
        public string Message { get; set; }

        [DataMember]
        [DisplayName("IsEvent")]
        public bool IsEvent { get; set; }
    }

    public class SmsMessageMap : EntityTypeConfiguration<SmsMessageInfo>, IEntityTypeConfiguration
    {
        public SmsMessageMap()
        {
            ToTable("Modules_SmsMessages");
            HasKey(x => x.Id);
            Property(m => m.LanguageCode).IsRequired().HasMaxLength(50);
            Property(x => x.Code).HasMaxLength(50).IsRequired();
            Property(x => x.Message).IsRequired().HasMaxLength(250);
        }
    }
}