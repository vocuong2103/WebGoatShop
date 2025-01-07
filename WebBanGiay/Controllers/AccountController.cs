using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class AccountController : Controller
    {
        GoatSneakerEntities4 db = new GoatSneakerEntities4();
        //Danh sách tài khoản
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        //Thêm tài khoản
        public ActionResult Create()
        {
            Account ac = new Account();
            return View(ac);
        }
        [HttpPost]
        public ActionResult Create(Account ac)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (GoatSneakerEntities4 db = new GoatSneakerEntities4())
                    {
                        db.Accounts.Add(ac);
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
            return View(ac);

        }
        //Chỉnh sửa tài khoản
        public ActionResult Edit(int? id)
        {
            return View(db.Accounts.Where(s => s.IDAccount == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int? id, Account ac)
        {
            db.Entry(ac).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Xóa tài khoản
        public ActionResult Delete(int id)
        {
            return View(db.Accounts.Where(s => s.IDAccount == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Account ac)
        {
            try
            {
                ac = db.Accounts.Where(s => s.IDAccount == id).FirstOrDefault();
                db.Accounts.Remove(ac);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
            }
        }

        //Chi tiết tài khoản
        public ActionResult Details(int? id)
        {
            return View(db.Accounts.Where(s => s.IDAccount == id).FirstOrDefault());
        }
    }
}