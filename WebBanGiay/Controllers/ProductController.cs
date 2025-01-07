using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class ProductController : Controller
    {
        GoatSneakerEntities4 db = new GoatSneakerEntities4();
        // GET: Product
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
        public ActionResult Details(int? id)
        {
            return View(db.Products.Where(s => s.IDProduct == id).FirstOrDefault());
        }
        public ActionResult DetailsAdmin(int? id)
        {
            return View(db.Products.Where(s => s.IDProduct == id).FirstOrDefault());
        }
        public ActionResult IndexAdmin()
        {
            return View(db.Products.ToList());
        }
        //ThemSanPham
        public ActionResult Create()
        {
            Product sp = new Product();
            return View(sp);
        }
        [HttpPost]
        public ActionResult Create(Product sp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = Path.GetFileNameWithoutExtension(sp.UploadImage.FileName);
                    string extension = Path.GetExtension(sp.UploadImage.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    sp.ImagePro = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    sp.UploadImage.SaveAs(fileName);
                    using (GoatSneakerEntities4 db = new GoatSneakerEntities4())
                    {
                        db.Products.Add(sp);
                        db.SaveChanges();

                    }
                    ModelState.Clear();
                    return RedirectToAction("IndexAdmin");
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
            return View(sp);

        }
        //ChinhSuaSanPham
        public ActionResult Edit(int? id)
        {
            return View(db.Products.Where(s => s.IDProduct == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int? id, Product sp)
        {
            db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }
        public ActionResult Delete(int id)
        {
            return View(db.Products.Where(s => s.IDProduct == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Product pro)
        {
            try
            {
                pro = db.Products.Where(s => s.IDProduct == id).FirstOrDefault();
                db.Products.Remove(pro);
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            catch
            {
                return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
            }
        }
        public ActionResult SelectCate()
        {
            Category se_cate = new Category();
            se_cate.ListCate = db.Categories.ToList<Category>();
            return PartialView(se_cate);
        }
        public ActionResult Search(string searchString)
        {
            var products = db.Products.Where(p => p.NamePro.Contains(searchString)).ToList();
            return View("SearchResults", products);
        }
    }
}