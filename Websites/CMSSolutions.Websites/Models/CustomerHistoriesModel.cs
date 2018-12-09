using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class CustomerHistoriesModel
    {
        [ControlHidden()]
        public long Id { get; set; }
        
        [ControlNumeric(LabelText="Customer Id", Order=0, Required=true)]
        public int CustomerId { get; set; }
        
        [ControlDatePicker(LabelText="Create Date", Order=0, Required=true)]
        public System.DateTime CreateDate { get; set; }
        
        [ControlText(Required=true, MaxLength=250)]
        public string Action { get; set; }
        
        [ControlText(Required=true, MaxLength=2000)]
        public string Description { get; set; }
        
        [ControlNumeric(LabelText="Status", Order=0, Required=false)]
        public int Status { get; set; }

        public static implicit operator CustomerHistoriesModel(CustomerHistoriesInfo entity)
        {
            return new CustomerHistoriesModel
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                CreateDate = entity.CreateDate,
                Action = entity.Action,
                Description = entity.Description,
                Status = entity.Status
            };
        }

    }
}
