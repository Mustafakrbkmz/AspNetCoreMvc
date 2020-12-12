using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvc2.Introduction.Identity;
using AspNetCoreMvc2.Introduction.Models.Security;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace AspNetCoreMvc2.Introduction.Controllers
{
    public class SecurityController : Controller
    {
        private UserManager<AppIdentityUser> _userManager; // Kullanıcı ile ilgili işlemleri yapmak için kullanacağız.
        private SignInManager<AppIdentityUser> _signInManager; // Login, logout işlemleri yapmak için kullanacağız.
        public SecurityController(UserManager<AppIdentityUser> userManager,
            SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) // Neden asenkron kullandığımızın açıklaması http://www.borakasmer.com/mvc-entityframework-asenkron-programlama/
        {
            if (ModelState.IsValid)
            {
                return View(loginViewModel); // kullanıcı istenilen bilgileri girmemişse geriye aynı viewmodel' i döndürsün.
            }

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user != null) //Böyle bir kullanıcı var mı?
            {
                if (!await _userManager.IsEmailConfirmedAsync(user)) // Böyle bir kullanıcı varsa confirmed etmiş mi/ kullanıcıyı onaylamış mı?
                {
                    ModelState.AddModelError(String.Empty, "Öncelikle mail adresinizden kullanıcızı confirmed ediniz");
                    return View(loginViewModel);
                }
            }
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false,
                false); //ilk false değeri beni hatırla için, ikinci false startup da yazdığımız kullanıcı 5 defa üst üste hatalı giriş
            // yaparsa blocklanması için.
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Student");
            }
            ModelState.AddModelError(String.Empty, "Login Failed");
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Student"); // Kullanıcı çıkış yaptıgında yönlendirileceği sayfa
        }

        public IActionResult AccessDenied() // Kullanıcı yetkisiz sayfaya girmek istediğinde çalıştıracağımız action.
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            var user = new AppIdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                Age = registerViewModel.Age
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                var confirmationCode = _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackUrl = Url.Action("ConfirmEmail", "Security", new { userId = user.Id, code = confirmationCode.Result });

                //Send Email

                return RedirectToAction("Index", "Student");
            }
            return View(registerViewModel);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Student");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException("Kullanıcı bulunamadı");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            return RedirectToAction("Index", "Student");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email)) //email kısmı boş ise kullanıcıya view döndür.
            {
                return View(); // Burda mail adresini girmelisin diye bir view dönderebiliriz.
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return View(); // Böyle bir kullanıcı yok.
            }
            var confirmationCode = await _userManager.GeneratePasswordResetTokenAsync(user); //Şifre sıfırlamak için token oluşturuyoruz.
            var callBackUrl = Url.Action("ResetPassword", "Security",
                new { userId = user.Id, code = confirmationCode });
            //send callback url with email
            return RedirectToAction("ForgotPasswordEmailSent");
        }
        public IActionResult ForgotPasswordEmailSent()
        {
            return View();
        }

        public IActionResult ResetPassword(string userId, string code)
        {
            if (userId == null || code == null)
            {
                throw new ApplicationException("Şifreyi sıfırlamak için istenen kodu sağlamalısınız");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user==null)
            {
                throw new ApplicationException("User not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code, resetPasswordViewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirm");
            }
            return View();
        }

        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }
    }
}