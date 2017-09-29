using MessageWorker.Domain.Entity;
using System;
using System.Threading.Tasks;

namespace MessageWorker.Domain.Bll.Impl
{
    public class SmsMessageService : ISmsMessageService
    {
        #region ISmsMessageService Members

        public async Task<SmsMessage> CreateAsync(SmsMessage smsMessage)
        {
            using (SmsMessageDbContext db = new SmsMessageDbContext())
            {
                try
                {
                    db.Add(smsMessage);
                    await db.ExecuteSaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return smsMessage;
        }

        #endregion ISmsMessageService Members
    }
}