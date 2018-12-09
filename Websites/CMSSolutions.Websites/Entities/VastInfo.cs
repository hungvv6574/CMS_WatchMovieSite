using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class VastInfo : BaseEntity<int>
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
        [DisplayName("AdId")]
        public int AdId { get; set; }

        [NotMapped]
        [DisplayName("AdName")]
        public string AdName { get; set; }

        [DataMember]
        [DisplayName("AdSystemVersion")]
        public string AdSystemVersion { get; set; }

        [DataMember]
        [DisplayName("AdSystemValue")]
        public string AdSystemValue { get; set; }

        [DataMember]
        [DisplayName("AdTitle")]
        public string AdTitle { get; set; }

        [DataMember]
        [DisplayName("LinkError")]
        public string LinkError { get; set; }

        [DataMember]
        [DisplayName("LinkImpression")]
        public string LinkImpression { get; set; }

        [DataMember]
        [DisplayName("Skipoffset")]
        public string Skipoffset { get; set; }

        [DataMember]
        [DisplayName("Duration")]
        public string Duration { get; set; }

        [DataMember]
        [DisplayName("LinkClickThrough")]
        public string LinkClickThrough { get; set; }

        [DataMember]
        [DisplayName("TrackingEvent1")]
        public string TrackingEvent1 { get; set; }

        [DataMember]
        [DisplayName("TrackingValue1")]
        public string TrackingValue1 { get; set; }

        [DataMember]
        [DisplayName("TrackingEvent2")]
        public string TrackingEvent2 { get; set; }

        [DataMember]
        [DisplayName("TrackingValue2")]
        public string TrackingValue2 { get; set; }

        [DataMember]
        [DisplayName("TrackingEvent3")]
        public string TrackingEvent3 { get; set; }

        [DataMember]
        [DisplayName("TrackingValue3")]
        public string TrackingValue3 { get; set; }

        [DataMember]
        [DisplayName("TrackingEvent4")]
        public string TrackingEvent4 { get; set; }

        [DataMember]
        [DisplayName("TrackingValue4")]
        public string TrackingValue4 { get; set; }

        [DataMember]
        [DisplayName("TrackingEvent5")]
        public string TrackingEvent5 { get; set; }

        [DataMember]
        [DisplayName("TrackingValue5")]
        public string TrackingValue5 { get; set; }

        [DataMember]
        [DisplayName("TrackingEvent6")]
        public string TrackingEvent6 { get; set; }

        [DataMember]
        [DisplayName("TrackingValue6")]
        public string TrackingValue6 { get; set; }

        [DataMember]
        [DisplayName("MediaFileBitrate")]
        public int MediaFileBitrate { get; set; }

        [DataMember]
        [DisplayName("MediaFileDelivery")]
        public string MediaFileDelivery { get; set; }

        [DataMember]
        [DisplayName("MediaFileHeight")]
        public int MediaFileHeight { get; set; }

        [DataMember]
        [DisplayName("MediaFileWidth")]
        public int MediaFileWidth { get; set; }

        [DataMember]
        [DisplayName("MediaFileMaintainAspectRatio")]
        public bool MediaFileMaintainAspectRatio { get; set; }

        [DataMember]
        [DisplayName("MediaFileScalable")]
        public bool MediaFileScalable { get; set; }

        [DataMember]
        [DisplayName("MediaFileType")]
        public string MediaFileType { get; set; }

        [DataMember]
        [DisplayName("MediaFileValue")]
        public string MediaFileValue { get; set; }
    }

    public class VastMap : EntityTypeConfiguration<VastInfo>, IEntityTypeConfiguration
    {

        public VastMap()
        {
            ToTable("Modules_Vast");
            HasKey(m => m.Id);
            Property(m => m.LanguageCode).HasMaxLength(50);
            Property(m => m.KeyCode).HasMaxLength(50);
            Property(m => m.AdSystemVersion).HasMaxLength(50);
            Property(m => m.AdSystemValue).HasMaxLength(250);
            Property(m => m.AdTitle).HasMaxLength(250);
            Property(m => m.LinkError).HasMaxLength(500);
            Property(m => m.LinkImpression).HasMaxLength(500);
            Property(m => m.Skipoffset).HasMaxLength(50);
            Property(m => m.Duration).HasMaxLength(50);
            Property(m => m.LinkClickThrough).HasMaxLength(500);
            Property(m => m.TrackingEvent1).HasMaxLength(50);
            Property(m => m.TrackingValue1).HasMaxLength(500);
            Property(m => m.TrackingEvent2).HasMaxLength(50);
            Property(m => m.TrackingValue2).HasMaxLength(500);
            Property(m => m.TrackingEvent3).HasMaxLength(50);
            Property(m => m.TrackingValue3).HasMaxLength(500);
            Property(m => m.TrackingEvent4).HasMaxLength(50);
            Property(m => m.TrackingValue4).HasMaxLength(500);
            Property(m => m.TrackingEvent5).HasMaxLength(50);
            Property(m => m.TrackingValue5).HasMaxLength(500);
            Property(m => m.TrackingEvent6).HasMaxLength(50);
            Property(m => m.TrackingValue6).HasMaxLength(500);
            Property(m => m.MediaFileDelivery).HasMaxLength(50);
            Property(m => m.MediaFileType).HasMaxLength(50);
            Property(m => m.MediaFileValue).HasMaxLength(500);
        }
    }
}
