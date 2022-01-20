using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_TFM10304.Data;
using Project_TFM10304.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace pg4_Company.Controllers
{
    public class tocProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public tocProductController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        //商品列表頁面
        public IActionResult Index()
        {
            return View();
        }

        //抓取存在Session中購物車的ProductId, 搜尋商品訊息
        public List<Product> GetCart()
        {
            var cartList = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartList))
            {
                return new List<Product>();
            }
            
            var data = JsonSerializer.Deserialize<List<int>>(cartList);

            return _dbContext.Product.Where(p => data.Contains(p.Id)).ToList();
        }

        //加入購物車
        [HttpPost]
        public string AddProductToCart([FromForm] int id)
        {
            var cartList = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartList))
            {
                var data = new List<int>();
                data.Add(id);
                var j = JsonSerializer.Serialize(data);
                HttpContext.Session.SetString("Cart", j);
            }
            else
            {
                var data = JsonSerializer.Deserialize<List<int>>(cartList);
                data.Add(id);
                var j = JsonSerializer.Serialize(data);
                HttpContext.Session.SetString("Cart", j);
            }
            return "已加入購物車";
        }

        //自購物車中移除
        [HttpPost]
        public string RemoveItem([FromForm] int id)
        {
            var cartList = HttpContext.Session.GetString("Cart");
            var data = JsonSerializer.Deserialize<List<int>>(cartList);
            data.Remove(id);
            var tempdata = JsonSerializer.Serialize(data);
            HttpContext.Session.SetString("Cart", tempdata);

            return "商品已刪除";
        }
    }
}
