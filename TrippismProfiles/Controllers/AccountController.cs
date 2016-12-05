using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre.Response;
using TrippismApi.TraveLayer;
using TrippismEntities;
using TrippismProfiles.Constants;
using TrippismProfiles.Models;
using TrippismRepositories;

namespace TrippismProfiles.Controllers
{
    /// <summary>
    /// It's used to get customer account information.
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class AccountController : ApiController
    {
        IAuthDetailsRepository _IAuthDetailsRepository;
        ICacheService _cacheService;
        IEmailTemplateRepository _IEmailTemplateRepository;

        private Guid AuthId
        {
            get
            {
                return Guid.Parse(Request.Headers.GetValues("AuthId").FirstOrDefault());
            }
        }


        /// <summary>
        /// Set api - Anonymous Repository.
        /// </summary>
        public AccountController(IAuthDetailsRepository iAuthDetailsRepository, ICacheService cacheService)
        {
            _IAuthDetailsRepository = iAuthDetailsRepository;
            _IEmailTemplateRepository = iEmailTemplateRepository;
            _cacheService = cacheService;
            _IEmailTemplateRepository = IEmailTemplateRepository;
        }

        /// <summary>
        /// This method is used to get customer details
        /// </summary>
        //[HttpPut]
        //[TrippismAuthorize]
        [Route("api/profiles/account")]
        public async Task<HttpResponseMessage> Get()
        {
            return await Task.Run(() => { return GetCustomer(); });
        }


        /// <summary>
        /// This method is used to update customer details
        /// </summary>
        [HttpPut]
        //[TrippismAuthorize]
        [Route("api/profiles/account")]
        public async Task<HttpResponseMessage> Update(SignUpViewModel authDetailsViewModel)
        {
            return await Task.Run(() => { return UpdateCustomer(authDetailsViewModel); });
        }

        [HttpGet]
        [Route("api/profiles/account/forgotpassword")]
        public async Task<HttpResponseMessage> pwd(string Emailid, string Url)
        {
            return await Task.Run(() => { return sendforgotPassword(Emailid, Url); });
        }


        /// <summary>
        /// This method is used to update customer password 
        /// </summary>
        [HttpPost]
        //[TrippismAuthorize]
        [Route("api/profiles/account/changepassword")]
        public async Task<HttpResponseMessage> ChangePassword(UpdatePasswordViewModel updatePasswordViewModel)
        {
            return await Task.Run(() => { return UpdatePassword(updatePasswordViewModel); });
        }

        [HttpPost]
        //[TrippismAuthorize]
        [Route("api/profiles/account/resetPassword")]
        public async Task<HttpResponseMessage> resetAccountPassword(AuthDetails authDetail)
        {
            return await Task.Run(() => { return resetPassword(authDetail); });
        }

        /// <summary>
        /// This method is used to delete customer
        /// </summary>
        [TrippismAuthorize]
        [Route("api/profiles/account")]
        public async Task<HttpResponseMessage> DeleteCustomer()
        {
            return await Task.Run(() => { return DeleteUser(); });
        }
        private HttpResponseMessage DeleteUser()
        {
            string message = string.Empty;
            var authDetails = _IAuthDetailsRepository.FindCustomer(this.AuthId);
            if (authDetails == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, TrippismConstants.CustomerNotFound);

            authDetails.IsActive = false;
            _IAuthDetailsRepository.UpdateCustomer(authDetails);
            return Request.CreateResponse(HttpStatusCode.OK, TrippismConstants.RecordDeleted);
        }
        private HttpResponseMessage UpdateCustomer(SignUpViewModel authDetailsViewModel)
        {
            string message = string.Empty;
            var authDetails = _IAuthDetailsRepository.FindCustomer(authDetailsViewModel.CustomerGuid);
            if (authDetails == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, TrippismConstants.CustomerNotFound);
            return Request.CreateResponse(HttpStatusCode.NotFound, TrippismConstants.CustomerNotFound);
        }

        private HttpResponseMessage UpdatePassword(UpdatePasswordViewModel updatePasswordViewModel)
        {

            var authDetails = _IAuthDetailsRepository.FindCustomer(updatePasswordViewModel.CustomerGuid);
            if (authDetails == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, TrippismConstants.CustomerNotFound);
            }
            var oldPassword = PasswordHash.CreateHash(updatePasswordViewModel.OldPassword);
            if (!PasswordHash.ValidatePassword(updatePasswordViewModel.OldPassword, authDetails.Password))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, TrippismConstants.IncorrectPassword);
            }
            authDetails.Password = PasswordHash.CreateHash(updatePasswordViewModel.NewPassword);
            _IAuthDetailsRepository.UpdateCustomer(authDetails);
            return Request.CreateResponse(HttpStatusCode.OK, TrippismConstants.PasswordUpdatedSuccessfully);
        }
        private HttpResponseMessage GetCustomer()
        {
            string message = string.Empty;
            var authDetails = _IAuthDetailsRepository.FindCustomer(this.AuthId);
            if (authDetails == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, TrippismConstants.CustomerNotFound);
            }
            else
            {
                SignUpViewModel authViewModel = Mapper.Map<AuthDetails, SignUpViewModel>(authDetails);

                return Request.CreateResponse(HttpStatusCode.OK, authViewModel);
            }

        }

        private HttpResponseMessage sendforgotPassword(string Emailid, string Url)
        {
            if (!String.IsNullOrEmpty(Emailid))
            {
                var authDetails = _IAuthDetailsRepository.FindCustomer(Emailid);
                if (authDetails == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, TrippismConstants.CustomerNotFound);
                }
                authDetails.Token = ApiHelper.CreateRandomPassword(8);

                var emailTemplateName = "Reset Password";
                var mail = _IEmailTemplateRepository.GetEmailTemplate(emailTemplateName);

                var changePasswordUrl = "http://" + Url + "/#/changepassword/T=" + authDetails.Token + ";G=" + authDetails.CustomerGuid;

                string hostUrl = Request.Headers.Referrer.AbsoluteUri.ToString();

                _IAuthDetailsRepository.UpdateCustomer(authDetails);
                mail.Body = mail.Body.Replace("<hostlink>", hostUrl)
                                    .Replace("<logo>", hostUrl + "images/trippism-logo.png")
                                    .Replace("<changePasswordLink>", changePasswordUrl)
                                    .Replace("<sitename>", "Trippism")
                                    .Replace("<year>", DateTime.Now.Year.ToString())
                                    .Replace("<facebook>", ConfigurationManager.AppSettings["FacebookUrl"].ToString())
                                    .Replace("<twitter>", ConfigurationManager.AppSettings["TwitterUrl"].ToString())
                                    .Replace("<pinterest>", ConfigurationManager.AppSettings["PinterestUrl"].ToString())
                                    .Replace("<linkedin>", ConfigurationManager.AppSettings["LinkedinUrl"].ToString())
                                    .Replace("<blog>", ConfigurationManager.AppSettings["BlogUrl"].ToString())
                                    .Replace("<faqs>", hostUrl + "#/FAQs");

                string fromEmail = ConfigurationManager.AppSettings["MailGunFromemail"];

                EmailVerification sendmail = new EmailVerification();
                sendmail.SendUserMail(fromEmail, authDetails.Email, mail.Subject, mail.Body);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Please, Check Your Mail!");
        }
        private HttpResponseMessage resetPassword(AuthDetails authDetail)
        {
            if (!String.IsNullOrEmpty(authDetail.Token))
            {
                var exitsAuthDetails = _IAuthDetailsRepository.FindCustomer(authDetail.CustomerGuid);
                if (exitsAuthDetails == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, TrippismConstants.CustomerNotFound);
                }

                if (authDetail.Token == exitsAuthDetails.Token)
                {
                    exitsAuthDetails.Token = null;
                    exitsAuthDetails.Password = PasswordHash.CreateHash(authDetail.Password);
                    _IAuthDetailsRepository.UpdateCustomer(exitsAuthDetails);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.GatewayTimeout, "Token expire Or " + TrippismConstants.CustomerNotFound);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, TrippismConstants.PasswordUpdatedSuccessfully);
        }
    }
}
