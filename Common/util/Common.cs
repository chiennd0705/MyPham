﻿using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Common.util
{
    public static class Common
    {
        /* public static string InitVoucherSerial(string index)
         {
             int len = index.Length;
             if (len == 1) return string.Format("0000{0}", index);
             if (len == 2) return string.Format("000{0}", index);
             if (len == 3) return string.Format("00{0}", index);
             if (len == 4) return string.Format("0{0}", index);
             return string.Format("{0}", index);
         }
         * */

        /// <summary>
        /// Kiểu tài khoản
        /// BOCO  = 0 : Riêng bo, co
        /// BO    = 1 : Gộp chung về bo
        /// </summary>

        public enum ACCOUNT_TYPE
        {
            BOCO,
            BO
        }

        /// <summary>
        /// Trạng thái thẻ
        /// NOACTIVE = 0 : Chưa active
        /// ACTIVE   = 1 : Đã active
        /// LOCK     = 2 : Đang bị khóa
        /// REPLACED = 3 : Đã được thay thế bằng thẻ khác
        /// </summary>

        public enum CARD_STATUS
        {
            NOACTIVE,
            ACTIVE,
            LOCK,
            REPLACED
        }

        /// <summary>
        /// Trạng thái trả về khi check Card
        /// NOACTIVE  =  0 : Chưa active
        /// ACTIVE    =  1 : Đang active
        /// LOCK      =  2 : Đang active
        /// REPLACED =  3  : Đã thay thế thành thẻ khác
        /// ERR_MONEY =  4 : Không đủ tiền thanh toán
        /// OK    =  5 : Ok, có thể thanh toán cho hóa đơn này
        /// </summary>

        public enum CARD_STATUS_RETURN
        {
            NOACTIVE,
            ACTIVE,
            LOCK,
            REPLACED,
            ERR_MONEY,
            OK
        }

        /// <summary>
        /// Trạng thái Voucher
        /// NOACTIVE  =  0 : Chưa active
        /// ACTIVE    =  1 : Đang active
        /// LOCK      =  2 : Đang active
        /// REDEEM_BO =  3 : Đã dùng BO
        /// REDEEM_CO =  4 : Đã dùng CO
        /// REDEEM    =  5 : Đã dùng hết
        /// </summary>

        public enum VOUCHER_STATUS
        {
            NOACTIVE,
            ACTIVE,
            LOCK,
            REDEEM_BO,
            REDEEM_CO,
            REDEEM
        }

        /// <summary>
        /// Voucher Áp dụng cho BO, CO, hay Cả 2
        /// BOCO  = 0 : Áp dụng cho cả 2
        /// BO    = 1 : Áp dụng cho BO
        /// CO    = 2 : Áp dụng cho CO
        /// </summary>

        public enum VOUCHER_APPLIED_BOCO
        {
            BOCO,
            BO,
            CO
        }

        /// <summary>
        /// Voucher Áp dụng cho BO, CO, hay Cả 2
        /// BOCO  = 0 : chưa Active
        /// ACTIVE    = 1 : Đang Active
        /// LOCK    = 2 : Đang khóa
        /// </summary>

        public enum USER_STATUS
        {
            NOACTIVE,
            ACTIVE,
            LOCK
        }

        static public string GetMd5Sum(string str)
        {
            // First we need to convert the string into bytes, which
            // means using a text encoder.
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

            // Create a buffer large enough to hold the string
            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

            // Now that we have a byte array we can ask the CSP to hash it
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            // Build the final string by converting each byte
            // into hex and appending it to a StringBuilder
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            // And return it
            return sb.ToString();
        }

        static public string GetHostName(string remote_addr)
        {
            try
            {
                IPHostEntry iPHostEntry = Dns.GetHostEntry(remote_addr);
                return iPHostEntry.HostName;
            }
            catch
            {
                return string.Empty;
            }
        }

        static public string IPAdress(string remote_addr)
        {
            //IPHostEntry iPHostEntry = Dns.GetHostEntry(remote_addr);
            //string iPAdress = iPHostEntry.AddressList.Length < 2 ? iPHostEntry.AddressList.First().ToString() : iPHostEntry.AddressList.Last().ToString();
            //return iPAdress;

            return "";
        }

        static public XmlDocument GetDirectLink(string strURL)
        {
            HttpWebRequest myHttpWebRequest = null;     //Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebResponse myHttpWebResponse = null;   //Declare an HTTP-specific implementation of the WebResponse class
            XmlDocument myXMLDocument = null;           //Declare XMLResponse document
            XmlTextReader myXMLReader = null;           //Declare XMLReader

            try
            {
                //Create Request
                myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(strURL);
                myHttpWebRequest.Method = "GET";
                myHttpWebRequest.ContentType = "text/xml; encoding='utf-8'";

                //Get Response
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                //Now load the XML Document
                myXMLDocument = new XmlDocument();

                //Load response stream into XMLReader
                myXMLReader = new XmlTextReader(myHttpWebResponse.GetResponseStream());
                myXMLDocument.Load(myXMLReader);
                return myXMLDocument;
            }
            catch
            {
                return null;
            }
        }
    }
}