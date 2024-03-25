using ApiMessages.Abstraction;
using AutoMapper;
using WebApiLibr;
using WebApiLibr.Models.DTO;
using WebApiLibr.Models.EntitiesModel;

namespace ApiMessages.Repo
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<MessageDTO> GetUnreadMessages(string recipientEmail)
        {
            using (_context)
            {
                var messages = _context.Messages
                    .Where(x => x.Recipient.Email == recipientEmail && !x.IsRead).ToList();

                var result = new List<MessageDTO>();
                foreach (var message in messages)
                {
                    message.IsRead = true;
                    result.Add(_mapper.Map<MessageDTO>(message));
                }

                _context.UpdateRange(messages);
                _context.SaveChanges();

                return result;
            }
            
        }

        public Guid SendMessage(MessageDTO model)
        {
            using (_context)
            {
                var sender = _context.Users.FirstOrDefault(x => x.Email == model.SenderEmail);
                var recipient = _context.Users.FirstOrDefault(x => x.Email == model.RecipientEmail);
                if (sender == null || recipient == null)
                {
                    return Guid.Empty;
                }

                var message = new Message
                {
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    RecipientId = recipient.Id,
                    SenderId = sender.Id,
                    Text = model.Text
                };

                _context.Messages.Add(message);
                _context.SaveChanges();

                return message.Id;
            }
            
        }
    }
    
}
