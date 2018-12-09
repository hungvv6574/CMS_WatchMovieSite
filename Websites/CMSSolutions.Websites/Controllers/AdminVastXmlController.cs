using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Xml;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Web.UI.Navigation;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = true), Authorize]
    public class AdminVastXmlController : BaseAdminController
    {
        public AdminVastXmlController(IWorkContextAccessor workContextAccessor) : base(workContextAccessor)
        {

        }

        [Url("admin/vast/xml")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý nhóm quảng cáo"), Url = Url.Action("Index","AdminAdvertisementGroup") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Tạo vast xml"), Url = "#" });

            var model = new VastXmlModel();
            var result = new ControlFormResult<VastXmlModel>(model)
            {
                Title = T("Tạo vast xml"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Tạo Xml"),
                CancelButtonText = T("Trở về"),
                CancelButtonUrl = Url.Action("Index", "AdminAdvertisementGroup"),
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.MakeReadOnlyProperty(x => x.Messages);

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/vast/xml/create")]
        public ActionResult Update(VastXmlModel model)
        {
            var service = WorkContext.Resolve<IAdvertisementGroupService>();
            var list = service.GetGroupGenerateXml(WorkContext.CurrentCulture);
            var data = service.GetDataGenerateXml(WorkContext.CurrentCulture).DefaultView;
            if (data.Count > 0)
            {
                foreach (var group in list)
                {
                    data.RowFilter = string.Format("Id IN ({0})", group.AdvertisementIds);
                    var listAds = new List<Ad>();
                    var doc = new XmlDocument();
                    foreach (DataRowView row in data)
                    {
                        var link = row["Link"].ToString();
                        if (link != "/")
                        {
                            continue;
                        }

                        var fileName = row["KeyCode"] + ".xml";
                        link = "/Media/Default/Advertisement/Vast/" + fileName;
                        var item = new Ad
                        {
                            Click = row["Click"].ToString(),
                            Duration = (int)row["Duration"],
                            Id = row["Id"].ToString(),
                            Link = link,
                            Position = (int)row["Position"],
                            Skip = (int)row["Skip"],
                            Type = row["Type"].ToString()
                        };
                        listAds.Add(item);

                        data.RowFilter = string.Format("AdId = {0}", item.Id);
                        foreach (DataRowView vast in data)
                        {
                            var dataVast = new Vast();
                            dataVast.ItemAds = new Ads
                            {
                                Id = int.Parse(item.Id),
                                ObjectInLine = new InLine
                                {
                                    ObjectAdSystem = new AdSystem
                                    {
                                        Version = vast["AdSystemVersion"].ToString(),
                                        Value = vast["AdSystemValue"].ToString()
                                    },
                                    AdTitle = vast["AdTitle"].ToString(),
                                    Error = vast["LinkError"].ToString(),
                                    Impression = vast["LinkImpression"].ToString(),
                                    Creatives = new Creatives
                                    {
                                        ListCreatives = new Creative[]
                                    {
                                        new Creative
                                        {
                                            ObjectLinear = new Linear
                                            {
                                                Skipoffset = vast["Skipoffset"].ToString(),
                                                Duration = vast["Duration"].ToString(),
                                                VideoClicks = new VideoClicks
                                                {
                                                    ClickThrough = vast["LinkClickThrough"].ToString()
                                                },
                                                TrackingEvents = new TrackingEvents
                                                {
                                                    LisTrackings = new Tracking[]
                                                    {
                                                        new Tracking { Event = vast["TrackingEvent1"].ToString(), Value = vast["TrackingValue1"].ToString()},
                                                        new Tracking { Event = vast["TrackingEvent2"].ToString(), Value = vast["TrackingValue2"].ToString()},
                                                        new Tracking { Event = vast["TrackingEvent3"].ToString(), Value = vast["TrackingValue3"].ToString()},
                                                        new Tracking { Event = vast["TrackingEvent4"].ToString(), Value = vast["TrackingValue4"].ToString()},
                                                        new Tracking { Event = vast["TrackingEvent5"].ToString(), Value = vast["TrackingValue5"].ToString()},
                                                        new Tracking { Event = vast["TrackingEvent6"].ToString(), Value = vast["TrackingValue6"].ToString()}
                                                    }
                                                },
                                                MediaFiles = new MediaFiles
                                                {
                                                    ListMediaFiles = new MediaFile[]
                                                    {
                                                        new MediaFile
                                                        {
                                                            Bitrate = (int)vast["MediaFileBitrate"],
                                                            Delivery = vast["MediaFileDelivery"].ToString(),
                                                            Height = (int)vast["MediaFileHeight"],
                                                            Width = (int)vast["MediaFileWidth"],
                                                            MaintainAspectRatio = bool.Parse(vast["MediaFileMaintainAspectRatio"].ToString()),
                                                            Scalable = bool.Parse(vast["MediaFileScalable"].ToString()),
                                                            Type = vast["MediaFileType"].ToString(),
                                                            Value = vast["MediaFileValue"].ToString()
                                                        } 
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    }
                                }
                            };

                            var xml = Utilities.SerializeXml<Vast>(dataVast);
                            doc = new XmlDocument();
                            doc.LoadXml(xml);
                            doc.Save(Server.MapPath(link));
                        }
                    }

                    if (listAds.Count > 0)
                    {
                        var ads = new Vplugin
                        {
                            ListAds = listAds.ToArray()
                        };

                        var name = group.Code + ".xml";
                        var path = "/Media/Default/Advertisement/";

                        var rootXml = Utilities.SerializeXml<Vplugin>(ads);
                        doc = new XmlDocument();
                        doc.LoadXml(rootXml);
                        doc.Save(Server.MapPath(path + name));

                        var obj = service.GetById(group.Id);
                        obj.IsGenerate = true;
                        obj.FolderPath = path;
                        obj.FileName = name;
                        obj.FullPath = path + name;
                        service.Update(obj);
                    }
                }   
            }

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(string.Format(T("Đã tạo thành công các file vast xml!")));
        }
    }
}
