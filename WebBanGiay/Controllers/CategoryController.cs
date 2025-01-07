using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class CategoryController : Controller
    {
        GoatSneakerEntities4 db = new GoatSneakerEntities4();
        //Danh mục sản phẩm
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }
        public ActionResult Nike()
        {
            var nike = db.Products.Where(p => p.NamePro.Contains("Nike"));
            return View(nike);
        }
        public ActionResult Jordan()
        {
            var jordan = db.Products.Where(p => p.NamePro.Contains("Jordan"));
            return View(jordan);
        }
        public ActionResult Adidas()
        {
            var adidas = db.Products.Where(p => p.NamePro.Contains("Adidas"));
            return View(adidas);
        }
        public ActionResult Vans()
        {
            var vans = db.Products.Where(p => p.NamePro.Contains("Vans"));
            return View(vans);
        }
        public ActionResult Converse()
        {
            var converse = db.Products.Where(p => p.NamePro.Contains("Converse"));
            return View(converse);
        }
        public ActionResult Other()
        {
            var other = db.Products.Where(p => p.Price <= 60);
            return View(other);
        }

        //Thêm danh mục sản phẩm
        public ActionResult Create()
        {
            Category ct = new Category();
            return View(ct);
        }
        [HttpPost]
        public ActionResult Create(Category ct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (GoatSneakerEntities4 db = new GoatSneakerEntities4())
                    {
                        db.Categories.Add(ct);
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
            return View(ct);

        }
        //Chỉnh sửa danh mục sản phẩm
        public ActionResult Edit(string id)
        {
            return View(db.Categories.Where(s => s.IDCate == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(string id, Category ct)
        {
            db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Xóa danh mục sản phẩm
        public ActionResult Delete(string id)
        {
            return View(db.Categories.Where(s => s.IDCate == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(string id, Category ct)
        {
            try
            {
                ct = db.Categories.Where(s => s.IDCate == id).FirstOrDefault();
                db.Categories.Remove(ct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
            }
        }

        //Chi tiết danh mục sản phẩm
        public ActionResult Details(string id)
        {
            return View(db.Categories.Where(s => s.IDCate == id).FirstOrDefault());
        }
    }
}