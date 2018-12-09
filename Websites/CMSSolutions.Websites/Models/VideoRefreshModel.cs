using CMSSolutions.Web.UI.ControlForms;

namespace CMSSolutions.Websites.Models
{
    public class VideoRefreshModel
    {
        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Máy chủ", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.ChildrenFolders + "').empty();")]
        public int ServerId { get; set; }

        [ControlCascadingDropDown(LabelText = "Thư mục gốc", ParentControl = "ServerId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string RootFolders { get; set; }

        [ControlCascadingDropDown(AllowMultiple = true, LabelText = "Thư mục con", ParentControl = "RootFolders", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string ChildrenFolders { get; set; }
    }
}