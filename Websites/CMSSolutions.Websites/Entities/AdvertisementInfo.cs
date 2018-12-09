using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class AdvertisementInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }

        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("KeyCode")]
        public string KeyCode { get; set; }

        [DataMember]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DataMember]
        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DataMember]
        [DisplayName("Code")]
        public string Code { get; set; }

        [DataMember]
        [DisplayName("Link")]
        public string Link { get; set; }

        [DataMember]
        [DisplayName("Type")]
        public string Type { get; set; }

        [DataMember]
        [DisplayName("Click")]
        public string Click { get; set; }

        [DataMember]
        [DisplayName("Duration")]
        public int Duration { get; set; }

        [DataMember]
        [DisplayName("Position")]
        public int Position { get; set; }

        [DataMember]
        [DisplayName("Skip")]
        public int Skip { get; set; }

        [DataMember]
        [DisplayName("IsBlock")]
        public bool IsBlock { get; set; }
    }

    public class AdvertisementMap : EntityTypeConfiguration<AdvertisementInfo>, IEntityTypeConfiguration
    {
        public AdvertisementMap()
        {
            ToTable("Modules_Advertisement");
            HasKey(m => m.Id);
            Property(m => m.LanguageCode).HasMaxLength(50);
            Property(m => m.KeyCode).IsRequired().HasMaxLength(50);
            Property(m => m.Title).IsRequired().HasMaxLength(250);
            Property(m => m.Code).IsRequired().HasMaxLength(50);
            Property(m => m.Link).IsRequired().HasMaxLength(500);
            Property(m => m.Type).IsRequired().HasMaxLength(50);
            Property(m => m.Click).HasMaxLength(500);
            Property(m => m.Duration).IsRequired();
            Property(m => m.Position).IsRequired();
            Property(m => m.Skip).IsRequired();
        }
    }
}