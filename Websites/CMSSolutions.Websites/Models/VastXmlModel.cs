using CMSSolutions.Web.UI.ControlForms;

namespace CMSSolutions.Websites.Models
{
    public class VastXmlModel
    {
        public VastXmlModel()
        {
            Messages = "Chú ý: Khi bạn click vào nút Tạo Xml hệ thống sẽ tự động tìm tất cả những nhóm quảng cáo nào đã có quảng cáo và có nội dung quảng cáo để tạo thành các file xml theo chuẩn VAST 3.0.";
        }

        [ControlText(Type = ControlText.MultiText, Rows = 3, LabelText = "Ghi chú", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 0)]
        public string Messages { get; set; }
    }
}