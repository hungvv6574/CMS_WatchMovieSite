using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.VipHDCardServices;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;

namespace CMSSolutions.Websites.Payments
{
    public class APICardCharging
    {
        public APICardCharging()
        {
            Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
            TransactionId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TransactionId"]);
            MPin = System.Configuration.ConfigurationManager.AppSettings["MPin"];
            CardList = System.Configuration.ConfigurationManager.AppSettings["CardList"];
            CardData = System.Configuration.ConfigurationManager.AppSettings["CardData"];
        }

        private string UserName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["UserName"]; }
        }

        private string Password { get; set; }

        private string PartnerId
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["CardPartnerId"]; }
        }

        private int TransactionId { get; set; }

        private string MPin { get; set; }

        public string CardList { get; set; }

        public string CardData { get; set; }

        public string EncryptDataRSA(string origData, string publicKey)
        {
            byte[] plaintext = Encoding.ASCII.GetBytes(origData);
            byte[] bKey = JavaScience.opensslkey.DecodePkcs8EncPrivateKey(publicKey);
            RSACryptoServiceProvider myrsa = JavaScience.opensslkey.DecodeX509PublicKey(bKey);
            byte[] encrypt = myrsa.Encrypt(plaintext, false);
            string enStr = Convert.ToBase64String(encrypt);
            return enStr;
        }

        public string DecryptDataRSA(string deData, string privateKey)
        {
            try
            {
                byte[] Databyte = Convert.FromBase64String(deData);
                byte[] privatebKEY = JavaScience.opensslkey.DecodeOpenSSLPrivateKey(privateKey);
                RSACryptoServiceProvider rsa = JavaScience.opensslkey.DecodeRSAPrivateKey(privatebKEY);
                string dataDecript = Encoding.ASCII.GetString(rsa.Decrypt(Databyte, false));
                return dataDecript;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string EncryptDataTripdesPKCS7(string origData, string key)
        {
            byte[] tripkey = Encrypt.hexa.hexatobyte(key);
            byte[] entrip = TripleDES_Encrypt_Byte(tripkey, Encoding.UTF8.GetBytes(origData));
            return ByteArrayToHexString(entrip);
        }

        public string EncryptDataTripdes(string origData, string key)
        {
            byte[] tripkey = Encrypt.hexa.hexatobyte(key);
            string erro = "";
            string entrip = Encrypt.Encrypt._EncryptTripleDes(origData, tripkey, ref erro);

            return entrip;
        }

        public string DecryptDataTripdes(string deData, string key)
        {
            byte[] tripkey = Encrypt.hexa.hexatobyte(key);
            string error = "";
            string detrip = Encrypt.Encrypt._DecryptTripleDes(deData, tripkey, ref error);
            return detrip;
        }

        private String GetLocal()
        {
            String local = System.Web.HttpContext.Current.Request.MapPath("~");
            return local;
        }

        public string Login(out string tokenKey)
        {
            try
            {
                var keyDir = GetLocal() + "\\RsaKeys\\";    
                var websv = new ServicesService();
                var loginRes = new LoginResponse();
                string publicKey = keyDir + "epay_public_key.pem";
                string encryptPass = Encrypt2(publicKey, Password);
                loginRes = websv.login(UserName, encryptPass, PartnerId);
                if ((ErrorMessages)Convert.ToInt32(loginRes.status) == ErrorMessages.Success)
                {
                    string privateKey = keyDir + "private_key.pem";
                    tokenKey = Decrypt2(privateKey, loginRes.sessionid);
                    return Utilities.GetMessages(Convert.ToInt32(loginRes.status));
                }

                tokenKey = string.Empty;
                return Utilities.GetMessages(Convert.ToInt32(loginRes.status));
            }
            catch (Exception ex)
            {
                tokenKey = string.Empty;
                return ex.Message;
            }
        }

        public string Logout(string sessionid)
        {
            var websv = new ServicesService();
            string md5hex2str = HashWithMD5(sessionid);
            byte[] cvmd52 = Convert.FromBase64String(md5hex2str);
            string md5sess = ByteArrayToHexString(cvmd52);
            var logoutres = new LogoutResponse();
            logoutres = websv.logout(UserName, PartnerId, md5sess);

            var message = Utilities.GetMessages(Convert.ToInt32(logoutres.status));
            return message + "-" + message;
        }

        public string ChangeMpin(string sessionid, string newPassword)
        {
            string datetime = GetDateTime();
            string strTransID = PartnerId.Trim() + "_" + UserName.Trim() + "_" + datetime.Trim() + "_" + TransactionId;
            TransactionId++;
            if (newPassword.Length == 0)
            {
                return "Mật khẩu nhập không đúng.";
            }

            if (newPassword.Equals(MPin))
            {
                return "Mật khẩu trùng với mật khẩu cũ.";
            }

            string ensess = HashWithMD5(sessionid);
            byte[] cvmd52 = Convert.FromBase64String(ensess);
            string md5sess = ByteArrayToHexString(cvmd52);
            var websv = new ServicesService();
            string enoldpin = EncryptDataTripdes(MPin, sessionid);
            string ennewpin = EncryptDataTripdes(newPassword, sessionid);
            var changeRes = new ChangeResponse();
            changeRes = websv.changeMPIN(strTransID, UserName, PartnerId, enoldpin, ennewpin, md5sess);
            var message = Utilities.GetMessages(Convert.ToInt32(changeRes.status));
            if ((ErrorMessages)Convert.ToInt32(changeRes.status) == ErrorMessages.Success)
            {
                MPin = newPassword;
            }

            return message;
        }

        public string GetDateTime()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public string ChangePassword(string sessionid, string newPassword)
        {
            string datetime = GetDateTime();
            string strTransID = PartnerId.Trim() + "_" + UserName.Trim() + "_" + datetime.Trim() + "_" + TransactionId;
            TransactionId++;
            if (newPassword.Length == 0)
            {
                return "Mật khẩu nhập không đúng";
            }
            if (newPassword.Equals(Password))
            {
                return "Mật khẩu trùng với mật khẩu cũ.";
            }

            string ensess = HashWithMD5(sessionid);
            byte[] cvmd52 = Convert.FromBase64String(ensess);
            string md5sess = ByteArrayToHexString(cvmd52);
            var websv = new ServicesService();
            string enoldpass = EncryptDataTripdes(Password, sessionid);
            string ennewpass = EncryptDataTripdes(newPassword, sessionid);
            var changeRes = new ChangeResponse();
            changeRes = websv.changePassword(strTransID, UserName, PartnerId, enoldpass, ennewpass, md5sess);
            var message = Utilities.GetMessages(Convert.ToInt32(changeRes.status));
            if ((ErrorMessages)Convert.ToInt32(changeRes.status) == ErrorMessages.Success)
            {
                Password = newPassword;
            }

            return message;
        }

        public string CheckTrain(string sessionid, string tranid)
        {
            if (string.IsNullOrEmpty(tranid))
            {
                return "Mã không để trống.";
            }

            string ensess = HashWithMD5(sessionid);
            byte[] cvmd52 = Convert.FromBase64String(ensess);
            string md5sess = ByteArrayToHexString(cvmd52);
            var websv = new ServicesService();
            var chargres = new ChargeReponse();
            chargres = websv.getTransactionStatus(tranid, UserName, PartnerId, md5sess);

            return Utilities.GetMessages(Convert.ToInt32(chargres.status));

        }

        public string CardCharging(string sessionid, int seed, string carddata, string transID, string target, out string amount)
        {
            string mapin = EncryptDataTripdesPKCS7(MPin, sessionid);
            string enCard = EncryptDataTripdesPKCS7(carddata, sessionid);
            string md5hex2str = HashWithMD5(sessionid);
            byte[] cvmd52 = Convert.FromBase64String(md5hex2str);
            string md5sess = ByteArrayToHexString(cvmd52);
            var chargeRes = new ChargeReponse();
            var message = string.Empty;
            amount = "0";
            try
            {
                var websv = new ServicesService();
                chargeRes = websv.cardCharging(transID, UserName, PartnerId, mapin, target, enCard, md5sess);
                if ((ErrorMessages)Convert.ToInt32(chargeRes.status) == ErrorMessages.Success)
                {
                    message = Utilities.GetMessages((int)ErrorMessages.Success);
                    amount = DecryptDataTripdesPKCS7(chargeRes.responseamount, sessionid);
                }
                else
                {
                    message = Utilities.GetMessages(Convert.ToInt32(chargeRes.status));
                }
            }
            catch
            {
                message = Utilities.GetMessages((int)ErrorMessages.SystemBusy);
            }

            return message;
        }

        public ErrorMessages CardCharging(string sessionid, string carddata, string target, out string amount)
        {
            string datetime = GetDateTime();
            var ran = new Random();
            string kqran = ran.Next(10000, 99999).ToString();
            string strTransID = System.Configuration.ConfigurationManager.AppSettings["TransactionId"] + "_" + datetime + "_" + kqran;
            string mapin = EncryptDataTripdesPKCS7(MPin, sessionid);
            string enCard = EncryptDataTripdesPKCS7(carddata, sessionid);
            string md5hex2str = HashWithMD5(sessionid);
            byte[] cvmd52 = Convert.FromBase64String(md5hex2str);
            string md5sess = ByteArrayToHexString(cvmd52);
            var chargeRes = new ChargeReponse();
            amount = "0";
            try
            {
                var websv = new ServicesService();
                chargeRes = websv.cardCharging(strTransID, UserName, PartnerId, mapin, target, enCard, md5sess);
                var status = (ErrorMessages) Convert.ToInt32(chargeRes.status);
                if (status == ErrorMessages.Success)
                {
                    amount = DecryptDataTripdesPKCS7(chargeRes.responseamount, sessionid);
                }

                return status;
            }
            catch
            {
                return ErrorMessages.Fail;
            }
        }

        public static string HashWithMD5(string text)
        {
            var hashAlgorithm = new MD5CryptoServiceProvider();
            return HashString(text, hashAlgorithm);
        }

        public static string HashString(string stringToHash, HashAlgorithm algo)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(stringToHash);
            bytes = algo.ComputeHash(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string ByteArrayToHexString(byte[] Bytes)
        {
            var Result = new StringBuilder();
            string HexAlphabet = "0123456789ABCDEF";
            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);
            }

            return Result.ToString();
        }

        public string DecryptDataTripdesPKCS7(string origData, string key)
        {
            byte[] tripkey = Encrypt.hexa.hexatobyte(key);
            string erro = "";
            byte[] entrip = TripleDES_Decrypt_Byte(tripkey, Encrypt.hexa.hexatobyte(origData));//Encrypt.Encrypt._EncryptTripleDes(origData, tripkey, ref erro);
            return Encoding.UTF8.GetString(entrip);
        }

        private byte[] TripleDES_Encrypt_Byte(byte[] Keys, byte[] clearText)
        {
            try
            {
                byte[] IVs = new byte[8];
                var des = new TripleDESCryptoServiceProvider
                {
                    IV = IVs, 
                    KeySize = 192, 
                    Key = Keys,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                byte[] cipherText = des.CreateEncryptor().TransformFinalBlock(clearText, 0, clearText.Length);
                return cipherText;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private byte[] TripleDES_Decrypt_Byte(byte[] Keys, byte[] clearText)
        {
            try
            {
                byte[] IVs = new byte[8];
                var des = new TripleDESCryptoServiceProvider();
                des.IV = IVs;
                des.KeySize = 192;             
                des.Key = Keys;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                byte[] cipherText = des.CreateDecryptor().TransformFinalBlock(clearText, 0, clearText.Length);
                return cipherText;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AsymmetricCipherKeyPair GetPrivateKey(string privateKey)
        {
            try
            {
                var sr = new StreamReader(privateKey);
                var pr = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                var KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
                sr.Close();
                return KeyPair;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception:", ex);
            }
        }

        public AsymmetricKeyParameter GetPublicKey(string publicKey)
        {
            try
            {
                var sr = new StreamReader(publicKey);
                var pr = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                var KeyPair = (AsymmetricKeyParameter)pr.ReadObject();
                sr.Close();
                return KeyPair;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception:", ex);
            }
        }

        public string Decrypt2(string privateKeyFileName, string encryptString)
        {
            try
            {
                AsymmetricCipherKeyPair keyPair = GetPrivateKey(privateKeyFileName);
                IAsymmetricBlockCipher cipher = new RsaEngine();
                cipher.Init(false, keyPair.Private);
                byte[] encryptByte = Convert.FromBase64String(encryptString);
                byte[] cipheredBytes = cipher.ProcessBlock(encryptByte, 0, encryptByte.Length);
                String decryptString = Encoding.UTF8.GetString(cipheredBytes);
                return decryptString;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception encrypting file:", ex);
            }
        }

        public string Encrypt2(string publicKeyFileName, string inputMessage)
        {
            try
            {
                AsymmetricKeyParameter keyPair = GetPublicKey(publicKeyFileName);
                var utf8enc = new UTF8Encoding();
                byte[] inputBytes = utf8enc.GetBytes(inputMessage);
                AsymmetricKeyParameter publicKey = keyPair;
                IAsymmetricBlockCipher cipher = new RsaEngine();
                cipher.Init(true, publicKey);
                byte[] cipheredBytes = cipher.ProcessBlock(inputBytes, 0, inputMessage.Length);
                String encrypt = Convert.ToBase64String(cipheredBytes);
                return encrypt;
            }
            catch (Exception ex)
            {
                throw new Exception("Encrypt string fail:", ex);
            }
        }
    }
}