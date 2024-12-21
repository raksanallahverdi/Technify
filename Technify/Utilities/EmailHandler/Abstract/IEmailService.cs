using Technify.Utilities.EmailHandler.Models;

namespace Technify.Utilities.EmailHandler.Abstract
{
    public interface IEmailService
    {
        public void SendMessage(Message message);
    }
}
