using System;
using CMSSolutions.ContentManagement.Messages.Services;
using CMSSolutions.Net.Mail;
using CMSSolutions.Tasks;
using CMSSolutions.Websites.Services;
using Castle.Core.Logging;

namespace CMSSolutions.Websites.Tasks
{
    public class SendEmailTasks: IScheduleTask
    {
        public SendEmailTasks()
        {
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public string Name { get { return "Gửi danh sách phim tự động qua email"; } }

        public bool Enabled { get { return false; } }

        public string CronExpression { get { return "0 0 23 ? * MON-FRI *"; } }

        public bool DisallowConcurrentExecution { get { return true; } }

        public void Execute(IWorkContextScope scope)
        {
            int maxTries = 1;
            int pageSize = 100;
            int pageIndex = 1;
            int totalRecord = 0;
            int totalPage = 1;

            var smtpSettings = scope.Resolve<SmtpSettings>();
            if (smtpSettings.MessagesPerBatch > 0)
            {
                maxTries = smtpSettings.MaxTries;
                pageSize = smtpSettings.MessagesPerBatch;
            }

            var emailSender = scope.Resolve<IEmailSender>();
            var messageService = scope.Resolve<IMessageService>();
            var customerService = scope.Resolve<ICustomerService>();
            try
            {
                customerService.AddEmailMessages();
            }
            catch (Exception)
            {
               
            }
            var queuedEmails = customerService.GetQueuedEmails(maxTries, DateTime.Parse("01/01/1900"), pageIndex, pageSize, out totalRecord);
            if (queuedEmails != null && totalRecord > 0)
            {
                if (totalRecord <= pageSize)
                {
                    totalPage = 1;
                }
                else
                {
                    var count = totalRecord % pageSize;
                    if ((count == 0))
                    {
                        totalPage = totalRecord / pageSize;
                    }
                    else
                    {
                        totalPage = ((totalRecord - count) / pageSize) + 1;
                    }
                }

                for (int i = 1; i <= totalPage; i++)
                {
                    try
                    {
                        if (i > 1)
                        {
                            queuedEmails = customerService.GetQueuedEmails(maxTries, DateTime.Parse("01/01/1900"), pageIndex, pageSize, out totalRecord);
                        }

                        foreach (var queuedEmail in queuedEmails)
                        {
                            try
                            {
                                var mailMessage = queuedEmail.GetMailMessage();
                                emailSender.Send(mailMessage);
                                queuedEmail.SentOnUtc = DateTime.UtcNow;
                            }
                            catch (Exception exc)
                            {
                                Logger.Error(string.Format("Error sending e-mail. {0}", exc.Message), exc);
                            }
                            finally
                            {
                                queuedEmail.SentTries = queuedEmail.SentTries + 1;
                                messageService.Update(queuedEmail);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                    pageIndex++;
                }
            }
        }
    }
}