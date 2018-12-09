using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class PromotionInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DataMember]
        [DisplayName("Contents")]
        public string Contents { get; set; }

        [DataMember]
        [DisplayName("FromDate")]
        public DateTime FromDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextFromDate
        {
            get
            {
                return FromDate.ToString(Extensions.Constants.DateTimeFomat);
            }
        }

        [DataMember]
        [DisplayName("ToDate")]
        public DateTime ToDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextToDate
        {
            get
            {
                return ToDate.ToString(Extensions.Constants.DateTimeFomat);
            }
        }

        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }
    }

    public class PromotionMap : EntityTypeConfiguration<PromotionInfo>, IEntityTypeConfiguration
    {
        public PromotionMap()
        {
            ToTable("Modules_Promotions");
            HasKey(m => m.Id);
            Property(m => m.Title).IsRequired().HasMaxLength(250);
            Property(m => m.Contents).IsRequired();
            Property(m => m.FromDate).IsRequired();
            Property(m => m.ToDate).IsRequired();
        }
    }
}