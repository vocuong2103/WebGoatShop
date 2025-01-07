using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class OrderController : Controller
    {
        GoatSneakerEntities4 db = new GoatSneakerEntities4();
        // GET: Order
        public ActionResult Index()
        {
            return View(db.OrderProes.ToList());
        }

        //Thêm đơn hàng
        public ActionResult Create()
        {
            OrderPro op = new OrderPro();
            return View(op);
        }
        [HttpPost]
        public ActionResult Create(OrderPro op)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (GoatSneakerEntities4 db = new GoatSneakerEntities4())
                    {
                        db.OrderProes.Add(op);
                        db.SaveChanges();

                    }
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            // In ra các thông điệp lỗi cụ thể
                            Console.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }

            }
            return View(op);

        }
        //Chỉnh sửa đơn hàng
        public ActionResult Edit(int? id)
        {
            return View(db.OrderProes.Where(s => s.IDPro == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int? id, OrderPro op)
        {
            db.Entry(op).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Xóa đơn hàng
        public ActionResult Delete(int id)
        {
            return View(db.OrderProes.Where(s => s.IDPro == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, OrderPro op)
        {
            try
            {
                op = db.OrderProes.Where(s => s.IDPro == id).FirstOrDefault();
                db.OrderProes.Remove(op);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
            }
        }

        //Chi tiết đơn hàng
        public ActionResult Details(int? id)
        {
            return View(db.OrderProes.Where(s => s.IDPro == id).FirstOrDefault());
        }
        //Thống kê
        public ActionResult RevenueReport(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var totalRevenue = db.OrderProes
                .Where(o => o.DateOrder >= startDate && o.DateOrder <= endDate)
                .Sum(o => o.OrderDetails.Sum(od => od.Quantity * od.Price));

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }
    }
}