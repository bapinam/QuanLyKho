using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using QuanLyKho.ApiIntegration.UserApiClient;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.User;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public LoginController(IUserApiClient userApiClient,
            IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // lỡ đăng nhập rồi mà vào trang Login thì:
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.Authenticate(request);

            if (!result.IsSuccessed)
            {
                if (result.Message != null)
                {
                    ViewBag.Login = result.Message;
                    return View();
                }
                ViewBag.Login = "Đăng nhập thất bại";
                return View();
            }
            var userPrincipal = this.ValidateToken(result.ResultObj);

            var remember = false;
            if (request.RememberMe)
            {
                remember = true;
            }
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1),
                IsPersistent = remember
            };

            // token cua api cu nhet vao cookie la dc roi
            // nhet luon cai user_id vao cookie, request tu web call sang api thi lay token ra send, con duoi api thi get ra dc user_id, ko can cai name kia

            // lưu token vào session
            HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageId, _configuration[SystemConstants.AppSettings.DefaultLanguageId]);
            HttpContext.Session.SetString(SystemConstants.AppSettings.Token, result.ResultObj);

            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            return RedirectToAction("Profile", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        // lấy Token và giải mã
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration[SystemConstants.Token.Issuer];
            validationParameters.ValidIssuer = _configuration[SystemConstants.Token.Issuer];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                        .GetBytes(_configuration[SystemConstants.Token.Key]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler()
                                        .ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}