using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class ShoppingCartController : Controller
    {
        GoatSneakerEntities4 db = new GoatSneakerEntities4();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("EmptyCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(int id)
        {
            var _pro = db.Products.SingleOrDefault(s => s.IDProduct == id);
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.Update_quantity(id_pro, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity_item = cart.Total_quantity();
            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }


        public ActionResult CheckOut_Success()
        {
            return View();
        }
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro _order = new OrderPro();
                _order.DateOrder = DateTime.Now;
                _order.AddressDeliverry = form["AddressDeliverry"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);
                db.OrderProes.Add(_order);
                foreach (var item in cart.Items)
                {
                    OrderDetail _order_detail = new OrderDetail();
                    _order_detail.IDDetail = _order.IDPro;
                    _order_detail.IDProduct = item._product.IDProduct;
                    _order_detail.UnitPrice = (double)item._product.Price;
                    _order_detail.Quantity = item._quantity;
                    db.OrderDetails.Add(_order_detail);
                    foreach (var p in db.Products.Where(s => s.IDProduct == _order_detail.IDProduct))
                    {
                        var update_quan_pro = p.Quantity - item._quantity;
                        p.Quantity = update_quan_pro;
                    }
                }
                db.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");
            }
            catch
            {
                return Content("Lỗi thanh toán. Vui lòng kiểm tra lại thông tin của khách hàng...Cảm ơn...");
            }
        }

        public ActionResult EmptyCart()
        {
            return View();
        }
    }
}