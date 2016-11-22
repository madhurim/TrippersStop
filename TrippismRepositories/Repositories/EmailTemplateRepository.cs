using TrippismEntities;
using DataLayer;
using System;

namespace TrippismRepositories
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private IDBContext _iDBContext;
        public EmailTemplateRepository(IDBContext iDBContext)
        {
            this._iDBContext = iDBContext;
        }

        public EmailTemplate AddEmailTemplate(EmailTemplate emailTemplate)
        {
            _iDBContext.Add<EmailTemplate>(emailTemplate);
            return emailTemplate;
        }
        public EmailTemplate UpdateEmailTemplate(EmailTemplate emailtemplate)
        {
            _iDBContext.Update<EmailTemplate>(a => a.EmailTemplateName == emailtemplate.EmailTemplateName, emailtemplate);
            return emailtemplate;
        }
        public EmailTemplate GetEmailTemplate(string EmailTemplateName)
        {
            var emailTemplate = _iDBContext.FindOne<EmailTemplate>(a => a.EmailTemplateName == EmailTemplateName);
            return emailTemplate;   
        }
        public int DeleteEmailTemplate(string emailTemplateName)
        {
            _iDBContext.Delete<EmailTemplate>(a => a.EmailTemplateName == emailTemplateName);
            return 1;
        }
    }
}
