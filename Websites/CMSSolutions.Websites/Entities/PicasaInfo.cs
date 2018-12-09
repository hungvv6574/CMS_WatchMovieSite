using System;

namespace CMSSolutions.Websites.Entities
{
    public class PicasaInfo
    {
        public string Medium { get; set; }

        public string Url { get; set; }

        public string Type { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string EncodeUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Url))
                {
                    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Url));
                }

                return string.Empty;
            }
        }
    }
}