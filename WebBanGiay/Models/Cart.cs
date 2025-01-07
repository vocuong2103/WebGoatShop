using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanGiay.Models
{
    public class Cart
    {
        public int ID { get; set; }
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

        public void Add_Product_Cart(Product _pro, int _quan = 1)
        {
            var item = Items.FirstOrDefault(s => s._product.IDProduct == _pro.IDProduct);
            if (item == null)
                items.Add(new CartItem
                {
                    _product = _pro,
                    _quantity = _quan
                });
            else
                item._quantity += _quan;
        }
        public int Total_quantity()
        {
            return items.Sum(s => s._quantity);
        }
        public double Total_money()
        {
            var total = items.Sum(s => s._quantity * s._product.Price);
            return (double)total;
        }

        //Cập nhật số lượng sản phẩm

        //Cập nhật số lượng sản phẩm

        public void Update_quantity(int id, int _new_quan)
        {
            var item = items.Find(s => s._product.IDProduct == id);
            if (item != null)
            {
                if (_new_quan > 0)
                {
                    item._quantity = _new_quan;
                    // Tính lại tổng tiền sau khi cập nhật số lượng sản phẩm
                    // Số lượng mới * giá sản phẩm
                    item._total = (double)(item._quantity * item._product.Price);
                }
            }
        }



        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s => s._product.IDProduct == id);
        }
        public void ClearCart()
        {
            items.Clear();
        }


    }
}