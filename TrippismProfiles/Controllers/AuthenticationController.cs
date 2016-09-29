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
        /// <summary>
        /// Set api - Authentication Repository.
        /// </summary>
        public AuthenticationController(IAuthDetailsRepository iAuthDetailsRepository, ICacheService cacheService)
        {
            _IAuthDetailsRepository = iAuthDetailsRepository;
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

            var data = _IAuthDetailsRepository.FindCustomer(authdetail.UserName);
            if (data != null)
                return Request.CreateResponse(HttpStatusCode.Found, TrippismConstants.CustomerAlreadyExist);
            
    
            string strPwd = ApiHelper.CreateRandomPassword(8);
            Guid guid = authdetail.CustomerGuid == Guid.Empty ? System.Guid.NewGuid() : authdetail.CustomerGuid;
            authdetail.Password = PasswordHash.CreateHash(strPwd);
            authdetail.CustomerGuid = guid;
            authdetail.IsEmailVerified = false;
            authdetail.CreatedDate = DateTime.Now;
            authdetail.LoginTime = DateTime.Now;
            authdetail.ModifiedDate = DateTime.Now;
            authdetail.IsActive = true;
            if (authdetail.Customer == null)
            {
                authdetail.Customer = new Customer()
                    {
                        Email = authdetail.UserName
                    };

            }
            authdetail.Customer.CreatedDate = DateTime.Now;
            authdetail.Customer.ModifiedDate = DateTime.Now;         
            _IAuthDetailsRepository.AddCustomer(authdetail);
            if (authdetail.Customer != null)
            {
                EmailVerification.SendMail(authdetail.Customer.FirstName, strPwd, authdetail.UserName);
            }
            SignUpViewModel authViewModel = Mapper.Map<AuthDetails, SignUpViewModel>(authdetail);
            return Request.CreateResponse(HttpStatusCode.OK, authViewModel);
        }


        private HttpResponseMessage Login(SignInViewModel signInViewModel)
        {
            var authDetails = _IAuthDetailsRepository.FindCustomer(signInViewModel.UserName);
            if (authDetails == null || !authDetails.IsActive)
                return Request.CreateResponse(HttpStatusCode.Forbidden, TrippismConstants.IncorrectUserNameOrPassword);
            else if (!PasswordHash.ValidatePassword(signInViewModel.Password, authDetails.Password))
                return Request.CreateResponse(HttpStatusCode.Forbidden, TrippismConstants.IncorrectUserNameOrPassword);
            else
            {
                SignUpViewModel authDetailsViewModel = Mapper.Map<AuthDetails, SignUpViewModel>(authDetails);
                string ipAddress = ApiHelper.GetClientIP(Request);
                string userAgent = ApiHelper.GetClientUserAgent(Request);

                string token = SecurityManager.GenerateToken(authDetails.UserName, authDetails.Password, ipAddress, userAgent,DateTime.UtcNow.Ticks); 

                return Request.CreateResponse(HttpStatusCode.OK, new {AuthDetailsViewModel=  authDetailsViewModel, SecutityToken=token});
            }
        }
    }
}
