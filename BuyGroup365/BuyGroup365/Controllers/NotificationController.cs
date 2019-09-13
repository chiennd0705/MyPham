using Business.ClassBusiness;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using System.Linq;
using System.Web.Mvc;

namespace BuyGroup365.Controllers
{
    public class NotificationController : Controller
    {
        //
        // GET: /Notification/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderContent(long od)
        {
            var oder = new OrdersBusiness().GetById(od);
            return View(oder);
        }

        public ActionResult Success()
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
            catch
            {
            }
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}