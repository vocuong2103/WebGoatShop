using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class LoginController : Controller
    {
        GoatSneakerEntities4 db = new GoatSneakerEntities4();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAcount(Account _taikhoan)
        {
            var check = db.Accounts.Where(s => s.Username == _taikhoan.Username && s.Password == _taikhoan.Password).FirstOrDefault();

            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai thông tin đăng nhập";
                return View("Index");
            }
            else
            {
                if (check.Role == "Admin")
                {
                    Session["Username"] = check.Username;
                    return RedirectToAction("IndexAdmin", "Product");
                }
                else if (check.Role == "User")
                {
                    Session["Username"] = check.Username;
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ViewBag.ErrorInfo = "Bạn không có quyền truy cập vào trang này";
                    return View("Index");
                }

            }
        }
        //Đăng ký
        public ActionResult DangKy()
        {
            return View(new DangKyViewModel { Account = new Account(), Customer = new Customer() });
        }

        [HttpPost]
        public ActionResult RegisterUser(DangKyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tài khoản có tồn tại trong database chưa
                var existingAccount = db.Accounts.FirstOrDefault(s => s.Username == viewModel.Account.Username);
                if (existingAccount == null)
                {
                    // Nếu tài khoản chưa tồn tại, thêm tài khoản mới
                    viewModel.Account.Role = "User"; // Gán role mặc định là "Khach hang"

                    // Lấy mã tài khoản tự động tăng bằng cách lấy mã tài khoản lớn nhất trong database, sau đó tăng lên 1
                    int maxMaTaiKhoan = db.Accounts.Max(t => t.IDAccount);
                    viewModel.Account.IDAccount = maxMaTaiKhoan + 1;

                    // Lấy mã khách hàng tự động tăng bằng cách lấy mã khách hàng lớn nhất trong database, sau đó tăng lên 
                    int maxMaKhachHang = db.Customers.Max(kh => kh.IDCus);
                    viewModel.Customer.IDCus = maxMaKhachHang + 1;

                    // Thêm thông tin khách hàng vào bảng KhachHang
                    viewModel.Customer.IDAccount = viewModel.Account.IDAccount; // Gán mã tài khoản mới cho khách hàng
                    db.Customers.Add(viewModel.Customer); // Thêm khách hàng vào database

                    db.Configuration.ValidateOnSaveEnabled = false; // Tạm thời tắt validate để không bị lỗi khi thêm dữ liệu
                    db.Accounts.Add(viewModel.Account); // Thêm tài khoản vào database
                    db.SaveChanges(); // Lưu thay đổi vào database

                    // Đăng ký thành công, chuyển hướng về trang đăng nhập
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    // Tài khoản đã tồn tại, hiển thị thông báo lỗi
                    ViewBag.ErrorRegister = "Tài khoản đã đăng nhập";
                    return View(viewModel);
                }
            }

            // Model không hợp lệ, hiển thị lại view đăng ký với thông báo lỗi
            return View(viewModel);
        }
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}