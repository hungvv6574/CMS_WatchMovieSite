using System.Collections.Generic;
using System.Web.Mvc;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class DataViewCategoryModel
    {
        public int Type { get; set; }

        public string Title { get; set; }

        public string TextNext { get; set; }

        public string UrlNext { get; set; }

        public string SliderName { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

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

        public string HtmlData { get; set; }

        public string HtmlFilmHot { get; set; }

        public string HtmlFilmRetail { get; set; }

        public string HtmlFilmLengthEpisodes { get; set; }

        public string HtmlFilmJJChannelIntroduce { get; set; }

        public string HtmlFilmTheater { get; set; }

        public string HtmlTVShow { get; set; }

        public string HtmlClip { get; set; }

        public string Data { get; set; }

        public string Breadcrumb { get; set; }

        public CategoryInfo CurrentCategory { get; set; }

        public List<FilmTypesInfo> ListFilmTypes { get; set; }

        public int SelectedFilmTypes { get; set; }

        public List<CountryInfo> ListCountries { get; set; }

        public int SelectedCountry { get; set; }

        public List<SelectListItem> SearchOrderBy { get; set; }

        public string SelectedOrderBy { get; set; }

        public int SelectedSortBy { get; set; }

        public IList<FilmInfo> ListFilms { get; set; }

        public ArticlesInfo News { get; set; }

        public IList<SupportInfo> ListSupportParents { get; set; }

        public IList<SupportInfo> ListSupportChildren { get; set; }

        public IList<ArticlesInfo> ListArticles { get; set; }

        public string Keyword { get; set; }

        public IList<SearchInfo> ListSearchFilms { get; set; }

        public IList<SearchInfo> ListSearchClip { get; set; }

        public IList<SearchInfo> ListSearchShow { get; set; }

        public IList<SearchInfo> ListSearchTrailer { get; set; }

        public FilmInfo FilmDetails { get; set; }
    }
}