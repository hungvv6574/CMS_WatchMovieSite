using System.Xml.Serialization;

namespace CMSSolutions.Websites.Models
{
    #region ads
    [XmlRoot("VAST")]
    public class Vast
    {
        [XmlElement("Ad")]
        public Ads ItemAds { get; set; }
    }

    [XmlRoot("Ad")]
    public class Ads
    {
        [XmlAttributeAttribute("id")]
        public int Id { get; set; }

        [XmlElement("InLine")]
        public InLine ObjectInLine { get; set; }
    }

    [XmlRoot("InLine")]
    public class InLine
    {
        [XmlElement("AdSystem")]
        public AdSystem ObjectAdSystem { get; set; }

        [XmlElement("AdTitle")]
        public string AdTitle { get; set; }

        [XmlElement("Error")]
        public string Error { get; set; }

        [XmlElement("Impression")]
        public string Impression { get; set; }

        [XmlElement("Creatives")]
        public Creatives Creatives { get; set; }
    }

    [XmlRoot("AdSystem")]
    public class AdSystem
    {
        [XmlAttributeAttribute("version")]
        public string Version { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot("Creatives")]
    public class Creatives
    {
        [XmlElement("Creative")]
        public Creative[] ListCreatives { get; set; }
    }

    [XmlRoot("Creative")]
    public class Creative
    {
        [XmlElement("Linear")]
        public Linear ObjectLinear { get; set; }
    }

    [XmlRoot("Linear")]
    public class Linear
    {
        [XmlAttributeAttribute("skipoffset")]
        public string Skipoffset { get; set; }

        [XmlElement("Duration")]
        public string Duration { get; set; }

        [XmlElement("VideoClicks")]
        public VideoClicks VideoClicks { get; set; }

        [XmlElement("TrackingEvents")]
        public TrackingEvents TrackingEvents { get; set; }

        [XmlElement("MediaFiles")]
        public MediaFiles MediaFiles { get; set; }
    }

    [XmlRoot("VideoClicks")]
    public class VideoClicks
    {
        [XmlElement("ClickThrough")]
        public string ClickThrough { get; set; }
    }

    public class TrackingEvents
    {
        [XmlElement("Tracking")]
        public Tracking[] LisTrackings { get; set; }
    }

    [XmlRoot("Tracking")]
    public class Tracking
    {
        [XmlAttributeAttribute("event")]
        public string Event { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot("MediaFiles")]
    public class MediaFiles
    {
        [XmlElement("MediaFile")]
        public MediaFile[] ListMediaFiles { get; set; }
    }

    [XmlRoot("MediaFile")]
    public class MediaFile
    {
        [XmlAttributeAttribute("bitrate")]
        public int Bitrate { get; set; }

        [XmlAttributeAttribute("delivery")]
        public string Delivery { get; set; }

        [XmlAttributeAttribute("height")]
        public int Height { get; set; }

        [XmlAttributeAttribute("width")]
        public int Width { get; set; }

        [XmlAttributeAttribute("maintainAspectRatio")]
        public bool MaintainAspectRatio { get; set; }

        [XmlAttributeAttribute("scalable")]
        public bool Scalable { get; set; }

        [XmlAttributeAttribute("type")]
        public string Type { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
    #endregion

    #region vplugin xml
    [XmlRoot("Ad")]
    public class Ad
    {
        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("type")]
        public string Type { get; set; }

        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("click")]
        public string Click { get; set; }

        [XmlElement("duration")]
        public int Duration { get; set; }

        [XmlElement("position")]
        public int Position { get; set; }

        [XmlElement("skip")]
        public int Skip { get; set; }
    }

    [XmlRoot("vplugin")]
    public class Vplugin
    {
        [XmlElement("Ad")]
        public Ad[] ListAds { get; set; }
    }
    #endregion
}