using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Web.ClientServices;
using System.Web.ClientServices.Providers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace MVCChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var userData = new UserData("张三1", "123",new ClientFormsAuthenticationMembershipProvider(),"authenticationType123",true,new CookieContainer());
            var userData = new UserData("张三", "123");

            var jss = new JavaScriptSerializer();

            var logonInfo = jss.Serialize(userData);

            //设置Ticket信息
            var ticket = new FormsAuthenticationTicket(1, "张三", DateTime.Now,
                                                       DateTime.Now.Add(FormsAuthentication.Timeout), false,
                                                       logonInfo);
            //加密验证票据
            var strTicket = FormsAuthentication.Encrypt(ticket);
            AddCookie(Request, FormsAuthentication.FormsCookieName, strTicket, FormsAuthentication.CookieDomain, DateTime.Now.Add(FormsAuthentication.Timeout));
            AddCookie(Response, FormsAuthentication.FormsCookieName, strTicket, FormsAuthentication.CookieDomain, DateTime.Now.Add(FormsAuthentication.Timeout));

            //Request.RequestContext.HttpContext.User = new ClientRolePrincipal(userData);

            //FormsAuthentication.SetAuthCookie("aaa张三", true);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            var user = Request.RequestContext.HttpContext.User;
            var formIdentiry = user.Identity as FormsIdentity;
            ViewBag.Name = formIdentiry.Name;
            ViewBag.RoleClaimType = formIdentiry.RoleClaimType;
            ViewBag.NameClaimType = formIdentiry.NameClaimType;
            ViewBag.Ticket = formIdentiry.Ticket;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ViewResult Chat()
        {
            return View();
        }


        /// <summary>
        /// 添加cookie
        /// </summary>
        /// <param name="request">Http响应</param>
        /// <param name="cookieName">cookieName</param>
        /// <param name="value">cookieValue</param>
        /// <param name="cookieDomain">cookie关联的域</param>
        /// <param name="expires">cookie有效期</param>
        /// <param name="isSetExpires">是否设置有效期</param>
        /// <param name="Path">要与当前 Cookie 一起传输的虚拟路径</param>
        /// <param name="HttpOnly"> Cookie 是否可通过客户端脚本访问 true代表客户端只能读，不能写。只有服务端可写，防止被篡改</param>
        /// <param name="Values">Cookie 对象所包含的键值对的集合</param>
        /// <param name="Secure">是否使用安全套接字层 (SSL)（即仅通过 HTTPS）传输 Cookie</param>
        /// <param name="Shareable">确定 cookie 是否允许参与输出缓存</param>
        public void AddCookie(HttpRequestBase request, string cookieName, string value, string cookieDomain, DateTime expires
            , bool isSetExpires = true, string Path = "/", bool HttpOnly = true, NameValueCollection Values = null, bool Secure = false, bool Shareable = false)
        {
            var cookie = request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = value;
            cookie.Domain = cookieDomain;
            cookie.Path = Path;
            cookie.Secure = Secure;
            cookie.Shareable = Shareable;
            //true代表客户端只能读，不能写。只有服务端可写，防止被篡改
            cookie.HttpOnly = HttpOnly;

            if (Values != null)
                cookie.Values.Add(Values);

            if (isSetExpires)
            {
                cookie.Expires = expires;
            }
            request.Cookies.Add(cookie);
        }
        /// <summary>
        /// 添加cookie
        /// </summary>
        /// <param name="response">Http响应</param>
        /// <param name="cookieName">cookieName</param>
        /// <param name="value">cookieValue</param>
        /// <param name="cookieDomain">cookie关联的域</param>
        /// <param name="expires">cookie有效期</param>
        /// <param name="isSetExpires">是否设置有效期</param>
        /// <param name="Path">要与当前 Cookie 一起传输的虚拟路径</param>
        /// <param name="HttpOnly"> Cookie 是否可通过客户端脚本访问 true代表客户端只能读，不能写。只有服务端可写，防止被篡改</param>
        /// <param name="Values">Cookie 对象所包含的键值对的集合</param>
        /// <param name="Secure">是否使用安全套接字层 (SSL)（即仅通过 HTTPS）传输 Cookie</param>
        /// <param name="Shareable">确定 cookie 是否允许参与输出缓存</param>
        public void AddCookie(HttpResponseBase response, string cookieName, string value, string cookieDomain, DateTime expires
            , bool isSetExpires = true, string Path = "/", bool HttpOnly = true, NameValueCollection Values = null, bool Secure = false, bool Shareable = false)
        {
            var cookie = response.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = value;
            cookie.Domain = cookieDomain;
            cookie.Path = Path;
            cookie.Secure = Secure;
            cookie.Shareable = Shareable;
            //true代表客户端只能读，不能写。只有服务端可写，防止被篡改
            cookie.HttpOnly = HttpOnly;

            if (Values != null)
                cookie.Values.Add(Values);

            if (isSetExpires)
            {
                cookie.Expires = expires;
            }

            response.AppendCookie(cookie);
        }

        //public class UserData : ClientFormsIdentity
        //{
        //    public UserData(string name, string password, MembershipProvider provider, string authenticationType, bool isAuthenticated, CookieContainer authenticationCookies) : base(name, password, provider, authenticationType, isAuthenticated, authenticationCookies)
        //    {
        //    }
        //}
        public class UserData
        {
            public string Name { get; set; }
            public string Password { get; set; }

            public UserData(string name, string password)
            {
                Name = name;
                Password = password;
            }
        }
    }
}