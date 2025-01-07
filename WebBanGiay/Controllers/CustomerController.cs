using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class CustomerController : Controller
    {
        GoatSneakerEntities4 db = new GoatSneakerEntities4();
        // GET: Order
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        //Thêm khách hàng
        public ActionResult Create()
        {
            Customer cs = new Customer();
            return View(cs);
        }
        [HttpPost]
        public ActionResult Create(Customer cs)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (GoatSneakerEntities4 db = new GoatSneakerEntities4())
                    {
                        db.Customers.Add(cs);
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
            return View(cs);

        }
        //Chỉnh sửa khách hàng
        public ActionResult Edit(int? id)
        {
            return View(db.Customers.Where(s => s.IDCus == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int? id, Customer cs)
        {
            db.Entry(cs).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Xóa khách hàng
        public ActionResult Delete(int id)
        {
            return View(db.Customers.Where(s => s.IDCus == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Customer cs)
        {
            try
            {
                cs = db.Customers.Where(s => s.IDCus == id).FirstOrDefault();
                db.Customers.Remove(cs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
            }
        }

        //Chi tiết khách hàng
        public ActionResult Details(int? id)
        {
            return View(db.Customers.Where(s => s.IDCus == id).FirstOrDefault());
        }
    }
}