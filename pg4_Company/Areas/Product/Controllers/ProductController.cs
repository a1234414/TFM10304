using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_TFM10304.Data;
using pg4_Company.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace pg4_Company.Areas.Product.Controllers
{
    [Area("Product")]

    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment env;
        private readonly ApplicationDbContext _dbContext;
        public ProductController(IWebHostEnvironment env, ApplicationDbContext db)
        {
            this.env = env;
            this._dbContext = db;
        }


        public IActionResult ProductIndex()
        {
            return View();
        }

        public IActionResult SearchIndex()
        {
            return View();

        }
        public string GetProducts()
        {
            var query = _dbContext.Product.Select(p => new { ProductPic = p.ProductPic.FirstOrDefault(), p.Id, p.Name, p.StockForSale, p.TotalStock, p.Price, p.Description_S, p.Description_L, p.Description_L_1, p.Description_L_2, p.Description_L_3, p.Description_L_4, p.Description_L_5, p.StartDate, p.EndDate, p.Location });
            return JsonSerializer.Serialize(query);
        }

        //[HttpPost]
        //public string SelectProductInfo([FromForm] int id)
        //{
        //    var result = _dbContext.Product.Where(x => x.Id == id).Select(p => new { p.Id, p.Name, p.Stock, p.TotalStock, p.Price, p.Description_S, p.Description_L, p.Product_Description_L_1, p.Product_Description_L_2, p.Product_Description_L_3, p.Product_Description_L_4, p.Product_Description_L_5, p.StartDate, p.EndDate, p.Location, p.PicPath });
        //    var  ProductInfo = JsonSerializer.Serialize(result);
        //    return  ProductInfo;
        //}


        [HttpPost]
        public string SelectProductInfo([FromForm] int id)
        {
            var SelectProduct = HttpContext.Session.GetString("SelectProductInfo");
            var data = new List<int>();
            data.Add(id);
            var j = JsonSerializer.Serialize(data);
            HttpContext.Session.SetString("SelectProductInfo", j);
            return j;
        }

        [HttpPost]
        public string SelectProductCatogory([FromForm] string Location)
        {
            var SelectProductCatogory = HttpContext.Session.GetString("SelectProductCatogory");
            var data = new List<string>();
            data.Add(Location);
            var j = JsonSerializer.Serialize(data);
            HttpContext.Session.SetString("SelectProductCatogory", j);
            return j;
        }

        public string GetSelectProductCatogory()
        {
            //向 Session 取得列表
            var SelectProductCatogory = SessionHelper.
                GetObjectFromJson<List<string>>(HttpContext.Session, "SelectProductCatogory");
            var query = _dbContext.Product.Where(x => SelectProductCatogory.Contains(x.Location)).Select(p => new { ProductPic = p.ProductPic.FirstOrDefault(), p.Id, p.Name, p.StockForSale, p.TotalStock, p.Price, p.Description_S, p.Description_L, p.Description_L_1, p.Description_L_2, p.Description_L_3, p.Description_L_4, p.Description_L_5, p.StartDate, p.EndDate, p.Location });
            return JsonSerializer.Serialize(query);
        }

        public string GetSelectProductPage()
        {
            //向 Session 取得商品列表
            var SelectProductInfo = SessionHelper.
                GetObjectFromJson<List<int>>(HttpContext.Session, "SelectProductInfo");
            var query = _dbContext.Product.Where(x => SelectProductInfo.Contains(x.Id)).Select(p => new { ProductPic = p.ProductPic.FirstOrDefault(), p.Id, p.Name, p.StockForSale, p.TotalStock, p.Price, p.Description_S, p.Description_L, p.Description_L_1, p.Description_L_2, p.Description_L_3, p.Description_L_4, p.Description_L_5, p.StartDate, p.EndDate, p.Location });
            return JsonSerializer.Serialize(query);
        }

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
