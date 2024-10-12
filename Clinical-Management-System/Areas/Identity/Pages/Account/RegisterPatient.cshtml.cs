// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Clinical_Management_System.Models;
using Clinical_Management_System.Models.DB_Entities;
using Clinical_Management_System.Utitlity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace Clinical_Management_System.Areas.Identity.Pages.Account
{
	public class RegisterPatientModel : PageModel
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IUserStore<IdentityUser> _userStore;
		private readonly IUserEmailStore<IdentityUser> _emailStore;
		private readonly ILogger<RegisterPatientModel> _logger;
		private readonly IEmailSender _emailSender;

		public RegisterPatientModel(
			UserManager<IdentityUser> userManager,
			IUserStore<IdentityUser> userStore,
			SignInManager<IdentityUser> signInManager,
			ILogger<RegisterPatientModel> logger,
			IEmailSender emailSender,
			RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_userStore = userStore;
			_emailStore = GetEmailStore();
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
			_roleManager = roleManager;
		}

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; } = new InputModel();

		
		public string ReturnUrl { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

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
            [Required]
            [StringLength(14, ErrorMessage = "National Id must be 14 characters", MinimumLength = 14)]
            public string NationalId { get; set; } = string.Empty;
            [Required]
            [MaxLength(100, ErrorMessage = "Username cannot be greater than 100 charcters")]
            public string UserName { get; set; } = string.Empty;
            [Required]
            [MaxLength(100, ErrorMessage = "First name cannot be greater than 100 charcters")]
            public string FirstName { get; set; } = string.Empty;
            [Required]
            [MaxLength(100, ErrorMessage = "Last name cannot be greater than 100 charcters")]
            public string LastName { get; set; } = string.Empty;
            [Required]
            [MaxLength(100, ErrorMessage = "City cannot be greater than 100 charcters")]
            public string City { get; set; } = string.Empty;
            [Required]
            [MaxLength(100, ErrorMessage = "Government cannot be greater than 100 charcters")]
            public string Government { get; set; } = string.Empty;
            [Required]
            [MaxLength(100, ErrorMessage = "Street name cannot be greater than 100 charcters")]
            public string StreetName { get; set; } = string.Empty;

            [Required]
            public int Floor { get; set; }

            [Required]
            public int BuildingNum { get; set; }

            public byte[]? Photo { get; set; }
            public ICollection<Document>? Documents { get; set; }
            public ICollection<Allergy>? Allergies { get; set; }
            public ICollection<ChronicDisease>? ChronicDiseases { get; set; }
            public ICollection<Appointment>? Appointments { get; set; }


        }


		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			if (!await _roleManager.RoleExistsAsync(Sd.Role_Patient))
			{
				await _roleManager.CreateAsync(new IdentityRole(Sd.Role_Patient));
			}
			}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (ModelState.IsValid)
			{
				var user = CreateUser();
				user.Allergies = Input.Allergies;
				user.Appointments = Input.Appointments;
				user.City = Input.City;
				user.StreetName = Input.StreetName;
				user.Photo = Input.Photo;
				user.UserName = Input.UserName;
				user.Email = Input.Email;
				user.FirstName = Input.FirstName;
				user.LastName = Input.LastName;
				user.ChronicDiseases = Input.ChronicDiseases;	
				user.Documents = Input.Documents;
				user.BuildingNum=Input.BuildingNum;
				user.Floor=Input.Floor;
				


				await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
				await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
					_logger.LogInformation("User created a new account with password.");

					await _userManager.AddToRoleAsync(user, Sd.Role_Patient);

					var userId = await _userManager.GetUserIdAsync(user);
					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
					var callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
						protocol: Request.Scheme);

					await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
						$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

					if (_userManager.Options.SignIn.RequireConfirmedAccount)
					{
						return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
					}
					else
					{
						await _signInManager.SignInAsync(user, isPersistent: false);
						return LocalRedirect(returnUrl);
					}
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}

		private Patient CreateUser()
		{
			try
			{
				return Activator.CreateInstance<Patient>();
			}
			catch
			{
				throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
					$"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
					$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
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
}
