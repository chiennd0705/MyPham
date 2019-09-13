using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Common.util
{
    public class Function
    {
        public static List<Catalog> ListCate = null;

        public static DateTime ConvertDateTime(string str)
        {
            DateTime dt = new DateTime();
            if (!string.IsNullOrEmpty(str))
            {
                var time = str.Split('/');
                string date = time[1] + "/" + time[0] + "/" + time[2];
                return DateTime.Parse(date);
            }
            else
            {
                return dt;
            }
        }

        //public static string GetLinkCdn()
        //{
        //}
        public static string GetSex(int id)
        {
            if (id == 1)
            {
                return "Nam";
            }
            else
            {
                return "Nữ";
            }
        }

        public static string FomatDate(DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy");
        }

        public static DateTime ConvertDate(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return DateTime.Parse(str);
            }
            else
            {
                return new DateTime();
            }
        }

        public static string FomatNumber(double number)
        {
            return number.ToString("N0", CultureInfo.InvariantCulture);
        }

        public static byte[] Upload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                using (Bitmap origImage = new Bitmap(file.InputStream))
                {
                    int maxWidth = 165;

                    int newWidth = origImage.Width;
                    int newHeight = origImage.Height;
                    if (origImage.Width < newWidth) //Force to max width
                    {
                        newWidth = maxWidth;
                        newHeight = origImage.Height * maxWidth / origImage.Width;
                    }

                    using (Bitmap newImage = new Bitmap(newWidth, newHeight))
                    {
                        using (Graphics gr = Graphics.FromImage(newImage))
                        {
                            gr.SmoothingMode = SmoothingMode.AntiAlias;
                            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            gr.DrawImage(origImage, new Rectangle(0, 0, newWidth, newHeight));

                            MemoryStream ms = new MemoryStream();
                            newImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            return ms.ToArray();
                        }
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public static byte[] ConvertImageUrl(string path)
        {
            return System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(path));
        }

        public static bool CheckImgExtension(string nameImg, Page page)
        {
            string extension = System.IO.Path.GetExtension(page.Server.HtmlEncode(nameImg)).ToLower();

            if ((extension == ".jpg") || (extension == ".png") || (extension == ".jpeg") || (extension == ".gif"))
            {
                return true;
            }
            return false;
        }

        public static string ReSizeImageTHAI(string path, string name, string radom, int width, int height, HttpPostedFileBase imageFileUpload)
        {
            string result = "";

            name = radom + "_" + name;
            System.Drawing.Image picTmp = System.Drawing.Image.FromStream(imageFileUpload.InputStream);

            int srcWidth = picTmp.Width;
            int srcHeight = picTmp.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)srcWidth);
            nPercentH = ((float)height / (float)srcHeight);

            if (width == 0)//Auto width
            {
                nPercent = nPercentH;
            }
            else if (height == 0)//auto height
            {
                nPercent = nPercentW;
            }
            else if (nPercentW > nPercentH)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }

            int thumb_width = 0;
            int thumb_height = 0;

            if (nPercent < 1)
            {
                thumb_width = (int)(srcWidth * nPercent);
                thumb_height = (int)(srcHeight * nPercent);

                Bitmap bmp = new Bitmap(thumb_width, thumb_height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                bmp.MakeTransparent(Color.White);
                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumb_width, thumb_height);
                gr.DrawImage(picTmp, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);

                bmp.Save(path + name, ImageFormat.Png);

                bmp.Dispose();
                result = path + name;
            }
            else //Just make a copy
            {
                //System.IO.File.Copy(source_file, thumb_file);
                imageFileUpload.SaveAs(path + name);
                result = path + name;
            }

            picTmp.Dispose();

            return result;
        }

        public static string ConvertFileName(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            string temp = sb.ToString().Normalize(NormalizationForm.FormC);
            char[] chars = @"–$%#@!*?;:~`+=()[]{}|\'<>,/^&""".ToCharArray();
            int len = chars.Length;
            for (int i = 0; i < len; i++)
            {
                string strChar = chars.GetValue(i).ToString();
                if (temp.Contains(strChar))
                {
                    temp = temp.Replace(strChar, string.Empty);
                }
            }
            temp = Regex.Replace(temp, "Đ|đ|đ|Đ", "d", RegexOptions.IgnoreCase);
            temp = Regex.Replace(temp, @"\s+", " ", RegexOptions.Multiline);
            return temp.Replace(" ", "-").ToLower();
        }

        public static string ConvertFileNameNotVietNamce(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            string temp = sb.ToString().Normalize(NormalizationForm.FormC);
            char[] chars = @"–$%#@!*?;:~`+=()[]{}|\'<>,/^&""".ToCharArray();
            int len = chars.Length;
            for (int i = 0; i < len; i++)
            {
                string strChar = chars.GetValue(i).ToString();
                if (temp.Contains(strChar))
                {
                    temp = temp.Replace(strChar, string.Empty);
                }
            }
            temp = Regex.Replace(temp, "Đ|đ|đ|Đ", "d", RegexOptions.IgnoreCase);

            return temp.Replace(" ", "").ToLower();
        }

        /// <summary>
        /// Fomat name giữ khoảng trắng
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertFileNameSpace(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            string temp = sb.ToString().Normalize(NormalizationForm.FormC);
            char[] chars = @"–$%#@!*?;:~`+=()[]{}|\'<>,/^&""".ToCharArray();
            int len = chars.Length;
            for (int i = 0; i < len; i++)
            {
                string strChar = chars.GetValue(i).ToString();
                if (temp.Contains(strChar))
                {
                    temp = temp.Replace(strChar, string.Empty);
                }
            }
            temp = Regex.Replace(temp, "Đ|đ|đ|Đ", "d", RegexOptions.IgnoreCase);

            return temp.Replace(" ", " ").ToLower();
        }

        public static string[] GetExeption(FaultException ex)
        {
            var str = ex.CreateMessageFault().Reason.ToString();
            string[] liststring = str.Split('#');
            string[] results = { liststring[1], liststring[2] };
            return results;
        }

        public static void email_send(ObjMailSend mailobj, string subject, String body)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(mailobj.FromMail);
            msg.To.Add(mailobj.ToMail); // txt la file nguoi nhan
            msg.Subject = subject; // txtSubject tieu de mail
            var contet = body;
            msg.Body = contet; // txtObject noi dung mail
            msg.IsBodyHtml = true; // xuat theo dinh dang HTML
            //System.Net.Mail.Attachment attachment;
            //attachment = new System.Net.Mail.Attachment(urlfile);
            //msg.Attachments.Add(attachment);
            SmtpClient client = new SmtpClient("smtp.zoho.com", 587); // giao thuc cua thang gmail
            client.Credentials = new System.Net.NetworkCredential(mailobj.FromMail, mailobj.PassFromMail); // usernam, password cua nguoi goi giong nhu dang nhap vao gmail cua nguoi goi
            client.EnableSsl = true;
            try
            {
                client.Send(msg);
            }
            catch (SmtpException ex)
            {
                // throw ex;
            }
        }

        public class ObjMailSend
        {
            public string FromMail { get; set; }
            public string ToMail { get; set; }
            public string PassFromMail { get; set; }
        }

        #region rezzi imag

        public static string ResizeImageNew(HttpPostedFileBase file, int width, int height, string originalDirectory, string randomImage)
        {
            var urlstr = "";
            if (file != null && file.ContentLength > 0)
            {
                //Định nghĩa đường dẫn lưu file trên server
                //ở đây mình lưu tại đường dẫn yourdomain.com/Uploads/
                //   var originalDirectory = new DirectoryInfo(@"/Uploads");
                //Lưu trữ hình ảnh theo từng tháng trong năm
                var date = DateTime.Now;
                var year = date.Year.ToString();
                var mouth = date.Month;
                var day = date.Day;
                var foder = "";
                if (width <= 300)
                {
                    foder = "Small";
                }
                else if (width <= 500)
                {
                    foder = "Medium";
                }
                else
                {
                    foder = "Large";
                }
                var urldetail = year + "/" + mouth + "/" + day + "/" + foder;
                string pathString = System.IO.Path.Combine(originalDirectory.ToString(), urldetail);
                bool isExists = System.IO.Directory.Exists(pathString);
                if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                string newFileName = /*randomImage + "_" +*/ ConvertFileName(file.FileName);
                var path = string.Format("{0}\\{1}", pathString, newFileName);
                urlstr = "/FileUpload/" + year + "/" + mouth + "/" + day + "/" + "?" + newFileName;
                //lấy đường dẫn lưu file sau khi kiểm tra tên file trên server có tồn tại hay không
                var newPath = GetNewPathForDupes(path, ref newFileName);

                string serverPath = string.Format("/{0}/{1}/{2}/{3}/{4}/{5}", "Uploads", year, mouth, day, foder, newFileName);
                //Lưu hình ảnh Resize từ file sử dụng file.InputStream
                //SaveResizeImage(Image.FromStream(file.InputStream), width, height, newPath);
                ReSizeImage(newPath, width, height, file);
                // fileNames.Add("LocalPath: " + newPath + "<br/>ServerPath: " + serverPath);
            }
            return urlstr;
        }

        public static bool SaveResizeImage(Image img, int width, int height, string path)
        {
            try
            {
                // lấy chiều rộng và chiều cao ban đầu của ảnh
                int originalW = img.Width;
                int originalH = img.Height;
                // lấy chiều rộng và chiều cao mới tương ứng với chiều rộng truyền vào của ảnh (nó sẽ giúp ảnh của chúng ta sau khi resize vần giứ được độ cân đối của tấm ảnh
                /*int resizedW = width;
                int resizedH = (originalH * resizedW) / originalW;*/
                int intMaxSide;
                int intNewWidth;
                int intNewHeight;
                if (originalW >= originalH)
                {
                    intMaxSide = originalW;
                    double dblCoef = width / (double)intMaxSide;
                    intNewWidth = Convert.ToInt32(dblCoef * originalW);
                    intNewHeight = Convert.ToInt32(dblCoef * originalH);
                }
                else
                {
                    intMaxSide = originalH;
                    double dblCoef = height / (double)intMaxSide;
                    intNewWidth = Convert.ToInt32(dblCoef * originalW);
                    intNewHeight = Convert.ToInt32(dblCoef * originalH);
                }
                /*if(intMaxSide >= width)
                {
                    double dblCoef = width / (double)intMaxSide;
                    intNewWidth = Convert.ToInt32(dblCoef * originalW);
                    intNewHeight = Convert.ToInt32(dblCoef * originalH);
                }
                else
                {
                        double dblCoef = width / (double)intMaxSide;
                        intNewHeight = Convert.ToInt32(originalH * dblCoef);
                        intNewWidth = Convert.ToInt32(originalW * dblCoef);
                }*/
                Bitmap b = new Bitmap(width, height);
                Graphics g = Graphics.FromImage((Image)b);
                g.InterpolationMode = InterpolationMode.Bicubic;    // Specify here
                g.DrawImage(img, (width - intNewWidth) / 2, (height - intNewHeight) / 2, intNewWidth, intNewHeight);
                g.Dispose();

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                b.Save(path);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        protected static void ReSizeImage(string path, int width, int height, HttpPostedFileBase ImageFileUpload)
        {
            System.Drawing.Image pic_tmp = System.Drawing.Image.FromStream(ImageFileUpload.InputStream);

            int src_width = pic_tmp.Width;
            int src_height = pic_tmp.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)src_width);
            nPercentH = ((float)height / (float)src_height);

            if (width == 0)//Auto width
            {
                nPercent = nPercentH;
            }
            else if (height == 0)//auto height
            {
                nPercent = nPercentW;
            }
            else if (nPercentW > nPercentH)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }

            int thumb_width = 0;
            int thumb_height = 0;

            if (nPercent < 1)
            {
                thumb_width = (int)(src_width * nPercent);
                thumb_height = (int)(src_height * nPercent);

                Bitmap bmp = new Bitmap(thumb_width, thumb_height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                bmp.MakeTransparent(Color.White);
                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumb_width, thumb_height);
                gr.DrawImage(pic_tmp, rectDestination, 0, 0, src_width, src_height, GraphicsUnit.Pixel);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                bmp.Save(path, ImageFormat.Png);

                bmp.Dispose();
            }
            else //Just make a copy
            {
                ImageFileUpload.SaveAs(path);
            }

            pic_tmp.Dispose();
        }

        private static string GetNewPathForDupes(string path, ref string fn)
        {
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            int counter = 1;
            string newFullPath = path;
            string new_file_name = filename + extension;
            while (System.IO.File.Exists(newFullPath))
            {
                string newFilename = string.Format("{0}({1}){2}", filename, counter, extension);
                new_file_name = newFilename;
                newFullPath = Path.Combine(directory, newFilename);
                counter++;
            };
            fn = new_file_name;
            return newFullPath;
        }

        #endregion rezzi imag

        public static string InitUrl(string catUrl, long id, string mid, double priceFrom, double priceTo, string param)
        {
            string html = catUrl;

            if (mid != null)
            {
                // html += "?mid=" + mid;
                if (priceFrom != 0 || priceTo != 0)
                {
                    html += "&price=" + priceFrom + ":" + priceTo;
                    if (!string.IsNullOrEmpty(param))
                    {
                        html += "&" + param;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(param))
                    {
                        html += "&" + param;
                    }
                }
            }
            else
            {
                if (priceFrom != 0 || priceTo != 0)
                {
                    html += "?price=" + priceFrom + ":" + priceTo;
                    if (!string.IsNullOrEmpty(param))
                    {
                        html += "&" + param;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(param))
                    {
                        html += "?" + param;
                    }
                }
            }

            return html;
        }

        public static string InitUrlFilter(string catUrl, string mid, string param)
        {
            string html = catUrl + param;

            return html;
        }

        public static string InitUrlOrder(string catUrl, string ordercu, string ordermoi)
        {
            string html = "";
            if (catUrl.Contains("order"))
                html = catUrl.Replace(ordercu, ordermoi);
            else
            {
                if (catUrl.Contains('?'))
                    html = catUrl + "&order=" + ordermoi;
                else
                    html = catUrl + "?order=" + ordermoi;
            }

            return html;
        }

        public static string InitUrlPage(string catUrl, string pagecu, string pagemoi)
        {
            string html = "";
            if (catUrl.Contains("page"))
                html = catUrl.Replace(pagecu, pagemoi);
            else
            {
                if (catUrl.Contains('?'))
                    html = catUrl + "&page=" + pagemoi;
                else
                    html = catUrl + "?page=" + pagemoi;
            }

            return html;
        }

        public static string InitUrlFilter(string catUrl, NameValueCollection param, int dau, string key, string value)
        {
            string html = catUrl;
            string query = "";
            NameValueCollection n = param;
            bool container = false;
            if (dau == 1)   //thêm giá trị vào link
            {
                // neu chuoi query co gia tri
                if (n.Count > 0)
                {
                    for (int j = 0; j < n.Count; j++)
                    {
                        string k1 = n.GetKey(j);
                        string v = n.Get(j);
                        //khong tinh thuoc tinh page
                        if (k1 != "page")
                        {
                            //key co ton tai trong query khong neu co thi se cong them gia tri và thoat ra vong lap
                            if (k1 == key)
                            {
                                container = true;
                                v += "," + value;
                                query += k1 + "=" + v + "&";
                            }
                            // Van add gia tri cu vao
                            else
                            {
                                query += k1 + "=" + v + "&";
                            }
                        }
                    }
                    // key khong ton tai trong vong lap thi cong them key vao query
                    if (!container)
                    {
                        query += key + "=" + value + "&";
                    }
                }
                // neu chuoi query khong co gia tri gi
                else
                {
                    query += key + "=" + value + "&";
                }
            }
            else
            {
                for (int j = 0; j < n.Count; j++)
                {
                    string k1 = n.GetKey(j);
                    string v = n.Get(j);
                    if (k1 != "page")
                    {
                        if (k1 == key)
                        {
                            v = v.Replace(value, "").Replace(",,", ",");
                            //truong hop xoa ky tu cuoi cung se thua dau , thi xoa luon
                            if (v.EndsWith(","))
                                v = v.Remove(v.Length - 1);
                            // Nếu xóa vị trí đầu xóa dau , đi
                            if (v.StartsWith(","))
                                v = v.Remove(0, 1);
                        }
                        if (v != "")
                            query += k1 + "=" + v + "&";
                    }
                }
            }
            if (query != "")
                html = catUrl + "?" + query.Remove(query.Length - 1);
            else
                html = catUrl;
            return html;
        }

        public static string InitUrlDetaiProduct(string url, long id)
        {
            return string.Format("/{0}-ct{1}", url, id) + ".html";
        }

        #region PAGING

        public static int[] Paging(int countRecord, object obj, int SIZE_PAGE, int SIZE_OF_PAGE, ref int page, ref int numberPage)
        {
            int[] arrayPage;
            int firtPage, lastPage = 0, j = 0;
            numberPage = 0;
            //Tính số trang
            if (countRecord % SIZE_OF_PAGE == 0)
                numberPage = countRecord / SIZE_OF_PAGE;
            else
                numberPage = countRecord / SIZE_OF_PAGE + 1;

            if (numberPage >= SIZE_PAGE * 2 + 1)
                arrayPage = new int[SIZE_PAGE * 2 + 1];
            else
                arrayPage = new int[numberPage];

            //Tìm trang hiện tại

            if (obj != null)
                page = Int32.Parse(obj.ToString());
            else
                page = 1;

            if (numberPage <= SIZE_PAGE * 2 + 1)
            {
                firtPage = 1;
                lastPage = numberPage;
            }
            else
            {
                if (page <= SIZE_PAGE + 1)
                {
                    firtPage = 1;
                    lastPage = SIZE_PAGE * 2 + 1;
                }
                else
                {
                    if (page + SIZE_PAGE <= numberPage)
                    {
                        firtPage = page - SIZE_PAGE;
                        lastPage = page + SIZE_PAGE;
                    }
                    else
                    {
                        lastPage = numberPage;
                        firtPage = numberPage - SIZE_PAGE * 2;
                    }
                }
            }
            for (int i = firtPage; i <= lastPage; i++)
            {
                arrayPage[j] = i;
                j++;
            }
            return arrayPage;
        }

        #endregion PAGING

        public static readonly string[] VietnameseSigns = new string[]

    {
        "aAeEoOuUiIdDyY",

        "áàạảãâấầậẩẫăắằặẳẵ",

        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

        "éèẹẻẽêếềệểễ",

        "ÉÈẸẺẼÊẾỀỆỂỄ",

        "óòọỏõôốồộổỗơớờợởỡ",

        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

        "úùụủũưứừựửữ",

        "ÚÙỤỦŨƯỨỪỰỬỮ",

        "íìịỉĩ",

        "ÍÌỊỈĨ",

        "đ",

        "Đ",

        "ýỳỵỷỹ",

        "ÝỲỴỶỸ"
    };

        protected static string RemoveSign4VietnameseString(string str)
        {
            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }

            return str;
        }

        public static string ConvertUrlString(string s1)
        {
            string temp = RemoveSign4VietnameseString(s1);

            /* string stFormD = s.Normalize(NormalizationForm.FormD);
             StringBuilder sb = new StringBuilder();

             for (int ich = 0; ich < stFormD.Length; ich++)
             {
                 UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                 if (uc != UnicodeCategory.NonSpacingMark)
                 {
                     sb.Append(stFormD[ich]);
                 }
             }*/
            // string temp = sb.ToString().Normalize(NormalizationForm.FormC);
            char[] chars = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                string strChar = chars.GetValue(i).ToString();
                if (temp.Contains(strChar))
                {
                    temp = temp.Replace(strChar, string.Empty);
                }
            }
            temp = Regex.Replace(temp, "Đ|đ|đ|Đ", "d", RegexOptions.IgnoreCase);

            return temp.Replace(" ", "-").ToLower();
        }

        #region Mesebox

        public static string MessengerBox(string type, string mes)
        {
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(mes))
            {
                return null;
            }
            else
            {
                var str = "<ul class=\"x\"><li class=\"" + type + "\">" +
                      "<ul> <li><span>" + mes + "</span></li>" +
                      "</ul></li></ul>";
                return str;
            }
        }

        #endregion Mesebox

        #region Read/WriteFile

        public static void WriteFile(string url, string[] text)
        {
            File.WriteAllLines(url, text, Encoding.Unicode);
        }

        public static List<string> ReadFile(string url)
        {
            var text = File.ReadAllLines(url);
            return text.ToList();
        }

        #endregion Read/WriteFile
    }
}