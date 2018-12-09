using System;
using CMSSolutions.Web.UI.ControlForms;

namespace CMSSolutions.Websites.Models
{
    public class SmsCloseModel
    {
        public SmsCloseModel()
        {
            EndDate = DateTime.Now;
            Messages = "Bạn chắc chắn đã xuất dữ liệu chưa đối soát? Hãy lưu lại nó vì nếu bạn chốt sẽ rồi sẽ không lấy lại được dữ liệu chưa đối soát.";
        }

        [ControlText(Type = ControlText.MultiText,Rows = 3, LabelText = "Ghi chú", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public string Messages { get; set; }

        [ControlDatePicker(LabelText = "Tính đến hết ngày", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public DateTime EndDate { get; set; }
    }
}