using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CMSSolutions.Data;
using CMSSolutions.Data.Entity;

namespace CMSSolutions.Websites.Entities
{
    [DataContract]
    public class CustomerInfo : BaseEntity<int>
    {
        [DataMember]
        [DisplayName("CustomerCode")]
        public string CustomerCode { get; set; }

        [DataMember]
        [DisplayName("UserName")]
        public string UserName { get; set; }

        [DataMember]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DataMember]
        [DisplayName("FullName")]
        public string FullName { get; set; }

        [DataMember]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DataMember]
        [DisplayName("Address")]
        public string Address { get; set; }

        [DataMember]
        [DisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember]
        [DisplayName("CityId")]
        public string CityId { get; set; }

        [DataMember]
        [DisplayName("Birthday")]
        public System.DateTime Birthday { get; set; }

        [DataMember]
        [DisplayName("Sex")]
        public int Sex { get; set; }

        [DataMember]
        [DisplayName("ImageIcon")]
        public string ImageIcon { get; set; }

        [DataMember]
        [DisplayName("FilmTypeIds")]
        public string FilmTypeIds { get; set; }

        [DataMember]
        [DisplayName("CountryIds")]
        public string CountryIds { get; set; }

        [DataMember]
        [DisplayName("MemberDate")]
        public System.DateTime MemberDate { get; set; }

        [DataMember]
        [DisplayName("Skype")]
        public string Skype { get; set; }

        [DataMember]
        [DisplayName("ZingMe")]
        public string ZingMe { get; set; }

        [DataMember]
        [DisplayName("Facebook")]
        public string Facebook { get; set; }

        [DataMember]
        [DisplayName("Google")]
        public string Google { get; set; }

        [DataMember]
        [DisplayName("Yahoo")]
        public string Yahoo { get; set; }

        [DataMember]
        [DisplayName("VipXu")]
        public decimal VipXu { get; set; }

        [DataMember]
        [DisplayName("TotalMoney")]
        public decimal TotalMoney { get; set; }

        [DataMember]
        [DisplayName("IsBlock")]
        public bool IsBlock { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public int Status { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DataMember]
        [DisplayName("TotalDay")]
        public int TotalDay { get; set; }

        [DataMember]
        [DisplayName("StartDate")]
        public DateTime? StartDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextStartDate
        {
            get
            {
                if (StartDate == null)
                {
                    return "";
                }

                return StartDate.Value.ToString(Extensions.Constants.DateTimeFomatFull);
            }
        }

        [DataMember]
        [DisplayName("EndDate")]
        public DateTime? EndDate { get; set; }

        [NotMapped]
        [DisplayName(Constants.NotMapped)]
        public string TextEndDate
        {
            get
            {
                if (EndDate == null)
                {
                    return "";
                }

                return EndDate.Value.ToString(Extensions.Constants.DateTimeFomatFull);
            }
        }

        [DataMember]
        [DisplayName("IsTest")]
        public bool IsTest { get; set; }
    }

    public class CustomerMap : EntityTypeConfiguration<CustomerInfo>, IEntityTypeConfiguration
    {

        public CustomerMap()
        {
            ToTable("Modules_Customers");
            HasKey(m => m.Id);
            Property(m => m.CustomerCode).IsRequired().HasMaxLength(250);
            Property(m => m.UserName).IsRequired().HasMaxLength(50);
            Property(m => m.Password).IsRequired().HasMaxLength(500);
            Property(m => m.FullName).IsRequired().HasMaxLength(250);
            Property(m => m.Email).IsRequired().HasMaxLength(50);
            Property(m => m.Address).HasMaxLength(2000);
            Property(m => m.PhoneNumber).HasMaxLength(50);
            Property(m => m.CityId).HasMaxLength(250);
            Property(m => m.ImageIcon).IsRequired().HasMaxLength(300);
            Property(m => m.FilmTypeIds).HasMaxLength(2000);
            Property(m => m.CountryIds).HasMaxLength(2000);
            Property(m => m.MemberDate).IsRequired();
            Property(m => m.Skype).HasMaxLength(50);
            Property(m => m.ZingMe).HasMaxLength(50);
            Property(m => m.Facebook).HasMaxLength(50);
            Property(m => m.Google).HasMaxLength(50);
            Property(m => m.Yahoo).HasMaxLength(50);
            Property(m => m.IsBlock).IsRequired();
            Property(m => m.Description).HasMaxLength(500);
        }
    }
}
