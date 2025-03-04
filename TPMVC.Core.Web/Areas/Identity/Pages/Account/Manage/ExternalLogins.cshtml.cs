// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;

namespace TPMVC.Core.Web.Areas.Identity.Pages.Account.Manage
{
    public class ExternalLoginsModel : PageModel
    {
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly IUserStore<IdentityUser> _userStore;

        //public ExternalLoginsModel(
        //    UserManager<IdentityUser> userManager,
        //    SignInManager<IdentityUser> signInManager,
        //    IUserStore<IdentityUser> userStore)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _userStore = userStore;
        //}
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginsModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool ShowRemoveButton { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

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
            public string Email { get; set; }
        }
        [TempData]
        public string StatusMessage { get; set; }
        public IActionResult OnGet() => RedirectToPage("./Login");


        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
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
    //public IActionResult OnPost(string provider, string returnUrl = null)
    //{
    //    // Request a redirect to the external login provider.
    //    var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
    //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
    //    return new ChallengeResult(provider, properties);
    //}

    //public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    //{
    //    returnUrl = returnUrl ?? Url.Content("~/");
    //    if (remoteError != null)
    //    {
    //        ErrorMessage = $"Error from external provider: {remoteError}";
    //        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
    //    }
    //    var info = await _signInManager.GetExternalLoginInfoAsync();
    //    if (info == null)
    //    {
    //        ErrorMessage = "Error loading external login information.";
    //        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
    //    }

    //    // Sign in the user with this external login provider if the user already has a login.
    //    var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
    //    if (result.Succeeded)
    //    {
    //        _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
    //        return LocalRedirect(returnUrl);
    //    }
    //    if (result.IsLockedOut)
    //    {
    //        return RedirectToPage("./Lockout");
    //    }
    //    else
    //    {
    //        // If the user does not have an account, then ask the user to create an account.
    //        ReturnUrl = returnUrl;
    //        ProviderDisplayName = info.ProviderDisplayName;
    //        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
    //        {
    //            Input = new InputModel
    //            {
    //                Email = info.Principal.FindFirstValue(ClaimTypes.Email)
    //            };
    //        }
    //        return Page();
    //    }
    //}
    //public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
    //{
    //    returnUrl = returnUrl ?? Url.Content("~/");
    //    // Get the information about the user from the external login provider
    //    var info = await _signInManager.GetExternalLoginInfoAsync();
    //    if (info == null)
    //    {
    //        ErrorMessage = "Error loading external login information during confirmation.";
    //        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        var user = CreateUser();

    //        await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
    //        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

    //        var result = await _userManager.CreateAsync(user);
    //        if (result.Succeeded)
    //        {
    //            result = await _userManager.AddLoginAsync(user, info);
    //            if (result.Succeeded)
    //            {
    //                _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

    //                var userId = await _userManager.GetUserIdAsync(user);
    //                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    //                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    //                var callbackUrl = Url.Page(
    //                    "/Account/ConfirmEmail",
    //                    pageHandler: null,
    //                    values: new { area = "Identity", userId = userId, code = code },
    //                    protocol: Request.Scheme);

    //                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
    //                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

    //                // If account confirmation is required, we need to show the link if we don't have a real email sender
    //                if (_userManager.Options.SignIn.RequireConfirmedAccount)
    //                {
    //                    return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
    //                }

    //                await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
    //                return LocalRedirect(returnUrl);
    //            }
    //        }
    //        foreach (var error in result.Errors)
    //        {
    //            ModelState.AddModelError(string.Empty, error.Description);
    //        }
    //    }

    //    ProviderDisplayName = info.ProviderDisplayName;
    //    ReturnUrl = returnUrl;
    //    return Page();
    //}
    //public async Task<IActionResult> OnGetAsync()
    //{
    //    var user = await _userManager.GetUserAsync(User);
    //    if (user == null)
    //    {
    //        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
    //    }

    //    CurrentLogins = await _userManager.GetLoginsAsync(user);
    //    OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
    //        .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
    //        .ToList();

    //    string passwordHash = null;
    //    if (_userStore is IUserPasswordStore<IdentityUser> userPasswordStore)
    //    {
    //        passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted);
    //    }

    //    ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
    //    return Page();
    //}

    //public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
    //{
    //    var user = await _userManager.GetUserAsync(User);
    //    if (user == null)
    //    {
    //        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
    //    }

    //    var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
    //    if (!result.Succeeded)
    //    {
    //        StatusMessage = "The external login was not removed.";
    //        return RedirectToPage();
    //    }

    //    await _signInManager.RefreshSignInAsync(user);
    //    StatusMessage = "The external login was removed.";
    //    return RedirectToPage();
    //}

    //public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
    //{
    //    // Clear the existing external cookie to ensure a clean login process
    //    await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

    //    // Request a redirect to the external login provider to link a login for the current user
    //    var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
    //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
    //    return new ChallengeResult(provider, properties);
    //}

    ////public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    ////{
    ////    returnUrl = returnUrl ?? Url.Content("~/");
    ////    if (remoteError != null)
    ////    {
    ////        ErrorMessage = $"Error from external provider: {remoteError}";
    ////        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
    ////    }
    ////    var info = await _signInManager.GetExternalLoginInfoAsync();
    ////    if (info == null)
    ////    {
    ////        ErrorMessage = "Error loading external login information.";
    ////        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
    ////    }

    ////    // Sign in the user with this external login provider if the user already has a login.
    ////    var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
    ////    if (result.Succeeded)
    ////    {
    ////        _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
    ////        return LocalRedirect(returnUrl);
    ////    }
    ////    if (result.IsLockedOut)
    ////    {
    ////        return RedirectToPage("./Lockout");
    ////    }
    ////    else
    ////    {
    ////        // If the user does not have an account, then ask the user to create an account.
    ////        ReturnUrl = returnUrl;
    ////        ProviderDisplayName = info.ProviderDisplayName;
    ////        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
    ////        {
    ////            Input = new InputModel
    ////            {
    ////                Email = info.Principal.FindFirstValue(ClaimTypes.Email)
    ////            };
    ////        }
    ////        return Page();
    ////    }
    ////}

    //private IUserEmailStore<IdentityUser> GetEmailStore()
    //{
    //    if (!_userManager.SupportsUserEmail)
    //    {
    //        throw new NotSupportedException("The default UI requires a user store with email support.");
    //    }
    //    return (IUserEmailStore<IdentityUser>)_userStore;
    //}

}
