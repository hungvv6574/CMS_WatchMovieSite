using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Payments
{
    public class APISms
    {
        public string PartnerId = ConfigurationManager.AppSettings["PartnerId"];
        public string Password = ConfigurationManager.AppSettings["PartnerPassword"];
        public string UrlSMSMT = ConfigurationManager.AppSettings["UrlSMSMT"];

        public string Md5(string source_str)
        {
            MD5 encrypter = new MD5CryptoServiceProvider();
            Byte[] original_bytes = ASCIIEncoding.Default.GetBytes(source_str);
            Byte[] encoded_bytes = encrypter.ComputeHash(original_bytes);
            return BitConverter.ToString(encoded_bytes).Replace("-", "").ToLower();
        }

        public int SentMT(string moid, string shortcode, 
            string keyword, string mobinumber, string contentmt, ref string url_)
        {
            url_ = CreatUrlMT(moid, shortcode, keyword, mobinumber, HttpUtility.UrlEncode(contentmt));
            LogFiles.WriteLogSms(string.Format("MT URL: {0}", url_));
            return SendRequest(url_);

        }

        private string CreatUrlMT(string moid, 
            string shortcode, string keyword, string mobinumber, string contentmt)
        {
            var mtid = CreateMTId();
            var transdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            string and = "&";
            string url_ = UrlSMSMT;
            url_ += "partnerid=" + PartnerId + and;
            url_ += "moid=" + moid + and;
            url_ += "mtid=" + mtid + and;
            url_ += "userid=" + mobinumber + and;
            url_ += "shortcode=" + shortcode + and;
            url_ += "keyword=" + keyword + and;
            url_ += "content=" + contentmt + and;
            url_ += "messagetype=1" + and;
            url_ += "totalmessage=1" + and;
            url_ += "messageindex=1" + and;
            url_ += "ismore=0" + and;
            url_ += "contenttype=0" + and;
            url_ += "transdate=" + transdate + and;
            string checkSum = CreateCheckSumMT(transdate, contentmt, keyword, shortcode, moid, mtid);
            url_ += "checksum=" + checkSum;

            return url_;
        }

        public string CreateMTId()
        {
            var ran = new Random();
            string kqran = ran.Next(0, 99999).ToString();
            return PartnerId + DateTime.Now.ToString("yyyyMMddHHmmss") + kqran;
        }

        public string CreateCheckSumMT(string transdate, string Content, 
            string Keyword, string ShortCode, string MoId, string mtId)
        {
            return Md5(mtId + MoId + ShortCode + Keyword + Content + transdate + Password);
        }

        public int SendRequest(string url)
        {
            try
            {
                var myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "GET";
                WebResponse myResponse = myRequest.GetResponse();
                var sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();
                string[] s = result.Split('=');
                LogFiles.WriteLogSms(string.Format("MT: RESULT {0}", result));
                if (result == "requeststatus=200")
                {
                    return 200;
                }

                if (result == "requeststatus=17")
                {
                    return 17;
                }
                LogFiles.WriteLogSms(string.Format("MT: SEND {0}", result));
                return int.Parse(s[1]);

            }
            catch (Exception ex)
            {
                LogFiles.WriteLogSms(string.Format("MT: SEND ERROR {0}", ex.InnerException.Message));
                throw ex;
            }
        }
    }
}