//using BuySellGoods.Common;
//using BuySellGoods.Models;
//using FakeModelDB.DaoModel;
//using FakeModelDB.EF;

using Business.ClassBusiness;
using BuyGroup365.Models.ApiGoogle;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BuyGroup365.Controllers
{
    public class PayGoodsController : Controller
    {
        private OrdersBusiness ordersBusiness = new OrdersBusiness();
        private OrderDetailBusiness orderDetailBusiness = new OrderDetailBusiness();
        private OrderBuyerBusiness orderBuyerBusiness = new OrderBuyerBusiness();
        private OrderReciverBusiness orderReciverBusiness = new OrderReciverBusiness();
        private ShopsBusiness shopsBusiness = new ShopsBusiness();
        private LocationsBusiness _locationsBusiness = new LocationsBusiness();
        //protected override void Initialize(RequestContext rc)
        //{
        //    base.Initialize(rc);

        //    if (!SessionUtility.CheckLoginMember(Session))
        //    {
        //        Response.Redirect("/Login/Login?returnUrl=" + rc.HttpContext.Request.Url.PathAndQuery);
        //    }
        //}
        private const string CartSession = "CartSession";

        //
        // GET: /PayGoods/

        private List<CartItem> cart = new List<CartItem>();

        [HttpGet]
        public ActionResult Payment(long? sp)
        {
            ViewBag.TotalMoney = 0;
            ViewBag.TotalGram = 0;
            LoadDropdown loadDropdown = new LoadDropdown();
            ViewBag.Location = loadDropdown.SearchLocationParenId(1, null);
            if (Session != null)
            {
                var entity = NlCheckout.GetSessionCard(Session);
                List<ShopCartItem> shopCartItems = new List<ShopCartItem>();
                List<long> listIdShop = new List<long>();
                if (sp == null)
                {
                    foreach (var item in entity)
                    {
                        if (!listIdShop.Exists(x => x.Equals(item.Product.MemberId)))
                        {
                            listIdShop.Add(item.Product.MemberId);
                            var shop = shopsBusiness.GetById(item.Product.MemberId);
                            ShopCartItem shopCartItem = new ShopCartItem();
                            shopCartItem.ShopCart = shop;
                            var listCart = entity.Where(x => x.Product.MemberId == item.Product.MemberId).ToList();
                            shopCartItem.ListCartItems = listCart;
                            shopCartItems.Add(shopCartItem);
                        }
                    }
                }
                else
                {
                    foreach (var item in entity)
                    {
                        if (!listIdShop.Exists(x => x.Equals(sp)))
                        {
                            listIdShop.Add(item.Product.MemberId);
                            var shop = shopsBusiness.GetById((long)sp);
                            ShopCartItem shopCartItem = new ShopCartItem();
                            shopCartItem.ShopCart = shop;
                            var listCart = entity.Where(x => x.Product.MemberId == sp).ToList();
                            shopCartItem.ListCartItems = listCart;
                            shopCartItems.Add(shopCartItem);
                        }
                    }
                    var shopCartItemsentity = shopCartItems.FirstOrDefault();
                    ViewBag.TotalMoney = shopCartItemsentity.ListCartItems.Sum(x => x.Product.Price);
                    var totalWeghit = 0.0;
                    foreach (var listCartItem in shopCartItemsentity.ListCartItems)
                    {
                        totalWeghit += listCartItem.Quantity * listCartItem.Product.Weight;
                    }
                    ViewBag.TotalGram = totalWeghit;
                }

                return View(shopCartItems);
            }
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public ActionResult PaymentCard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Payment(double kmRoad, double gramGoods, double totalMoneyPay, double feeTranspotGood, string nameOfBuyer, string buyerPhone, string buyerEmail, string buyerAddress, string receiverName, string receiverPhone, string receiverEmail, string receiverAddress, long formPayGood, string receiverTown, string receiverPhuong, string note, long shopid)
        {
            //    var id = Request.QueryString["sp"];
            if (ModelState.IsValid)
            {
                //  var shopId = long.Parse(Request.QueryString["sp"]);
                //var idShop = FakeModelCartItem().First().Product.IdShop;
                //Member entity = SessionUtility.GetSessionMember(Session);
                // List<long> listShop=new List<long>();
                // var listoder = NlCheckout.GetSessionCard(Session);
                //foreach (var cartItem in listoder)
                //{
                //    var id = cartItem.Product.MemberId;
                //    if (listShop.Exists(x => x.Equals(id)))
                //    {
                //    }
                //    else
                //    {
                //        listShop.Add(id);
                //    }
                //}
                //foreach (var idshop in listShop)
                //{
                //}

                var order = new Order();
                order.FeeOfTranspot = 0;
                order.GramGood = 0;
                order.KmRoad = kmRoad;
                order.TotalMoney = 0;
                order.IdPayForm = formPayGood;
                order.IdShop = shopid;
                order.NoteAboutOrder = note;
                //  ordersBusiness.AddNew(order);

                if (formPayGood == 0)
                {
                    //thanh toan qua ngan luong
                    string return_url = "http://demo.nganluong.vn";// Địa chỉ trả về
                    string transaction_info = "Hãy lập trình thông tin của giao dịch của bạn vào đây";//Thông tin giao dịch
                    string order_code = order.Id.ToString();//Mã giỏ hàng
                    string receiver = "ngodangkhoa.04@gmail.com";//Tài khoản nhận tiền
                    string price = totalMoneyPay.ToString();//Lấy giá của giỏ hàng
                    BuyGroup365.Models.Member.NlCheckout nl = new BuyGroup365.Models.Member.NlCheckout();
                    string url;
                    url = nl.buildCheckoutUrl(return_url, receiver, transaction_info, order_code, price);
                    //ImageButton imgBtn = new ImageButton();
                    //imgBtn.ImageUrl = "https://www.nganluong.vn/data/images/buttons/11.gif";//source file ảnh
                    //imgBtn.PostBackUrl = url;//Gán địa chỉ url cho nút thanh toán
                    order.UrlBtnPayNL = url;
                    ordersBusiness.Edit(order);
                }
                else if (formPayGood == 1)
                {
                    //thanh toan qua bao kim
                }
                else
                {
                    //nguoi mua va nguoi ban tu thuong luong gia ca
                }
                //Order detail
                //var listItem = (List<CartItem>)Session[CartSession];
                var sesion = NlCheckout.GetSessionCard(Session);
                Member entity = SessionUtility.GetSessionMember(Session);
                LocationsBusiness locationsBusiness = new LocationsBusiness();
                double moneytransport = 0;
                double totalmoney = 0;
                double totalGram = 0;
                List<OrderDetail> listOrderDetails = new List<OrderDetail>();
                foreach (var item in sesion)
                {
                    if (item.Product.MemberId == shopid)
                    {
                        var orderDetail = new OrderDetail();
                        orderDetail.IdOrder = order.Id;
                        orderDetail.IdProduct = item.Product.Id;

                        orderDetail.Price = (decimal)item.Product.Price;
                        moneytransport += FuntionMember.CountMoneyTransport(kmRoad, item.Product.Weight, receiverAddress);
                        totalGram += item.Product.Weight;
                        totalmoney += item.Product.Price + moneytransport;
                        orderDetail.NameProduct = item.Product.ProductName;
                        orderDetail.PathImage = item.Product.ProductImages.First(x => x.IsAvatar == 1).ImgSource;
                        orderDetail.Quantity = item.Quantity;

                        //    var orderDetailDao = new OrderDetailDao();
                        //   orderDetailBusiness.AddNew(orderDetail);
                        listOrderDetails.Add(orderDetail);
                    }
                }
                order.TotalMoney = totalmoney;
                order.FeeOfTranspot = totalMoneyPay;
                order.GramGood = totalGram;
                order.OrderDetails = listOrderDetails;
                //   ordersBusiness.Edit(order);
                //Buyer
                Member member = SessionUtility.GetSessionMember(Session);
                var buyer = new OrderBuyer();
                buyer.Id = order.Id;
                buyer.Name = nameOfBuyer;
                buyer.PhoneNumber = buyerPhone;
                buyer.Email = buyerEmail;
                buyer.Address = buyerAddress;
                buyer.CreateDate = DateTime.Now;
                if (member != null)
                {
                    buyer.IdMember = member.Id;// người đăng nhập mua hàng
                }

                //var buyerDao = new BuyerDao();
                //buyerDao.Insert(buyer);
                //  orderBuyerBusiness.AddNew(buyer);
                order.OrderBuyer = buyer;
                //Reciver
                var reciver = new OrderReciver();
                reciver.Id = order.Id;
                reciver.Name = receiverName;
                reciver.PhoneNumber = receiverPhone;
                reciver.Email = receiverEmail;
                reciver.Address = receiverTown + " - " + receiverPhuong + " - " + receiverAddress;
                reciver.CreateDate = DateTime.Now;
                //var reciverDao = new ReciverDao();
                //reciverDao.Insert(reciver);
                order.OrderReciver = reciver;
                //  orderReciverBusiness.AddNew(reciver);
                //  var idShop = entity.Id;
                //Order

                #region Xoa san phâm trong sesion

                List<CartItem> listCartItems = new List<CartItem>();
                foreach (var item in sesion)
                {
                    if (item.Product.MemberId != shopid)
                    {
                        listCartItems.Add(item);
                    }
                    else
                    {
                    }
                }
                ordersBusiness.AddNew(order);
                NlCheckout.SetSessionCard(listCartItems, Session);

                #endregion Xoa san phâm trong sesion

                string body = ControllerExtensions.RenderRazorViewToString(this, "DetailCart", order);

                Function.ObjMailSend objmail = new Function.ObjMailSend();
                var mailsend = new SystemSettingBusiness().GetDynamicQuery().First(x => x.Key == "mail_noreply");
                var acount = mailsend.Value.Split('|');
                objmail.FromMail = acount[0];
                objmail.PassFromMail = acount[1];
                objmail.ToMail = reciver.Email;

                Function.email_send(objmail, "Thông tin đơn hàng(" + DateTime.Now + ")", body);
                return Json(listCartItems, JsonRequestBehavior.AllowGet);
            }
            return Json(1);
        }

        public ActionResult CheckPayGoods()
        {
            bool check;
            //var idShop = FakeModelCartItem().First().Product.IdShop;
            //   Member entity = SessionUtility.GetSessionMember(Session);
            var listoder = NlCheckout.GetSessionCard(Session);
            var idShop = listoder.First().Product.MemberId;

            string transaction_info = Request.QueryString["transaction_info"];
            string order_code = Request.QueryString["order_code"];
            string payment_id = Request.QueryString["payment_id"];
            string payment_type = Request.QueryString["payment_type"];
            string secure_code = Request.QueryString["secure_code"];
            string price = Request.QueryString["price"];
            string error_text = Request.QueryString["error_text"];
            BuyGroup365.Models.Member.NlCheckout nl = new BuyGroup365.Models.Member.NlCheckout();
            check = nl.verifyPaymentUrl(transaction_info, order_code, price, payment_id, payment_type, error_text, secure_code);
            // (String transaction_info, String order_code, String price, String payment_id, String payment_type, String error_text, String secure_code)
            if (check)
            {
                ViewBag.Check = "Quá trình thanh toán thành công chúng tôi sẽ chuyển hàng sớm cho bạn";
            }
            else
                ViewBag.Check = "Quá trình thanh toán thất bại bạn vui lòng thử lại";

            return View();
        }

        public JsonResult Update(string cartModel)
        {
            ProductsBusiness productsBusiness = new ProductsBusiness();
            var serializerSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            var jsonCart = JsonConvert.DeserializeObject<List<CartItem>>(cartModel, serializerSettings);
            var sesion = NlCheckout.GetSessionCard(Session);
            List<CartItem> listCartItems = new List<CartItem>();
            foreach (var item in sesion)
            {
                foreach (var item2 in jsonCart)
                {
                    if (item.Product.Id == item2.Product.Id)
                    {
                        item.Quantity = item2.Quantity;
                    }
                }
                listCartItems.Add(item);
            }
            //  var sessionCart = (List<CartItem>)Session[CartSession];
            NlCheckout.SetSessionCard(listCartItems, Session);
            //foreach (var item in sessionCart)
            //{
            //    var jsonItem = jsonCart.SingleOrDefault(x => x.Product.Id == item.Product.Id);
            //    if (jsonItem != null)
            //    {
            //        item.Quantity = jsonItem.Quantity;
            //    }
            //}
            //Session[CartSession] = sessionCart;
            // string body = ControllerExtensions.RenderRazorViewToString(this, "PartialProductDetailCart", jsonCart);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Xoa sản phẩm trong giỏ hàng
        /// </summary>
        /// <param name="id">Id của sản phẩm cần xóa</param>
        /// <param name="tp">tp là biến Type: tp=1 là xóa từng sản phẩm, tp!=1 xóa all</param>
        /// <returns></returns>

        public JsonResult DeleteCart(long id, string tp)
        {
            if (tp == "1")
            {
                List<CartItem> listItems = new List<CartItem>();
                var listObj = NlCheckout.GetSessionCard(Session);
                foreach (var cartItem in listObj)
                {
                    if (cartItem.Product.Id != id)
                    {
                        listItems.Add(cartItem);
                    }
                }
                NlCheckout.SetSessionCard(listItems, Session);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                NlCheckout.SetSessionCard(null, Session);
                return Json(2, JsonRequestBehavior.AllowGet);//Xóa hết đơn hàng
            }
        }

        public JsonResult GetLocationByParent(long parentId)
        {
            var listlocation = _locationsBusiness.GetDynamicQuery().Where(x => x.ParentId == parentId).OrderBy(x => x.Name).ToList();
            var htm = "";
            if (listlocation.Any())
            {
                htm += "<option value=\"-1\">--- Chọn quận huyện ---</option>";
                foreach (var item in listlocation)
                {
                    htm += "<option value=\"" + item.Id + "\">" + item.Name + "</option>";
                }
            }
            else
            {
                htm = "Không tìm thấy kết quả!";
            }
            return Json(htm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.Id == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult CheckOut()
        {
            try
            {
                double transportfee = 0;

                ViewBag.TotalMoney = 0;
                ViewBag.TotalGram = 0;

                LoadDropdown loadDropdown = new LoadDropdown();
                ViewBag.Location = loadDropdown.SearchLocationParenId(1, null);
                if (Session != null)
                {
                    var entity = NlCheckout.GetSessionCard(Session);
                    ShopCartItem shopCartItem = new ShopCartItem();

                    //         ShopCartItem shopCartItem = new ShopCartItem();

                    var listCart = entity.Where(x => x.Product.MemberId == 0).ToList();
                    shopCartItem.ListCartItems = listCart;
                    //    shopCartItems.Add(shopCartItem);

                    //    var shopCartItemsentity = shopCartItems.FirstOrDefault();
                    ViewBag.TotalMoney = shopCartItem.ListCartItems.Sum(x => x.Product.Price);

                    var totalWeghit = 0.0;
                    foreach (var listCartItem in shopCartItem.ListCartItems)
                    {
                        totalWeghit += listCartItem.Quantity * listCartItem.Product.Weight;
                    }
                    ViewBag.TotalGram = totalWeghit;
                    Member member = SessionUtility.GetSessionMember(Session);
                    if (member != null)
                    {
                        var loctionmember = member.MemberProfile.Address + ", " + new LocationsBusiness().SearchAddress(member.MemberProfile.LocationId);
                        //var km = new FunctionCountKm().GetDistance(locationshop, loctionmember);
                        //transportfee=FuntionMember.CountMoneyTransport(km, totalWeghit, loctionmember);
                    }
                    ViewBag.Transportfee = transportfee;
                    return View(shopCartItem);
                }
            }
            catch { }
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="from">Vị trí shop</param>
        /// <param name="to">Ví trí người nhân</param>
        /// <param name="gram">Tổng khối lương đơn hàng</param>
        /// <returns></returns>
        public JsonResult CountKm(string from, string to, float gram)
        {
            var km = new FunctionCountKm().GetDistance(from, to);
            var transportfee = FuntionMember.CountMoneyTransport(km, gram, to);
            return Json(transportfee, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaymentCheckOut(string recieveOrder, string buyerOrder, string note, long shopid, long payfrom, string TaxOrder)
        {
            ProductsBusiness productsBusiness = new ProductsBusiness();
            var serializerSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            var recieve = JsonConvert.DeserializeObject<List<string>>(recieveOrder, serializerSettings);
            var buyeror = JsonConvert.DeserializeObject<List<string>>(buyerOrder, serializerSettings);
            var taxOrder = JsonConvert.DeserializeObject<List<string>>(TaxOrder, serializerSettings);

            if (ModelState.IsValid)
            {
                var recieveadress = recieve[4];

                var order = new Order();
                order.FeeOfTranspot = 0;
                order.GramGood = 0;
                order.KmRoad = 0;
                order.TotalMoney = 0;
                order.IdPayForm = payfrom;
                order.IdShop = 0;
                order.NoteAboutOrder = note;

                var sesion = NlCheckout.GetSessionCard(Session);
                Member entity = SessionUtility.GetSessionMember(Session);
                LocationsBusiness locationsBusiness = new LocationsBusiness();
                double moneytransport = 0;
                double totalmoney = 0;
                double totalGram = 0;
                List<OrderDetail> listOrderDetails = new List<OrderDetail>();
                foreach (var item in sesion)
                {
                    if (item.Product.MemberId == shopid)
                    {
                        var orderDetail = new OrderDetail();
                        orderDetail.IdOrder = order.Id;
                        orderDetail.IdProduct = item.Product.Id;

                        orderDetail.Price = (decimal)item.Product.Price;
                        moneytransport += 0;// FuntionMember.CountMoneyTransport(0, item.Product.Weight * item.Quantity, recieve[3]); //Kiêm tra tinh chính xác đoạn này
                        totalGram += item.Product.Weight;
                        totalmoney += item.Product.Price + moneytransport;
                        orderDetail.NameProduct = item.Product.ProductName;
                        orderDetail.PathImage = item.Product.ProductImages.First(x => x.IsAvatar == 1).ImgSource;
                        orderDetail.Quantity = item.Quantity;

                        listOrderDetails.Add(orderDetail);
                    }
                }
                NlCheckout.SetSessionOrderDetail(listOrderDetails, Session);
                var url = "";

                order.TotalMoney = totalmoney;
                order.FeeOfTranspot = moneytransport;
                order.GramGood = totalGram;
                order.OrderDetails = listOrderDetails;
                order.Status = 0;// đơn hàng mới

                //Buyer
                Member member = SessionUtility.GetSessionMember(Session);
                var buyer = new OrderBuyer();
                if (buyeror != null && buyeror.Any()) // lấy thông tin ngươi mua giông ng nhận hàng
                {
                    buyer.Id = order.Id;
                    buyer.Name = taxOrder[0];
                    buyer.PhoneNumber = buyeror[1];
                    buyer.Email = buyeror[2];
                    buyer.Address = taxOrder[1];
                    buyer.CMTND = taxOrder[2];
                }
                else
                {
                    buyer.Id = order.Id;
                    buyer.Name = taxOrder[0];
                    buyer.PhoneNumber = recieve[1];
                    buyer.Email = recieve[2];
                    buyer.Address = taxOrder[1];
                    buyer.CMTND = taxOrder[2];
                }

                buyer.CreateDate = DateTime.Now;
                if (member != null)
                {
                    buyer.IdMember = member.Id;// người đăng nhập mua hàng
                }

                order.OrderBuyer = buyer;
                //Reciver
                var reciver = new OrderReciver();
                reciver.Id = order.Id;
                reciver.Name = recieve[0];
                reciver.PhoneNumber = recieve[1];
                reciver.Email = recieve[2];
                // reciver.Address = recieve[4]+", "+ recieve[3];
                reciver.Address = recieve[4];
                reciver.CreateDate = DateTime.Now;
                SessionUtility.SetSessionOrderReciver(reciver, Session);
                order.OrderReciver = reciver;

                #region Xoa san phâm trong sesion

                List<CartItem> listCartItems = new List<CartItem>();
                foreach (var item in sesion)
                {
                    if (item.Product.MemberId != shopid)
                    {
                        listCartItems.Add(item);
                    }
                    else
                    {
                    }
                }
                order.IdShop = null;
                ordersBusiness.AddNew(order);
                Session["Madonhang"] = order.Id;
                Session["TotalMoney"] = order.TotalMoney;

                NlCheckout.SetSessionCard(listCartItems, Session);

                #endregion Xoa san phâm trong sesion

                string body = ControllerExtensions.RenderRazorViewToString(this, "DetailCart", order);

                Function.ObjMailSend objmail = new Function.ObjMailSend();
                var mailsend = new SystemSettingBusiness().GetDynamicQuery().First(x => x.Key == "mail_noreply");
                var acount = mailsend.Value.Split('|');
                objmail.FromMail = acount[0];
                objmail.PassFromMail = acount[1];
                objmail.ToMail = reciver.Email;

                Function.email_send(objmail, "[Noithatgoquy]Thông tin đơn hàng(" + DateTime.Now + ")", body);
                var car = NlCheckout.GetSessionCard(Session);
                if (payfrom == 200)
                {
                    //thanh toan qua ngan luong
                    string return_url = "https://wwww.vinaplaza.vn";// Địa chỉ trả về
                    string transaction_info = "info";//Thông tin giao dịch
                    string order_code = order.Id.ToString();//Mã giỏ hàng
                    string receiver = new NlCheckout().GetvalueAppsetting("acount_receiver");//Tài khoản nhận tiền
                    string price = totalmoney.ToString();//Lấy giá của giỏ hàng
                    BuyGroup365.Models.Member.NlCheckout nl = new BuyGroup365.Models.Member.NlCheckout();
                    //string url;
                    url = nl.buildCheckoutUrl(return_url, receiver, transaction_info, order_code, price);
                    //ImageButton imgBtn = new ImageButton();
                    //imgBtn.ImageUrl = "https://www.nganluong.vn/data/images/buttons/11.gif";//source file ảnh
                    //imgBtn.PostBackUrl = url;//Gán địa chỉ url cho nút thanh toán
                    // Response.Redirect(url);
                }
                else if (payfrom == 1)
                {
                    //thanh toan qua bao kim
                }
                else
                {
                    //nguoi mua va nguoi ban tu thuong luong gia ca
                }
                if (payfrom == 200)
                {
                    return Json(url, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (car != null && car.Any())
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(1, JsonRequestBehavior.AllowGet); // thay đôi lại luồng trước đây phân ra 2 trương hợp như thê này đê Render tới các page khác nhau
                    }
                }

                //  return Json(listCartItems, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckLoginPay(string usename, string pass)
        {
            var entity = new MembersBusiness().CheckLogin(usename, pass);
            if (entity != null)
            {
                if (entity.Status == 0)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else if (entity.Status == 3)
                {
                    return Json(3, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    SessionUtility.SetSessionMember(entity, Session);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(4, JsonRequestBehavior.AllowGet);
            }
        }
    }
}