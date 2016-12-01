using DataLayer;
using System;
using TrippismEntities;

namespace TrippismRepositories
{
    public interface IEmailTemplateRepository
    {
        EmailTemplate AddEmailTemplate(EmailTemplate emailTemplate);
        EmailTemplate GetEmailTemplate(string EmailTemplateName);
        EmailTemplate UpdateEmailTemplate(EmailTemplate emailTemplate);
    }
}
