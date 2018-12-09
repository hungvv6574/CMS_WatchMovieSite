using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class AdvertisementGroupInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("SiteId")]
        public int SiteId { get; set; }

        [DataMember]
        [DisplayName("LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember]
        [DisplayName("CategoryId")]
        public int CategoryId { get; set; }

        [NotMapped]
        [DisplayName("CategoryName")]
        public string CategoryName { get; set; }

        [DataMember]
        [DisplayName("Code")]
        public string Code { get; set; }

        [DataMember]
        [DisplayName("GroupName")]
        public string GroupName { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DataMember]
        [DisplayName("AdvertisementIds")]
        public string AdvertisementIds { get; set; }

        [DataMember]
        [DisplayName("IsGenerate")]
        public bool IsGenerate { get; set; }

        [DataMember]
        [DisplayName("IsActived")]
        public bool IsActived { get; set; }

        [DataMember]
        [DisplayName("FolderPath")]
        public string FolderPath { get; set; }

        [DataMember]
        [DisplayName("FileName")]
        public string FileName { get; set; }

        [DataMember]
        [DisplayName("FullPath")]
        public string FullPath { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string EncodeFullPath
        {
            get
            {
                if (!string.IsNullOrEmpty(FullPath))
                {
                    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(FullPath));
                }

                return string.Empty;
            }
        }

        [DataMember]
        [DisplayName("CreateDate")]
        public DateTime CreateDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextCreateDate
        {
            get
            {
                return string.Format("{0}", CreateDate.ToString(Extensions.Constants.DateTimeFomat));
            }
        }

        [DataMember]
        [DisplayName("FinishDate")]
        public DateTime FinishDate { get; set; }

        [DataMember]
        [DisplayName("EndTime")]
        public string FinishTime { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextFinishTime
        {
            get
            {
                return string.Format("{0} {1}", FinishDate.ToString(Extensions.Constants.DateTimeFomat), FinishTime);
            }
        }
    }

    public class AdvertisementGroupMap : EntityTypeConfiguration<AdvertisementGroupInfo>, IEntityTypeConfiguration
    {

        public AdvertisementGroupMap()
        {
            ToTable("Modules_AdvertisementGroup");
            Property(m => m.LanguageCode).HasMaxLength(50);
            Property(m => m.Code).HasMaxLength(50);
            Property(m => m.GroupName).HasMaxLength(250);
            Property(m => m.AdvertisementIds).HasMaxLength(250);
            Property(m => m.FolderPath).HasMaxLength(250);
            Property(m => m.FileName).HasMaxLength(250);
            Property(m => m.FullPath).HasMaxLength(500);
            Property(m => m.Description).HasMaxLength(2000);
        }
    }
}
