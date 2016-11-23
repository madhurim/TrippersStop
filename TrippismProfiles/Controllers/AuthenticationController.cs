using ExpressMapper;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TrippismApi.TraveLayer;
using TrippismEntities;
using TrippismProfiles.Constants;
using TrippismProfiles.Models;
using TrippismRepositories;

namespace TrippismProfiles.Controllers
{
    /// <summary>
    /// It's used to authenticate/register customer.
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    [ValidateModel]
    public class AuthenticationController : ApiController
    {

        IAuthDetailsRepository _IAuthDetailsRepository;
        ICacheService _cacheService;
        IEmailTemplateRepository _IEmailTemplateRepository;
        /// <summary>
        /// Set api - Authentication Repository.
        /// </summary>
        public AuthenticationController(IAuthDetailsRepository iAuthDetailsRepository, IEmailTemplateRepository iEmailTemplateRepository, ICacheService cacheService)
        {
            _IAuthDetailsRepository = iAuthDetailsRepository;
            _IEmailTemplateRepository = iEmailTemplateRepository;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Create new user
        /// </summary>
        [HttpPost]
        [Route("api/profiles/authentication/signup")]
        public async Task<HttpResponseMessage> SignUp(SignUpViewModel authDetailsViewModel)
        {
            return await Task.Run(() => { return CreateUser(authDetailsViewModel); });
        }


        /// <summary>
        /// For user signin
        /// </summary>
        [HttpPost]
        [Route("api/profiles/authentication/signin")]
        public async Task<HttpResponseMessage> SignIn(SignInViewModel signin)
        {
            return await Task.Run(() => { return Login(signin); });
        }


        private HttpResponseMessage CreateUser(SignUpViewModel signUpViewModel)
        {
            AuthDetails authdetail = Mapper.Map<SignUpViewModel, AuthDetails>(signUpViewModel);

            var data = _IAuthDetailsRepository.FindCustomer(authdetail.Email);
            if (data != null)
                return Request.CreateResponse(HttpStatusCode.Found, TrippismConstants.CustomerAlreadyExist);


            string strPwd = ApiHelper.CreateRandomPassword(8);

            data = _IAuthDetailsRepository.FindCustomer(authdetail.CustomerGuid);

            Guid guid = authdetail.CustomerGuid == Guid.Empty ? System.Guid.NewGuid() : ((data != null) ? System.Guid.NewGuid() : authdetail.CustomerGuid);
            authdetail.Password = PasswordHash.CreateHash(strPwd);
            authdetail.CustomerGuid = guid;
            authdetail.IsEmailVerified = false;
            authdetail.CreatedDate = DateTime.Now;
            authdetail.LoginTime = DateTime.Now;
            authdetail.ModifiedDate = DateTime.Now;
            authdetail.IsActive = true;
            authdetail.Token = null;

            _IAuthDetailsRepository.AddCustomer(authdetail);

            var emailTemplateName = "New Account Password";
            var mail = _IEmailTemplateRepository.GetEmailTemplate(emailTemplateName);

            mail.Body = mail.Body.Replace("<hostlink>", "http://dev.trippism.com/")
                                    .Replace("<logo>", "http://dev.trippism.com/images/trippism-logo.png")
                                    .Replace("<password>", strPwd)
                                    .Replace("<sitename>", "Trippism")
                                    .Replace("<year>", DateTime.Now.Year.ToString())
                                    .Replace("<facebook>", "https://www.facebook.com/trippismcom-1493664570958968/")
                                    .Replace("<twitter>", "https://twitter.com/trippismapp")
                                    .Replace("<pinterest>", "https://www.pinterest.com/trippismsite/trippism/")
                                    .Replace("<linkedin>", "https://www.linkedin.com/company/trippism?trk=vsrp_companies_res_name&trkInfo=VSRPsearchId%3A44363051459161854061%2CVSRPtargetId%3A10201827%2CVSRPcmpt%3Aprimary")
                                    .Replace("<blog>", "http://blog.trippism.com/")
                                    .Replace("<faqs>", "http://dev.trippism.com/#/FAQs");
            
            EmailVerification.SendUserMail("noreply@trippism.com", authdetail.Email, mail.Subject, mail.Body);
            SignUpViewModel authViewModel = Mapper.Map<AuthDetails, SignUpViewModel>(authdetail);
            return Request.CreateResponse(HttpStatusCode.OK, authViewModel);
        }


        private HttpResponseMessage Login(SignInViewModel signInViewModel)
        {
            var authDetails = _IAuthDetailsRepository.FindCustomer(signInViewModel.Email);
            if (authDetails == null || !authDetails.IsActive)
                return Request.CreateResponse(HttpStatusCode.Forbidden, TrippismConstants.IncorrectUserNameOrPassword);
            else if (!PasswordHash.ValidatePassword(signInViewModel.Password, authDetails.Password))
                return Request.CreateResponse(HttpStatusCode.Forbidden, TrippismConstants.IncorrectUserNameOrPassword);
            else
            {
                SignUpViewModel authDetailsViewModel = Mapper.Map<AuthDetails, SignUpViewModel>(authDetails);
                string ipAddress = ApiHelper.GetClientIP(Request);
                string userAgent = ApiHelper.GetClientUserAgent(Request);

                string token = SecurityManager.GenerateToken(authDetails.Email, authDetails.Password, ipAddress, userAgent, DateTime.UtcNow.Ticks);

                return Request.CreateResponse(HttpStatusCode.OK, new { AuthDetailsViewModel = authDetailsViewModel, SecutityToken = token });
            }
        }
    }
}
