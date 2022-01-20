using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project_TFM10304.Attributes;
using Project_TFM10304.Data;
using Project_TFM10304.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project_TFM10304.Controllers
{
    //權限 & layout設定
    [ViewLayout("_CompanyLayout")]
    [Authorize(Roles = "Company")]
    public class CompanyHomeController : Controller
    {
        private readonly ILogger<CompanyHomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Users> _userManager;

        public CompanyHomeController(ILogger<CompanyHomeController> logger,
            ApplicationDbContext dbContext,
            UserManager<Users> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //後端首頁
            return View();
        }

        //CompanyHome/Index 初始資料-1
        public string ComingOrders(int comingDays)
        {
            ClaimsPrincipal thisUser = this.User;
            var IsCompany = thisUser.IsInRole("Company");

            if(IsCompany == true)
            {
                var userId = thisUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                //join [order, orderDetail, Product], groupby(product.name), 將orderDetail.quantity加總
                var ordersGroups = _dbContext.Order
                    .Join(_dbContext.OrderDetail, o => o.OrderId, od => od.OrderId, (o, od) => new { id = o.OrderId, productId = od.ProductId, quantity = od.Quantity })
                    .Join(_dbContext.Product, od => od.productId, p => p.Id, (od, p) => new { oid = od.id, date = p.StartDate, productName = p.Name, quantity = od.quantity, cid = p.CompanyUserId, price=p.Price })
                    .Where(o => o.cid == userId && (o.date >= DateTime.Now && o.date <= DateTime.Now.AddDays(comingDays)))
                    .GroupBy(o => new { o.productName, o.date, o.price })
                    .Select(g => new {productName = g.Key.productName, date = g.Key.date.ToString("yyyy/MM/dd"), quantity = g.Sum(q => q.quantity), totalPrice = (g.Key.price * g.Sum(q=>q.quantity))});

                var jsonResult = JsonSerializer.Serialize(ordersGroups);
                return jsonResult;
            }
            return "";
        }

        //CompanyHome/Index 初始資料-2
        public string PassedOrders(int passedDays)
        {
            ClaimsPrincipal thisUser = this.User;
            var IsCompany = thisUser.IsInRole("Company");

            if(IsCompany == true)
            {
                var userId = thisUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                //最近完成訂單 x日內完成
                var ordersTask = _dbContext.Order
                    .Join(_dbContext.OrderDetail, o => o.OrderId, od => od.OrderId, (o, od) => new { id = o.OrderId, productId = od.ProductId, quantity = od.Quantity })
                    .Join(_dbContext.Product, od => od.productId, p => p.Id, (od, p) => new { oid = od.id, date = p.StartDate, productName = p.Name, quantity = od.quantity, cid = p.CompanyUserId, price=p.Price })
                    .Where(o => o.cid == userId && o.date <= DateTime.Now && o.date >= DateTime.Now.AddDays(-passedDays))
                    .GroupBy(o => new { o.productName, o.date, o.price })
                    .Select(r => new {productName = r.Key.productName, date = r.Key.date.ToString("yyyy/MM/dd"), quantity = r.Sum(q => q.quantity), totalPrice=(r.Key.price * r.Sum(q=>q.quantity)) });

                //var orders = await ordersTask.OrderBy(r => r.date).ToListAsync();
                var jsonResult = JsonSerializer.Serialize(ordersTask);
                return jsonResult;
            }
            return "";
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
