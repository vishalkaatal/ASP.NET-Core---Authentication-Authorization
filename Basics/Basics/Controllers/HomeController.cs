using Basics.CustomPolicyProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Policy="Claim.DoB")]
        
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }

        [SecurityLevel(5)]
        public IActionResult SecretLevel()
        {
            return View("Secret");
        }

        [SecurityLevel(10)]
        public IActionResult SecretHigherLevel()
        {
            return View("Secret");
        }



        [AllowAnonymous]
        public IActionResult Authenticate()
        {
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Yugal"),
                new Claim(ClaimTypes.Email,"Yugal@gmail.com"),
                new Claim(ClaimTypes.DateOfBirth,"24/07/1999"),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Role,"AdminTwo"),
                new Claim(DynamicPolicies.SecurityLevel,"7"),
                new Claim("Says"," hi Yugal"),

            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Yugal Desai"),

                new Claim("Says"," A+"),

            };
            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma claims");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government claims");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });
            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DoStuff(
            [FromServices] IAuthorizationService authorizationService)
        {

            var builder =new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();

            var authResult = await authorizationService.AuthorizeAsync(User, customPolicy);

            if (authResult.Succeeded)
            {
                return View("Index");

            }
            
            return View("Index");
        }


    }
}
