using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Models
{
    public class DataViewerModel
    {
        public string CustomerCode { get; set; }

        public int DataType { get; set; }

        public int TotalRow { get; set; }

        public int TotalPage
        {
            get
            {
                if (TotalRow <= PageSize)
                {
                    return 1;
                }

                var count = TotalRow % PageSize;
                if ((count == 0))
                {
                    return TotalRow / PageSize;
                }

                return ((TotalRow - count) / PageSize) + 1;
            }
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string Title { get; set; }

        public string SliderName { get; set; }

        public int SiteId { get; set; }

        public bool IsShow { get; set; }

        public bool Status { get; set; }

        public string Data { get; set; }

        public string Url { get; set; }

        public CustomerInfo Customer { get; set; }

        public List<CityInfo> ListCities { get; set; }

        public List<FilmTypesInfo> ListFilmTypes { get; set; }

        public List<CountryInfo> ListCountries { get; set; }

        public List<SelectListItem> ListDay { get; set; }

        public List<SelectListItem> ListMonth { get; set; }

        public List<SelectListItem> ListYear { get; set; }

        public FilmInfo FilmDetails { get; set; }

        public string JwplayerKey { get; set; }

        public int EpisodeId { get; set; }

        public List<PicasaInfo> ListFilmsPicasa { get; set; }

        public AdvertisementGroupInfo Advertisement { get; set; }

        public string UrlSwitch 
        { 
            get
            {
                var url = string.Empty;
                switch (DataType)
                {
                    case (int)LinkType.Streaming:
                        url = FilmDetails.EncodeStreamingUrl;
                        break;
                    case (int)LinkType.Picasa:
                        if (ListFilmsPicasa != null && ListFilmsPicasa.Count > 0)
                        {
                            url = ListFilmsPicasa.FirstOrDefault().EncodeUrl;
                            if (ListFilmsPicasa.Count > 1)
                            {
                                url = ListFilmsPicasa[1].EncodeUrl;
                            }
                        }
                        break;
                    case (int)LinkType.Youtube:
                        url = FilmDetails.EncodeSourceUrl;
                        break;
                }

                return url;
            } 
        }

        public string Skin { get; set; }

        public string AdvertisementsPath { get; set; }

        public List<DownloadGameInfo> ListGames { get; set; }

        public List<DownloadCustomerInfo> ListDownloadCustomer { get; set; }

        public DataTable ListHistoryDownload { get; set; }

        public DownloadCustomerInfo GetByGame(int id)
        {
            if (ListDownloadCustomer != null)
            {
                return ListDownloadCustomer.FirstOrDefault(x => x.DownloadId == id);
            }

            return null;
        }
    }
}