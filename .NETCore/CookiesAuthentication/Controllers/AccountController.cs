using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookiesAuthentication.Controllers
{
    public class AccountController:Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl=null){
            ViewData["ReturnUrl"]=returnUrl;
            return View();
        }

        public bool ValidateLogin(string userName,string password){
            // 所有的登陆都通过
            return true;
        }

        public async Task<IActionResult> Login(string userName,string password,string returnUrl=null){
            ViewData["ReturnUrl"]=returnUrl;

            if(ValidateLogin(userName,password)){
                var claims = new List<Claim>(){
                    new Claim("user",userName),
                    new Claim("role","Member")
                };
            
                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims,"Cookies","user","role")));

                if(Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return Redirect("/");
            }
            return View();
        }

        public IActionResult AccessDenied(string returnUrl=null){
            return View();
        }

        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}