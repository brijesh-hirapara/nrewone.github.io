
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Errors.Model;
using SparkleWeb.model.Account;
using SparkleWeb.model.Common;
using SparkleWeb.model.DataContext;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SparkleWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _context;
        private readonly IMessageService _messageService;
        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IMessageService messageService, ApplicationDbContext context)
        {
            _configuration = configuration;
            _messageService = messageService;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        #region Registration 
        [HttpPost]
        [Route("Register")]
        public async Task<RegistrationResult> Register([FromBody] Registration model)
        {


            var userExist = await _userManager.FindByEmailAsync(model.Emailid);
            if (userExist != null)
                return new RegistrationResult
                {
                    Messsage = "Error",

                };
            //var EmailExist = await _userManager.(model.Email);
            //if (EmailExist != null)
            //    return new RegistrationResult
            //    {
            //        Messsage = "Error",

            //    };
            if (model == null)
                throw new Exception("Command is required");

            if (string.IsNullOrEmpty(model.Name))
                throw new ArgumentNullException("Name is required");

            if (model.MobileNo == null)
                throw new ArgumentNullException("MobileNo is required");

            if (string.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException("Password is required");
            if (string.IsNullOrEmpty(model.Status))
                throw new ArgumentNullException("Status is required");

            if (string.IsNullOrEmpty(model.Emailid))
                throw new ArgumentNullException("Emailid is required");
            model.CreatedDate = DateTime.Now;
            var newUser = new ApplicationUser
            {
                UserName = model.Name,
                PhoneNumber = model.MobileNo,
                Email = model.Emailid,
                Status = model.Status,
                CreatedDate = model.CreatedDate,
                UpdatedDate = model.UpdatedDate

            };

            var userCreationResult = await _userManager.CreateAsync(newUser, model.Password);

            if (!userCreationResult.Succeeded)
                throw new Exception(userCreationResult.Errors.First().Description);

            //This is used for AspNetRoles.
            string[] roles = new string[] { "Admin", "SuparAdmin" };

            //foreach (string role in roles)
            //{
            //    var roleStore = new RoleStore<IdentityRole>(_context);

            //    if (!_context.Roles.Any(r => r.Name == role))
            //    {
            //        await roleStore.CreateAsync(new IdentityRole(role));
            //    }
            //}
            var roleStore = new RoleStore<IdentityRole>(_context);
            foreach (string role in roles)
            {
                if (!_context.Roles.Any(r => r.Name == role))
                {
                    var nr = new IdentityRole(role);
                    nr.NormalizedName = role.ToUpper();
                    await roleStore.CreateAsync(nr);
                }
            }
            await _userManager.AddToRoleAsync(newUser, UserRole.SuparAdmin);


            //string code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            //var encodedCode = HttpUtility.UrlEncode(code).Replace('%', '-');

            //var callbackUrl = GetVerifyEmailRoute(HttpUtility.UrlEncode(newUser.Id), encodedCode);

            //var resetPasswordContent = EmailTemplate.VerifyEmailTemplate(callbackUrl);

            //var sendMessageResult = _messageService.SendEmailAsync(newUser.Email, "Verify your email", resetPasswordContent);

            //_telemetryClient.TrackEvent($"New user: {command.Email}");

            return new RegistrationResult
            {
                Messsage = $"{model.Emailid} registered succesfully",
                ResponseStatusCode = System.Net.HttpStatusCode.Created
            };


        }
        #endregion

        #region Login 
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {

            var user = await _userManager.FindByEmailAsync(model.Emailid);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, userRole));
                }
                //token generate
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                      issuer: _configuration["JWT:ValidIssuer"],
                      audience: _configuration["JWT:ValidAudience"],
                      expires: DateTime.Now.AddHours(3),
                      claims: authClaim,
                      signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                      );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = user.UserName,
                    email = user.Email,
                    userRoles
                });
            }
            return Unauthorized();
        }

        #endregion
        #region Logout 
        [HttpPost]
        [Route("Logout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<BaseResult> Logout()
        {

            Request.Headers.TryGetValue(Constants.AuthorizationKey, out var accessToken);

            if (StringValues.IsNullOrEmpty(accessToken))
            {
                //return StatusCode (new Response { Status = "Error", Massege = "Logout Fail"});
                return new BaseResult
                {

                    ResponseStatusCode = System.Net.HttpStatusCode.Unauthorized
                };
            }
            var token = accessToken.FirstOrDefault();

            token = token.Substring(token.IndexOf(" ", StringComparison.InvariantCulture) + 1);

            await _signInManager.SignOutAsync(); // We are not using cookie based auth now?

            // Delete the access token
            //await _authRepository.RemoveAccessTokenAsync(token);

            return new BaseResult
            {
                Messsage = "You've been logged out successfully",
                ResponseStatusCode = System.Net.HttpStatusCode.OK

            };

        }
        #endregion

        #region ChangePassword 
        [HttpPost]
        [Route("changepassword")]
        public async Task<BaseResult> ChangePassword(ChangePassword password)
        {

            if (string.IsNullOrEmpty(password.newpassword))
                throw new ArgumentNullException("password is required");

            if (string.IsNullOrEmpty(password.oldpassword))
                throw new ArgumentNullException("old password is required");
            if (string.IsNullOrEmpty(password.Email))
                throw new ArgumentNullException(" Email is required");
            var user = await _userManager.FindByEmailAsync(password.Email);

            if (user == null)
                throw new InvalidOperationException();

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, password.oldpassword, password.newpassword);

            if (!changePasswordResult.Succeeded)
                throw new Exception(changePasswordResult.Errors.First().Description);

            await _signInManager.RefreshSignInAsync(user);
            return new BaseResult
            {
                Messsage = "Password Changed Successfully.",
                ResponseStatusCode = System.Net.HttpStatusCode.OK
            };
        }
        #endregion

        #region ResetPassword 
        [HttpGet]
        [Route("resetpassword/{email}")]
        public async Task<BaseResult> ResetPassword(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    throw new ArgumentNullException("email is required");

                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    throw new Exception("Enter valid email.");

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                if (token == null)
                    throw new Exception();

                var callbackUrl = GetResetPasswordEmailRoute(token, user.Email);
                var resetPasswordContent = EmailTemplate.ResetPasswordEmailTemplate(callbackUrl);

                var sendMessageResult = _messageService.SendEmailAsync(user.Email, "Password reset email", resetPasswordContent);

                return new BaseResult
                {
                    Messsage = "Reset Password Email sent Successfully.",
                    ResponseStatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                //_telemetryClient.TrackException(ex);

                return new BaseResult
                {
                    Errors = new List<string> { ex.Message },
                    ResponseStatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }
        #endregion


        #region VerifyPassword 
        [HttpPost]
        [Route("verifypassword")]
        public async Task<BaseResult> VerifyPassword([FromBody] ResetPasswordCommand command)
        {
            try
            {
                if (string.IsNullOrEmpty(command.newPassword))
                    throw new ArgumentNullException("newPassword is required");

                if (string.IsNullOrEmpty(command.token))
                    throw new ArgumentNullException("token is required");

                if (string.IsNullOrEmpty(command.email))
                    throw new ArgumentNullException("email is required");

                var user = await _userManager.FindByEmailAsync(command.email);

                if (user == null)
                    throw new InvalidOperationException();

                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, command.token, command.newPassword);

                if (!resetPasswordResult.Succeeded)
                    throw new Exception(resetPasswordResult.Errors.First().Description);

                return new BaseResult
                {
                    Messsage = "Password Changed Successfully.",
                    ResponseStatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                //_telemetryClient.TrackException(ex);

                return new BaseResult
                {
                    Errors = new List<string> { ex.Message },
                    ResponseStatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion

        #region VerifyEmail 
        [HttpGet]
        [Route("verify/{id}")]
        public async Task<BaseResult> VerifyEmail(string id, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentNullException("Id is required");

                if (string.IsNullOrEmpty(token))
                    throw new ArgumentNullException("Token is required");

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    throw new InvalidOperationException();

                // we replace % when encoding the token value and so we should replace the - with a % back.
                token = token.Replace('-', '%');

                // httputlity will replace + with a blank space when encoded, explicitly replace the empty space with a +
                var decodedToken = HttpUtility.UrlDecode(token).Replace(' ', '+');

                var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, decodedToken);

                var redirectUrl = _configuration.GetValue<string>(Constants.PortalUrlKey);

                if (!emailConfirmationResult.Succeeded)
                    throw new Exception($"{user.Email} failed to verify");

                return new BaseResult
                {
                    Messsage = $"{user.Email} verified",
                    ResponseStatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                //_telemetryClient.TrackException(ex);

                return new BaseResult
                {
                    Errors = new List<string> { ex.Message },
                    ResponseStatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }
        #endregion
        private string GetVerifyEmailRoute(string id, string token)
        {
            var scheme = Request.Scheme;
            var host = Request.Host;

            return $"{scheme}://{host}/Account/verify/{id}?token={token}";
        }
        private string GetResetPasswordEmailRoute(string token, string Email)
        {
            var scheme = "http";
            var host = "localhost:4200";

            return $"{scheme}://{host}/account/verifypassword?email={Email}&token={token}";
        }

        internal string GetUserByClaimType(string claimType)
        {
            try
            {
                var claimResult = User.Claims.FirstOrDefault(c => c.Type.Equals(claimType));

                if (claimResult == null)
                    throw new UnauthorizedException("Invalid request");

                if (string.IsNullOrEmpty(claimResult.Value))
                    throw new UnauthorizedException("Invalid request");

                return claimResult.Value;
            }
            catch (UnauthorizedException ex)
            {
                Console.WriteLine(ex.Message);
                throw new UnauthorizedException("Unauthorized");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        #region Contactus Api
        [HttpPost]
        [Route("contact-us")]
        public async Task<BaseResult> ContactUs(ContactUs contactUs)
        {
            try
            {
                var ContactUsContent = EmailTemplate.ContactUsEmailTemplate(contactUs);

                var sendMessageResult = _messageService.SendEmailAsync(contactUs.EmailAddress, "Contact Us", ContactUsContent);

                return new BaseResult
                {
                    Messsage = "Thank you, we will contact you shortly.",
                    ResponseStatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResult
                {
                    Errors = new List<string> { ex.Message },
                    ResponseStatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }
        #endregion

        //[HttpPost]
        //[Route("Upload")]
        //public async Task<BaseResult> Upload()
        //{
        //    try
        //    {

        //        string imageName = null;
        //        var httpRequest = HttpContext.Current.Request;
        //        //Upload Image
        //        var postedFile = httpRequest.Files["file"];
        //        //Create custom filename
        //        if (postedFile != null)
        //        {
        //            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
        //            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
        //            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
        //            postedFile.SaveAs(filePath);
        //        }

        //        UploadViewModel model = new UploadViewModel();
        //        string uniqueFileName = UploadedFile(model);
        //        uploadModel upload = new uploadModel
        //        {
        //            Upload = uniqueFileName

        //        };
        //        return new BaseResult
        //        {
        //            Messsage = "Thank you, we will contact you shortly.",
        //            ResponseStatusCode = System.Net.HttpStatusCode.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResult
        //        {
        //            Errors = new List<string> { ex.Message },
        //            ResponseStatusCode = System.Net.HttpStatusCode.InternalServerError
        //        };
        //    }
        //}
        //private string UploadedFile(UploadViewModel model)
        //{
        //    string uniqueFileName = null;

        //    if (model.Upload != null)
        //    {
        //        //string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
        //        var folderName = Path.Combine("Resources", "Images");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Upload.FileName;
        //        string filePath = Path.Combine(pathToSave, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.Upload.CopyTo(fileStream);
        //        }
        //    }
        //    return uniqueFileName;
        //}

        [HttpPost, DisableRequestSizeLimit]
        [Route("Upload")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}


