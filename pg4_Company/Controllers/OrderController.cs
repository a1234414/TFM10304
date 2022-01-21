using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_TFM10304.Attributes;
using Project_TFM10304.Data;
using Project_TFM10304.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Project_TFM10304.Controllers
{
    //權限 & layout設定
    [ViewLayout("_CompanyLayout")]
    [Authorize(Roles = "Company")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Users> _userManager;

        public OrderController(ApplicationDbContext dbContext,
            UserManager<Users> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        //歷史訂單
        public IActionResult Index()
        {
            return View();
        }

        //歷史訂單 初始資料
        public string MyOrders()
        {
            ClaimsPrincipal thisUser = this.User;
            var IsCompany = thisUser.IsInRole("Company");

            if (IsCompany == true)
            {
                var userId = thisUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                //join [order, orderDetail, Product], groupby(product.name), 將orderDetail.quantity加總
                var ordersGroups = _dbContext.OrderDetail
                    .Join(_dbContext.Product, od => od.ProductId, p => p.Id, (od, p) =>
                    new
                    {
                        oid = od.OrderId,
                        productName = p.Name,
                        price = p.Price,
                        quantity = od.Quantity,
                        psdate = p.StartDate,
                        pedate = p.EndDate,
                        cid = p.CompanyUserId
                    })
                    .Where(o => o.cid == userId && o.pedate <= DateTime.Now.AddDays(100)).Select(r =>
                    new {
                        oid = r.oid,
                        productName = r.productName,
                        price = r.price,
                        quantity = r.quantity,
                        psdate = r.psdate.ToString("yyyy/MM/dd"),
                        pedate = r.pedate.ToString("yyyy/MM/dd"),
                        totalPrice = r.price * r.quantity
                    });

                var jsonResult = JsonSerializer.Serialize(ordersGroups);
                return jsonResult;
            }
            return "";
        }

        public string GetOrders(string sdate, string edate)
        {
            ClaimsPrincipal thisUser = this.User;
            string userId = thisUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            DateTime dts = (sdate == null) ? DateTime.Parse("2000-01-01") : DateTime.Parse(sdate);
            DateTime dte = (edate == null) ? DateTime.Now.AddDays(100) : DateTime.Parse(edate);

            var query = _dbContext.Order.Join(_dbContext.OrderDetail, o => o.OrderId, od => od.OrderId, (o, od) => new { id = o.OrderId, pid = od.ProductId, qty = od.Quantity })
                .Join(_dbContext.Product, o => o.pid, p => p.Id, (o, p) => new { cid = p.CompanyUserId, oid = o.id, productName = p.Name, price = p.Price, quantity = o.qty, psdate = p.StartDate, pedate = p.EndDate })
                .Where(o => o.cid == userId && o.psdate >= dts && o.pedate <= dte)
                .Select(r => new { oid = r.oid, productName = r.productName, price = r.price, quantity = r.quantity, psdate = r.psdate.ToString("yyyy/MM/dd"), pedate = r.pedate.ToString("yyyy/MM/dd"), totalPrice = r.price * r.quantity });

            return JsonSerializer.Serialize(query);
        }
    }
}
