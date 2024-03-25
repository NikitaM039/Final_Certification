using System.Threading;
using WebApiLibr.Models.DTO;

namespace ApiMessages.Abstraction
{
    public interface IMessageRepository
    {
        public IEnumerable<MessageDTO> GetUnreadMessages(string recipientEmail);
        public Guid SendMessage(MessageDTO message);
    }
}
