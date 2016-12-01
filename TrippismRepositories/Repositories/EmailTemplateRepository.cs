using TrippismEntities;
using DataLayer;
using System;

namespace TrippismRepositories
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        //private IDBContext _iDBContext;   
        private IEmailContext _iEmailContext;
        public EmailTemplateRepository(IEmailContext iEmailContext)
        {
            this._iEmailContext = iEmailContext;
        }

        public EmailTemplate AddEmailTemplate(EmailTemplate emailTemplate)
        {
            _iEmailContext.Add<EmailTemplate>(emailTemplate);
            return emailTemplate;
        }
        public EmailTemplate UpdateEmailTemplate(EmailTemplate emailtemplate)
        {
            _iEmailContext.Update<EmailTemplate>(a => a.EmailTemplateName == emailtemplate.EmailTemplateName, emailtemplate);
            return emailtemplate;
        }
        public EmailTemplate GetEmailTemplate(string EmailTemplateName)
        {
            var emailTemplate = _iEmailContext.FindOne<EmailTemplate>(a => a.EmailTemplateName == EmailTemplateName);
            return emailTemplate;
        }
        public int DeleteEmailTemplate(string emailTemplateName)
        {
            _iEmailContext.Delete<EmailTemplate>(a => a.EmailTemplateName == emailTemplateName);
            return 1;
        }
    }
}
