using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// It's used to get customer account information.
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class AccountController : ApiController
    {
        IAuthDetailsRepository _IAuthDetailsRepository;
        ICacheService _cacheService;

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
            _cacheService = cacheService;
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

        [HttpPost]
        [Route("api/profiles/account/forgotpassword")]
        public async Task<HttpResponseMessage> pwd(SignUpViewModel authDetailsViewModel)
        {
            return await Task.Run(() => { return sendforgotPassword(authDetailsViewModel.Email); });
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
            //if (authDetails.Customer != null)
            //{
            //    authDetails.Customer.FirstName = authDetailsViewModel.Customer.FirstName;
            //    authDetails.Customer.LastName = authDetailsViewModel.Customer.LastName;
            //    authDetails.Customer.DOB = authDetailsViewModel.Customer.DOB;
            //    authDetails.Customer.Gender = authDetailsViewModel.Customer.Gender;
            //    authDetails.Customer.Mobile = authDetailsViewModel.Customer.Mobile;
            //    _IAuthDetailsRepository.UpdateCustomer(authDetails);
            //    SignUpViewModel authViewModel = Mapper.Map<AuthDetails, SignUpViewModel>(authDetails);
            //    return Request.CreateResponse(HttpStatusCode.OK, authViewModel);
            //}
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
                return Request.CreateResponse(HttpStatusCode.Forbidden, TrippismConstants.IncorrectPassword);
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

        private HttpResponseMessage sendforgotPassword(string Emailid)
        {
            if(!String.IsNullOrEmpty(Emailid))
            {
                var authDetails = _IAuthDetailsRepository.FindCustomer(Emailid);
                if(authDetails == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, TrippismConstants.CustomerNotFound);
                }
                var password = authDetails.Password;
                EmailVerification.SendMail(null, password, authDetails.Email);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
