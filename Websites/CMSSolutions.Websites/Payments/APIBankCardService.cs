namespace CMSSolutions.Websites.Payments
{
    public class APIBankCardService
    {
        public string SenderKey
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SenderKey"]; }
        }

        public string ReceiverKey
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ReceiverKey"]; }
        }

        public string MerchantId = "88000072";
        public string ReponseUrl { get; set; }
    }
}