// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Marginean_Silviu_Lab2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Marginean_Silviu_Lab2.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private readonly Marginean_Silviu_Lab2.Data.Marginean_Silviu_Lab2Context
_context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            Marginean_Silviu_Lab2.Data.Marginean_Silviu_Lab2Context context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 

        [BindProperty]
        public Member Member { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        //        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //        {
        //returnUrl ??= Url.Content("~/");
        //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        //if (ModelState.IsValid)
        //{
        //    var user = CreateUser();

        //    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        //    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
        //    var result = await _userManager.CreateAsync(user, Input.Password);

        //    if (result.Succeeded)
        //    {
        //        _logger.LogInformation("User created a new account with password.");

        //        var userId = await _userManager.GetUserIdAsync(user);
        //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //        var callbackUrl = Url.Page(
        //            "/Account/ConfirmEmail",
        //            pageHandler: null,
        //            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
        //            protocol: Request.Scheme);

        //        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        //            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //        if (_userManager.Options.SignIn.RequireConfirmedAccount)
        //        {
        //            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
        //        }
        //        else
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return LocalRedirect(returnUrl);
        //        }
        //    }
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }
        //}

        //// If we got this far, something failed, redisplay form
        //return Page();

        //        }

        // COD CORECTAT CICA
        // public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //{
        //    returnUrl ??= Url.Content("~/");

        //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        //    // 1. Verificare: Asigură-te că datele de intrare sunt valide (parola, email, etc.)
        //    if (ModelState.IsValid)
        //    {
        //        var user = CreateUser();

        //        await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        //        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

        //        // Încearcă să creezi utilizatorul Identity
        //        var result = await _userManager.CreateAsync(user, Input.Password);

        //        // 2. Verificare: Identity User a fost creat cu succes?
        //        if (result.Succeeded)
        //        {
        //            // 3. Salvează membrul doar DUPĂ ce Identity User a reușit
        //            Member.Email = Input.Email;
        //            _context.Member.Add(Member);
        //            await _context.SaveChangesAsync();

        //            _logger.LogInformation("User created a new account with password.");

        //            var userId = await _userManager.GetUserIdAsync(user);
        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        //            var callbackUrl = Url.Page(
        //                "/Account/ConfirmEmail",
        //                pageHandler: null,
        //                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
        //                protocol: Request.Scheme);

        //            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        //                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //            if (_userManager.Options.SignIn.RequireConfirmedAccount)
        //            {
        //                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
        //            }
        //            else
        //            {
        //                // Loghează utilizatorul imediat (deoarece RequireConfirmedAccount = false în Program.cs)
        //                await _signInManager.SignInAsync(user, isPersistent: false);
        //                return LocalRedirect(returnUrl);
        //            }
        //        }

        //        // 4. Dacă Identity User eșuează (parolă slabă, email duplicat, etc.), adaugă erorile
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    // Dacă ajungem aici (ModelState invalid sau Identity eșuează), ne întoarcem la formular
        //    return Page();
        //}
        //private IdentityUser CreateUser()
        //{
        //    try
        //    {
        //        return Activator.CreateInstance<IdentityUser>();
        //    }
        //    catch
        //    {
        //        throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
        //            $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
        //            $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        //    }
        //}

        //private IUserEmailStore<IdentityUser> GetEmailStore()
        //{
        //    if (!_userManager.SupportsUserEmail)
        //    {
        //        throw new NotSupportedException("The default UI requires a user store with email support.");
        //    }
        //    return (IUserEmailStore<IdentityUser>)_userStore;
        //}


        //public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //{
        //    returnUrl ??= Url.Content("~/"); // 353

        //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(); // 352

        //    var user = CreateUser(); // 354

        //    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None); // 355, 356
        //    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None); // 357, 358

        //    var result = await _userManager.CreateAsync(user, Input.Password); // 359, 360

        //    Member.Email = Input.Email; // 361
        //    _context.Member.Add(Member); // 362
        //    await _context.SaveChangesAsync(); // 363

        //    if (result.Succeeded) // 364
        //    {
        //        _logger.LogInformation("User created a new account with password."); // 367

        //        var role = await _userManager.AddToRoleAsync(user, "User");
        //        var userId = await _userManager.GetUserIdAsync(user); // 368
        //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user); // 369

        //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)); // 370, 371

        //        var callbackUrl = Url.Page( // 372
        //            "/Account/ConfirmEmail", // 373
        //            pageHandler: null, // 373
        //            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl }, // 375
        //            protocol: Request.Scheme); // 376

        //        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", // 378
        //            $"Please confirm your account by clicking here."); // 379, 380

        //        if (_userManager.Options.SignIn.RequireConfirmedAccount) // 383
        //        {
        //            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl }); // 384, 385
        //        }
        //        else
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false); // 388, 387
        //            return LocalRedirect(returnUrl); // 389
        //        }
        //    }

        //    // Logica pentru afișarea erorilor în caz de eșec (Pasul 20 nu includea această parte, dar este necesară)
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }

        //    return Page(); // 392


        //}

        //private IdentityUser CreateUser()
        //{
        //    try
        //    {
        //        return Activator.CreateInstance<IdentityUser>();
        //    }
        //    catch
        //    {
        //        throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
        //            $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
        //            $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        //    }
        //}
        //private IUserEmailStore<IdentityUser> GetEmailStore()
        //{
        //    if (!_userManager.SupportsUserEmail)
        //    {
        //        throw new NotSupportedException("The default UI requires a user store with email support.");
        //    }
        //    return (IUserEmailStore<IdentityUser>)_userStore;
        //}

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // 1. Încercăm să creăm utilizatorul Identity
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);

                // Dacă Identity User a fost creat cu SUCCES
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Adaugă utilizatorul în rolul "User"
                    await _userManager.AddToRoleAsync(user, "User");

                    // 2. SALVAREA MODELULUI MEMBER (ACUM este ordinea corectă)
                    try
                    {
                        // Deoarece alte câmpuri (FirstName, LastName, etc.) nu vin din formularul standard,
                        // le setăm ca fiind null sau goale, PENTRU A EVITA EROAREA [Required].

                        // Asigurăm că Email-ul din Member este setat
                        Member.Email = Input.Email;

                        // Dacă nu aveți câmpurile Member în formular, probabil Member este NULL
                        // Soluție rapidă: Inițializăm un membru nou, punem doar email-ul
                        var newMember = new Member { Email = Input.Email };

                        // Dacă Member are [Required] pentru alte câmpuri, această secțiune va trebui ajustată.
                        _context.Member.Add(newMember);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        // Aceasta prinde eroarea de validare a modelului Member!
                        _logger.LogError($"Eroare la salvarea modelului Member pentru {Input.Email}: {ex.Message}");
                        // ATENȚIE: Dacă salvarea Member eșuează, utilizatorul Identity a fost deja creat!
                        // Pentru curățenie, ar trebui șters (sau lăsat și investigat manual).
                        // Momentan, doar afișăm eroarea.
                        ModelState.AddModelError(string.Empty, "Eroare la salvarea detaliilor membrului în baza de date.");
                        await _userManager.DeleteAsync(user); // Șterge utilizatorul Identity creat anterior
                        return Page();
                    }


                    // 3. Loghează utilizatorul și redirecționează
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                // 4. Dacă Identity User eșuează (parolă slabă, email duplicat, etc.), adaugă erorile
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Dacă ajungem aici (ModelState invalid sau Identity/Member eșuează), reafișăm formularul
            return Page();
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
