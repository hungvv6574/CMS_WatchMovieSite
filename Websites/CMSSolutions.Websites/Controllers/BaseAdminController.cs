using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CMSSolutions.Configuration;
using CMSSolutions.Extensions;
using CMSSolutions.Localization.Domain;
using CMSSolutions.Localization.Services;
using CMSSolutions.Security.Cryptography;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Security;
using CMSSolutions.Web.Security.Services;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Authorize]
    [Themed(IsDashboard = true)]
    public class BaseAdminController : BaseController
    {
        public BaseAdminController(
            IWorkContextAccessor workContextAccessor)
            : base(workContextAccessor)
        {
            
        }

        public virtual FTPClient GetConnectionFtp(int serverId, out string rootFolder, out string languageCode, out int siteId)
        {
            var service = WorkContext.Resolve<IFilmServersService>();
            var server = service.GetById(serverId);
            if (server != null)
            {
                string password = EncryptionExtensions.Decrypt(KeyConfiguration.PublishKey, server.Password);
                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException(T(SecurityConstants.ErrorConfigKey).Text);
                }

                string userName = EncryptionExtensions.Decrypt(KeyConfiguration.PublishKey, server.UserName);
                if (string.IsNullOrEmpty(userName))
                {
                    throw new ArgumentException(T(SecurityConstants.ErrorConfigKey).Text);
                }

                rootFolder = server.FolderRoot;
                languageCode = server.LanguageCode;
                siteId = server.SiteId;
                return new FTPClient(server.ServerIP, userName, password);
            }

            rootFolder = string.Empty;
            languageCode = string.Empty;
            siteId = 0;
            return null;
        }

        public virtual string BuildFromDate(bool showDefaultDate = true)
        {
            var sb = new StringBuilder();
            var date = "";
            if (showDefaultDate)
            {
                date = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");
            }

            var fromDateSelected = GetQueryString(9);
            if (!string.IsNullOrEmpty(fromDateSelected))
            {
                date = fromDateSelected;
            }

            sb.AppendFormat(T("Từ ngày") + " <input id=\"" + Extensions.Constants.FromDate + "\" name=\"" + Extensions.Constants.FromDate + "\" value=\"" + date + "\" class=\"form-control datepicker\"></input>");
            sb.Append("<script>$(document).ready(function () { " +
                      "$('.datepicker').datepicker({ " +
                      "dateFormat: 'dd/mm/yy', " +
                      "changeMonth: true, " +
                      "changeYear: true, " +
                      "onSelect: function (dateText) { " +
                      "$('#" + TableName + "').jqGrid().trigger('reloadGrid'); " +
                      "}}); });</script>");

            return sb.ToString();
        }

        public virtual string BuildToDate(bool showDefaultDate = true)
        {
            var sb = new StringBuilder();
            var date = "";
            if (showDefaultDate)
            {
                date = DateTime.Now.ToString("dd/MM/yyyy");
            }

            var toDateSelected = GetQueryString(10);
            if (!string.IsNullOrEmpty(toDateSelected))
            {
                date = toDateSelected;
            }

            sb.AppendFormat(T("Đến ngày") + " <input id=\"" + Extensions.Constants.ToDate + "\" name=\"" + Extensions.Constants.ToDate + "\" value=\"" + date + "\" class=\"form-control datepicker\"></input>");
            sb.Append("<script>$(document).ready(function () { " +
                      "$('.datepicker').datepicker({ " +
                      "dateFormat: 'dd/mm/yy', " +
                      "changeMonth: true, " +
                      "changeYear: true, " +
                      "onSelect: function (dateText) { " +
                      "$('#" + TableName + "').jqGrid().trigger('reloadGrid'); " +
                      "}}); });</script>");

            return sb.ToString();
        }

        public virtual string BuildSearchText()
        {
            var sb = new StringBuilder();

            var keyword = GetQueryString(11);
            sb.AppendFormat(T("Từ khóa") + " <input value=\"" + keyword + "\" placeholder=\"" + T("Nhập từ khóa cần tìm.") + "\" id=\"" + Extensions.Constants.SearchText + "\" name=\"" + Extensions.Constants.SearchText + "\" class=\"form-control\" onkeypress = \"return InputEnterEvent(event, '" + TableName + "');\" onblur=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\"></input>");

            return sb.ToString();
        }

        public virtual string BuildStatus()
        {
            var list = EnumExtensions.GetListItems<Status>();
            var statusSelected = GetQueryString(8);
            var sb = new StringBuilder();
            sb.AppendFormat(T("Trạng thái") + " <select id=\"" + Extensions.Constants.StatusId + "\" name=\"" + Extensions.Constants.StatusId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var status in list)
            {
                if (!string.IsNullOrEmpty(statusSelected) && status.Value == statusSelected)
                {
                    sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", status.Text, status.Value);
                    continue;
                }

                sb.AppendFormat("<option value=\"{1}\">{0}</option>", status.Text, status.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        public virtual string BuildServers()
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var siteId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId]))
            {
                siteId = Convert.ToInt32(Request.Form[Extensions.Constants.SiteId]);
            }

            var service = WorkContext.Resolve<IFilmServersService>();
            var list = service.GetRecords(x => x.LanguageCode == languageCode && x.SiteId == siteId);
            var sb = new StringBuilder();
            sb.AppendFormat(T("Máy chủ") + " <select id=\"" + Extensions.Constants.ServerId + "\" name=\"" + Extensions.Constants.ServerId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var server in list)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", server.ServerName, server.Id);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        public virtual IEnumerable<SelectListItem> BindServers()
        {
            var service = WorkContext.Resolve<IFilmServersService>();
            var items = service.GetRecords();
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = string.Format("[{0}] {1} {2}", item.ServerIP, item.ServerName, item.IsVip ? T(" - VIP") : T(" - Thường")),
                Value = item.Id.ToString(),
                Selected = item.IsDefault
            }));

            result.Insert(0, new SelectListItem { Text = "--- Chọn máy chủ ---", Value = "0" });

            return result;
        }

        public virtual string BuildUsers()
        {
            var service = WorkContext.Resolve<IMembershipService>();
            var list = service.GetRecords(x => !x.IsLockedOut);
            var sb = new StringBuilder();
            var userSelected = GetQueryString(3);
            sb.AppendFormat(T("Người quản trị") + " <select id=\"" + Extensions.Constants.UserId + "\" name=\"" + Extensions.Constants.UserId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var user in list)
            {
                if (string.IsNullOrEmpty(userSelected))
                {
                    if (user.Id == WorkContext.CurrentUser.Id)
                    {
                        sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", user.FullName, user.Id);
                        continue;
                    }
                }

                if (!string.IsNullOrEmpty(userSelected) && user.Id == int.Parse(userSelected))
                {
                    sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", user.FullName, user.Id);
                    continue;
                }
                
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", user.FullName, user.Id);
            }

            sb.Append("</select>");

            return sb.ToString();
        }

        public virtual string BuildCountries()
        {
            var service = WorkContext.Resolve<ICountryService>();
            var items = service.GetRecords();
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem {Text = T("--- Chọn nước sản xuât ---"), Value = "0"});

            var sb = new StringBuilder();
            sb.AppendFormat(T("Nước sản xuất") + " <select id=\"" + Extensions.Constants.CountryId + "\" name=\"" + Extensions.Constants.CountryId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var item in result)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", item.Text, item.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        public string GetQueryString(int index)
        {
            var returnUrl = Request.QueryString[Extensions.Constants.ReturnUrl];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                var data = Encoding.UTF8.GetString(Convert.FromBase64String(returnUrl));
                if (!string.IsNullOrEmpty(data))
                {
                    var list = data.Split(',');
                    if (list.Length > index)
                    {
                        var result = list[index];
                        if (result != "null")
                        {
                            return result;
                        }
                    }
                }
            }

            return string.Empty;
        }

        public virtual string BuildLanguages(bool hasEvent, string controlName)
        {
            var service = WorkContext.Resolve<ILanguageService>();
            var items = service.GetActiveLanguages();
            var list = new List<Language>();
            list.AddRange(items.Where(x => x.Theme == Constants.ThemeDefault));
            var sb = new StringBuilder();

            if (hasEvent)
            {
                var url = Url.Action("GetSitesByLanguage");
                sb.AppendFormat(T("Ngôn ngữ") + " <select id=\"" + Extensions.Constants.LanguageCode + "\" name=\"" + Extensions.Constants.LanguageCode + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"" +
                " var data = $('#" + TableName + "_Container').find('select').serialize();" +
                    @"$.ajax({{
	                url: '" + url + @"',
	                data: data,
	                type: 'POST',
	                dataType: 'json',
	                success: function (result) {{
                        $('#" + controlName + @"').empty();
                        if (result != null){{
		                    $.each(result, function(idx, item) {{                  
                                $('#" + controlName + @"').append($('<option>', {{
                                    value: item.Value,
                                    text: item.Text
                                }}));
                            }});   
                        }}
	                }}
                }});
                $('#" + TableName + "').jqGrid().trigger('reloadGrid');" +
                "\">");
            }
            else
            {
                sb.AppendFormat(T("Ngôn ngữ") + " <select id=\"" + Extensions.Constants.LanguageCode + "\" name=\"" + Extensions.Constants.LanguageCode + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            }

            var languageSelected = GetQueryString(0);
            foreach (var language in list)
            {
                if (string.IsNullOrEmpty(languageSelected))
                {
                    if (language.CultureCode == WorkContext.CurrentCulture)
                    {
                        sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", language.Name, language.CultureCode);
                        continue;
                    }
                }

                if (!string.IsNullOrEmpty(languageSelected) && language.CultureCode == languageSelected)
                {
                    sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", language.Name, language.CultureCode);
                    continue;
                }

                sb.AppendFormat("<option value=\"{1}\">{0}</option>", language.Name, language.CultureCode);
            }

            sb.Append("</select>");

            return sb.ToString();
        }

        public virtual IEnumerable<SelectListItem> BindFilmTypes()
        {
            var service = WorkContext.Resolve<IFilmTypesService>();
            var items = service.GetRecords(x => x.Status == (int)Status.Approved);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }));

            return result;
        }

        public virtual IEnumerable<SelectListItem> BindCountries()
        {
            var service = WorkContext.Resolve<ICountryService>();
            var items = service.GetRecords();
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }));

            return result;
        }

        public virtual string BuildSites(bool hasEvent, string controlName)
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }
            var service = WorkContext.Resolve<ISiteService>();
            var list = service.GetRecords(x => x.LanguageCode == languageCode);
            var result = new List<SelectListItem>();
            result.AddRange(list.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.IsActived
            }));
            result.Insert(0, new SelectListItem { Text = "--- Chọn trang web ---", Value = "0" });

            var url = Url.Action("GetCategoriesBySite");
            var sb = new StringBuilder();
            if (hasEvent)
            {
                sb.AppendFormat(T("Trang web") + " <select id=\"" + Extensions.Constants.SiteId + "\" name=\"" + Extensions.Constants.SiteId +
                "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"" +
                " var data = $('#" + TableName + "_Container').find('select').serialize();" +
                @"$.ajax({{
	                url: '" + url + @"',
	                data: data,
	                type: 'POST',
	                dataType: 'json',
	                success: function (result) {{
                        $('#" + controlName + @"').empty();
                        if (result != null){{
		                    $.each(result, function(idx, item) {{
                                $('#" + controlName + @"').append($('<option>', {{
                                    value: item.Value,
                                    text: item.Text
                                }}));
                            }});   
                        }} 
	                }}
                }});
                $('#" + TableName + "').jqGrid().trigger('reloadGrid');" +
                "\">");
            }
            else
            {
                sb.AppendFormat(T("Trang web") + " <select id=\"" + Extensions.Constants.SiteId + "\" name=\"" + Extensions.Constants.SiteId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            }

            var siteSelected = GetQueryString(1);
            foreach (var site in result)
            {
                if (!string.IsNullOrEmpty(siteSelected) && site.Value == siteSelected)
                {
                    sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", site.Text, site.Value);
                    continue;
                }

                sb.AppendFormat("<option value=\"{1}\">{0}</option>", site.Text, site.Value);
            }
            sb.Append("</select>");

            var categorySelected = GetQueryString(2);
            if (!string.IsNullOrEmpty(siteSelected) && !string.IsNullOrEmpty(categorySelected))
            {
                sb.AppendFormat("<script type=\"text/javascript\">$(document).ready(function() {{" +
                "var data = $('#" + TableName + "_Container').find('select').serialize();" +
                @"$.ajax({{
	                url: '" + url + @"',
	                data: data,
	                type: 'POST',
	                dataType: 'json',
	                success: function (result) {{
                        $('#" + controlName + @"').empty();
                        if (result != null){{
		                    $.each(result, function(idx, item) {{
                                if(item.Value == '"+categorySelected+@"'){{
                                    $('#" + controlName + @"').append('<option selected value=' + item.Value + '>'+ item.Text +'</option>');
                                }} else{{
                                    $('#" + controlName + @"').append('<option value=' + item.Value + '>'+ item.Text +'</option>');
                                }}                               
                            }}); 
                        }} 
	                }}
                }});}});</script>");
            }

            return sb.ToString();
        }

        public virtual string BuildSiteServers(bool hasEvent, string controlName)
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }
            var service = WorkContext.Resolve<ISiteService>();
            var list = service.GetRecords(x => x.LanguageCode == languageCode);
            var result = new List<SelectListItem>();
            result.AddRange(list.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.IsActived
            }));
            result.Insert(0, new SelectListItem { Text = "--- Chọn trang web ---", Value = "0" });

            var sb = new StringBuilder();
            if (hasEvent)
            {
                var url = Url.Action("GetServersBySite");
                sb.AppendFormat(T("Trang web") + " <select id=\"" + Extensions.Constants.SiteId + "\" name=\"" + Extensions.Constants.SiteId +
                "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"" +
                " var data = $('#" + TableName + "_Container').find('select').serialize();" +
                @"$.ajax({{
	                url: '" + url + @"',
	                data: data,
	                type: 'POST',
	                dataType: 'json',
	                success: function (result) {{
                        $('#" + controlName + @"').empty();
                        if (result != null){{
		                    $.each(result, function(idx, item) {{
                                $('#" + controlName + @"').append($('<option>', {{
                                    value: item.Value,
                                    text: item.Text
                                }}));
                            }});   
                        }} 
	                }}
                }});
                $('#" + TableName + "').jqGrid().trigger('reloadGrid');" +
                "\">");
            }
            else
            {
                sb.AppendFormat(T("Trang web") + " <select id=\"" + Extensions.Constants.SiteId + "\" name=\"" + Extensions.Constants.SiteId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            }

            foreach (var site in result)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", site.Text, site.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        [Url("admin/vast/get-ads-by-site")]
        public ActionResult GetAdBySite()
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var siteId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId]))
            {
                siteId = Convert.ToInt32(Request.Form[Extensions.Constants.SiteId]);
            }

            var service = WorkContext.Resolve<IAdvertisementService>();
            var items = service.GetAdBySite(languageCode, siteId);
            var result = new List<SelectListItem>();

            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            }));

            return Json(result);
        }

        public virtual string BuildSiteAds(bool hasEvent, string controlName)
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }
            var service = WorkContext.Resolve<ISiteService>();
            var list = service.GetRecords(x => x.LanguageCode == languageCode);
            var result = new List<SelectListItem>();
            result.AddRange(list.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.IsActived
            }));
            result.Insert(0, new SelectListItem { Text = "--- Chọn trang web ---", Value = "0" });

            var sb = new StringBuilder();
            if (hasEvent)
            {
                var url = Url.Action("GetAdsBySite");
                sb.AppendFormat(T("Trang web") + " <select id=\"" + Extensions.Constants.SiteId + "\" name=\"" + Extensions.Constants.SiteId +
                "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"" +
                " var data = $('#" + TableName + "_Container').find('select').serialize();" +
                @"$.ajax({{
	                url: '" + url + @"',
	                data: data,
	                type: 'POST',
	                dataType: 'json',
	                success: function (result) {{
                        $('#" + controlName + @"').empty();
                        if (result != null){{
		                    $.each(result, function(idx, item) {{
                                $('#" + controlName + @"').append($('<option>', {{
                                    value: item.Value,
                                    text: item.Text
                                }}));
                            }});   
                        }} 
	                }}
                }});
                $('#" + TableName + "').jqGrid().trigger('reloadGrid');" +
                "\">");
            }
            else
            {
                sb.AppendFormat(T("Trang web") + " <select id=\"" + Extensions.Constants.SiteId + "\" name=\"" + Extensions.Constants.SiteId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            }

            foreach (var site in result)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", site.Text, site.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        public virtual string BuildCategories()
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var siteId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId]))
            {
                siteId = Convert.ToInt32(Request.Form[Extensions.Constants.SiteId]);
            }
            var service = WorkContext.Resolve<ICategoryService>();
            service.LanguageCode = languageCode;
            service.SiteId = siteId;
            var sb = new StringBuilder();

            var categorySelected = GetQueryString(2);
            sb.AppendFormat(T("Chuyên mục") + " <select id=\"" + Extensions.Constants.CategoryId + "\" name=\"" + Extensions.Constants.CategoryId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#"+TableName+"').jqGrid().trigger('reloadGrid');\">");
            var list = service.GetTree();
            foreach (var cate in list)
            {
                if (!string.IsNullOrEmpty(categorySelected) && cate.Id == int.Parse(categorySelected))
                {
                    sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", cate.ChildenName, cate.Id);
                    continue;
                }

                sb.AppendFormat("<option value=\"{1}\">{0}</option>", cate.ChildenName, cate.Id);
            }

            sb.Append("</select>");

            return sb.ToString();
        }

        public virtual IEnumerable<SelectListItem> BindLanguages()
        {
            var service = WorkContext.Resolve<ILanguageService>();
            var items = service.GetActiveLanguages();
            var result = new List<SelectListItem>();
            result.AddRange(items.Where(x => x.Theme == Constants.ThemeDefault).Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.CultureCode.ToString(),
                Selected = item.CultureCode == WorkContext.CurrentCulture
            }));

            return result;
        }

        public virtual IEnumerable<SelectListItem> BindSites()
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var service = WorkContext.Resolve<ISiteService>();
            var items = service.GetRecords(x => x.LanguageCode == languageCode);
            var result = new List<SelectListItem>();

            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.IsActived
            }));
            result.Insert(0, new SelectListItem { Text = "--- Chọn trang web ---", Value = "0" });

            return result;
        }

        public virtual string BuildDirectors()
        {
            var service = WorkContext.Resolve<IDirectorService>();
            var items = service.GetRecords(x => x.Status == (int)Status.Approved);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem { Text = "--- Đạo diễn phim ---", Value = "0" });

            var directorSelected = GetQueryString(4);
            var sb = new StringBuilder();
            sb.AppendFormat(T("Đạo diễn") + " <select id=\"" + Extensions.Constants.DirectorId + "\" name=\"" + Extensions.Constants.DirectorId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var director in result)
            {
                if (!string.IsNullOrEmpty(directorSelected) && director.Value == directorSelected)
                {
                    sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", director.Text, director.Value);
                    continue;
                }
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", director.Text, director.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        public virtual string BuildActors()
        {
            var service = WorkContext.Resolve<IActorService>();
            var items = service.GetRecords(x => x.Status == (int)Status.Approved);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem { Text = "--- Diễn viên ---", Value = "0" });

            var actorSelected = GetQueryString(5);
            var sb = new StringBuilder();
            sb.AppendFormat(T("Diễn viên") + " <select id=\"" + Extensions.Constants.ActorId + "\" name=\"" + Extensions.Constants.ActorId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var actor in result)
            {
                if (!string.IsNullOrEmpty(actorSelected) && actor.Value == actorSelected)
                {
                    sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", actor.Text, actor.Value);
                    continue;
                }

                sb.AppendFormat("<option value=\"{1}\">{0}</option>", actor.Text, actor.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        public virtual string BuildFilmTypes()
        {
            var service = WorkContext.Resolve<IFilmTypesService>();
            var items = service.GetRecords(x => x.Status == (int)Status.Approved);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem { Text = "--- Thể loại phim ---", Value = "0" });

            var filmTypesSelected = GetQueryString(6);
            var sb = new StringBuilder();
            sb.AppendFormat(T("Thể loại phim") + " <select id=\"" + Extensions.Constants.FilmTypesId + "\" name=\"" + Extensions.Constants.FilmTypesId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(filmTypesSelected) && item.Value == filmTypesSelected)
                {
                    sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", item.Text, item.Value);
                    continue;
                }

                sb.AppendFormat("<option value=\"{1}\">{0}</option>", item.Text, item.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        public virtual string BuildFilmGroups()
        {
            var items = EnumExtensions.GetListItems<FilmGroup>();
            items.Insert(0, new SelectListItem { Text = "--- Nhóm phim ---", Value = "0" });

            var filmGroupSelected = GetQueryString(7);
            var sb = new StringBuilder();
            sb.AppendFormat(T("Nhóm phim") + " <select id=\"" + Extensions.Constants.FilmGroup + "\" name=\"" + Extensions.Constants.FilmGroup + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var group in items)
            {
                if (!string.IsNullOrEmpty(filmGroupSelected) && group.Value == filmGroupSelected)
                {
                    sb.AppendFormat("<option selected value=\"{1}\">{0}</option>", group.Text, group.Value);
                    continue;
                }
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", group.Text, group.Value);
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        public virtual IEnumerable<SelectListItem> BindStatus()
        {
            var items = EnumExtensions.GetListItems<Status>();
            items[0].Selected = true;
            return items;
        }

        public virtual IEnumerable<SelectListItem> BindSex()
        {
            var items = EnumExtensions.GetListItems<Sex>();
            items[0].Selected = true;
            return items;
        }

        public virtual IEnumerable<SelectListItem> BindCities()
        {
            var cityService = WorkContext.Resolve<ICityService>();
            var items = cityService.GetRecords(x => x.Status == (int)Status.Approved).ToList();
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem { Text = "--- Chọn Quận huyện/Thành phố ---", Value = "0" });
            return result;
        }

        [Url("admin/video-files/get-root-folders")]
        public virtual ActionResult GetRootFolders()
        {
            var serverId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ServerId]))
            {
                serverId = Convert.ToInt32(Request.Form[Extensions.Constants.ServerId]);
            }

            var service = WorkContext.Resolve<IFilmServersService>();
            var items = service.GetRecords(x => x.Id == serverId);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.FolderRoot,
                Value = item.FolderRoot
            }));
            result.Insert(0, new SelectListItem { Text = "--- Tất cả ---", Value = Extensions.Constants.ValueAll });

            return Json(result);
        }

        [Url("admin/get-sites-by-language")]
        public ActionResult GetSitesByLanguage()
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var service = WorkContext.Resolve<ISiteService>();
            var items = service.GetRecords(x => x.LanguageCode == languageCode);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem { Text = "--- Chọn trang web ---", Value = "0" });
           
            return Json(result);
        }

        [Url("admin/category/get-servers-by-site")]
        public ActionResult GetServersBySite()
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var siteId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId]))
            {
                siteId = Convert.ToInt32(Request.Form[Extensions.Constants.SiteId]);
            }

            var service = WorkContext.Resolve<IFilmServersService>();
            var items = service.GetRecords(x => x.LanguageCode == languageCode && x.SiteId == siteId);
            var result = new List<SelectListItem>();

            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.ServerName,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem { Text = T("--- Chọn máy chủ ---"), Value = "0" });

            return Json(result);
        }

        [Url("admin/vast/get-ads-by-site")]
        public ActionResult GetAdsBySite()
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var siteId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId]))
            {
                siteId = Convert.ToInt32(Request.Form[Extensions.Constants.SiteId]);
            }

            var service = WorkContext.Resolve<IAdvertisementService>();
            var items = service.GetRecords(x => x.LanguageCode == languageCode && x.SiteId == siteId);
            var result = new List<SelectListItem>();

            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem { Text = T("--- Chọn quảng cáo ---"), Value = "0" });

            return Json(result);
        }

        [Url("admin/category/get-categories-by-site")]
        public ActionResult GetCategoriesBySite()
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }
            
            var siteId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId]))
            {
                siteId = Convert.ToInt32(Request.Form[Extensions.Constants.SiteId]);
            }

            var service = WorkContext.Resolve<ICategoryService>();
            service.LanguageCode = languageCode;
            service.SiteId = siteId;
            var items = service.GetTree();
            var result = new List<SelectListItem>();

            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.ChildenName,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem { Text = T("--- Không chọn ---"), Value = "0" });

            return Json(result);
        }

        [Url("admin/reset-cache")]
        public ActionResult ResetCache()
        {
            var serviceLanguage = WorkContext.Resolve<ILanguageService>();
            var items = serviceLanguage.GetRecords(x => x.Theme == Constants.ThemeDefault);
            if (items != null && items.Count > 0)
            {
                var categoryService = WorkContext.Resolve<ICategoryService>();
                foreach (var language in items)
                {
                    try
                    {
                        categoryService.LanguageCode = language.CultureCode;
                        categoryService.SiteId = 1;
                        categoryService.ResetCache();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                foreach (var language in items)
                {
                    try
                    {
                        categoryService.LanguageCode = language.CultureCode;
                        categoryService.SiteId = 2;
                        categoryService.ResetCache();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            #region Film

            try
            {
                var serviceFilms = WorkContext.Resolve<IFilmService>();
                serviceFilms.LanguageCode = WorkContext.CurrentCulture;
                serviceFilms.SiteId = (int)Site.Home;
                serviceFilms.ResetCache();
                serviceFilms.RefreshFilmJJChannelIntroduce(0);
                serviceFilms.RefreshTVShows(0);
                serviceFilms.RefreshClips(5);
                serviceFilms.RefreshStatistical1(30, 1);
                serviceFilms.RefreshStatistical1(30, 2);
                serviceFilms.RefreshStatistical1(30, 3);

                serviceFilms.RefreshStatistical2(30, 1);
                serviceFilms.RefreshStatistical2(30, 2);
                serviceFilms.RefreshStatistical2(30, 3);

                serviceFilms.RefreshStatistical3(30, 1);
                serviceFilms.RefreshStatistical3(30, 2);
                serviceFilms.RefreshStatistical3(30, 3);

                serviceFilms.RefreshStatistical4(30, 1);
                serviceFilms.RefreshStatistical4(30, 2);
                serviceFilms.RefreshStatistical4(30, 3);

                serviceFilms.RefreshStatistical5(30, 1);
            }
            catch (Exception)
            {

            }
            #endregion

            #region Articles

            try
            {
                var serviceArticles = WorkContext.Resolve<IArticlesService>();
                serviceArticles.LanguageCode = WorkContext.CurrentCulture;
                serviceArticles.SiteId = 1;
                serviceArticles.CategoryId = 0;
                serviceArticles.ResetCache();
                serviceArticles.CategoryId = 60;
                serviceArticles.ResetCache();
                serviceArticles.CategoryId = 61;
                serviceArticles.ResetCache();
                serviceArticles.CategoryId = 62;
                serviceArticles.ResetCache();
            }
            catch (Exception)
            {

            }
            #endregion

            #region Search
            try
            {
                var service = WorkContext.Resolve<ISearchService>();
                service.SearchRestore(1);
                service.SiteId = (int)Site.Home;
                service.ResetCache();
            }
            catch (Exception)
            {

            }
            #endregion

            #region Slider
            var sliderService = WorkContext.Resolve<ISliderService>();
            sliderService.LanguageCode = WorkContext.CurrentCulture;
            sliderService.SiteId = (int) Site.Home;
            var list = EnumExtensions.GetListItems<SliderPages>();
            foreach (var page in list)
            {
                try
                {
                    sliderService.RefreshByPage(int.Parse(page.Value));
                }
                catch (Exception)
                {

                }
            }
            #endregion

            #region Customers
            var customerService = WorkContext.Resolve<ICustomerService>();
            customerService.ResetCacheCustomers();
            #endregion

            return Redirect(Url.Action("Index", "Admin"));
        }
    }
}