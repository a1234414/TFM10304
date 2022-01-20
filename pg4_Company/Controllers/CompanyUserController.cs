using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Project_TFM10304.Attributes;
using Project_TFM10304.Data;
using Project_TFM10304.Models;
using Project_TFM10304.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project_TFM10304.Controllers
{
    
    public class CompanyUserController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CompanyUserController(
            RoleManager<IdentityRole> roleManager,
            UserManager<Users> userManager, 
            ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        //註冊為商家 頁面
        [ViewLayout("_Layout")]
        public IActionResult Index()
        {
            return View();
        }

        //接收Index的表單, 新增至資料庫(Users, Company, Roles)
        [HttpPost]
        public async Task<IActionResult> Register(cuCreateViewModel data)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok($"發生錯誤: {errors}");
            }
            var user = new Users { UserName = data.Email, Email = data.Email };
            //新增至Users
            var result = await _userManager.CreateAsync(user, data.Password);
            //新增至Company
            if (result.Succeeded)
            {
                Company c = new Company
                {
                    UserId = user.Id,
                    TaxId = int.Parse(data.TaxId),
                    CompanyName = data.CompanyName,
                    LegalName = data.LegalName,
                    Nation = data.Nation,
                    City = data.City,
                    PostalCode = data.PostalCode,
                    Address = data.Address,
                    ContactNumber = data.ContactNumber
                };

                _dbContext.Company.Add(c);
                _dbContext.SaveChanges();

                //設定role
                if (!await _roleManager.RoleExistsAsync("Company"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Company"));
                }
                await _userManager.AddToRoleAsync(user, "Company");

                return RedirectToAction("Index", "CompanyHome", null);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            //重新導向
            return RedirectToAction("Index", "CompanyUser", null);
        }

        //Company User資料編輯頁
        [Authorize(Roles = "Company")]
        [ViewLayout("_CompanyLayout")]
        public IActionResult Manage()
        {
            return View();
        }

        //取得Manage 初始資料(目前登入user的資料)
        [Authorize(Roles = "Company")]
        public string GetManageDefaultValues()
        {
            ClaimsPrincipal thisUser = this.User;
            var userId = thisUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var currentUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            var query = _dbContext.Company.Where(c => c.UserId == userId)
                .Select(c => new
                {
                    taxId = c.TaxId,
                    companyName = c.CompanyName,
                    legalName = c.LegalName,
                    contactNumber = c.ContactNumber,
                    postalCode = c.PostalCode,
                    nation = c.Nation,
                    city = c.City,
                    address = c.Address
                });

            string jsonResult = JsonSerializer.Serialize(query);
            return jsonResult;
        }

        //接收Manage送出的表單, 更改資料庫內容
        [Authorize(Roles = "Company")]
        public IActionResult Update(cuUpdateViewModel data)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok($"發生錯誤: {errors}");
            }

            ClaimsPrincipal thisUser = this.User;
            var userId = thisUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var targetUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            var targetCompany = _dbContext.Company.FirstOrDefault(c => c.UserId == userId);

            if(targetUser != null)
            {
                targetCompany.TaxId = int.Parse(data.TaxId);
                targetCompany.CompanyName = data.CompanyName;
                targetCompany.LegalName = data.LegalName;
                targetCompany.ContactNumber = data.ContactNumber;
                targetCompany.PostalCode = data.PostalCode;
                targetCompany.Nation = data.Nation;
                targetCompany.City = data.City;
                targetCompany.Address = data.Address;

                _dbContext.SaveChanges();
            }
            return RedirectToAction();
        }
    }
}
