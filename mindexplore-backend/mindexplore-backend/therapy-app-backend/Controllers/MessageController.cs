using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Hubs;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUserRepository _userRepository;
       
        private readonly ITherapistService _therapistService;
        public MessageController(AppDbContext context, IHubContext<ChatHub> hubContext, IUserRepository userRepository, ITherapistService therapistService)
        {
            _context = context;
            _hubContext = hubContext;
            _userRepository = userRepository;
            _therapistService = therapistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesAsync()
        {
            return await _context.Messages.ToListAsync();
        }
        
        [HttpPost]
        public async Task<ActionResult<Message>> AddMessageAsync(Message message)
        {
          

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            await _hubContext.Groups.AddToGroupAsync(message.ConnectionId, message.RoomName);
            await _hubContext.Clients.Group(message.RoomName).SendAsync("ReceiveMessage", message);
            // await _hubContext.Clients.User(message.Receiver).SendAsync("ReceiveMessage", message);

            return Ok(message);
        }

        [HttpGet("{sender}/{receiver}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesAsync([FromRoute]string sender, [FromRoute] string receiver)
        {
            return await _context.Messages
                .Where(m => (m.Sender == sender && m.Receiver == receiver) ||
                            (m.Sender == receiver && m.Receiver == sender) )
                .ToListAsync();
        }

        [HttpGet("getUserId/{id}/{role}")]
        public async Task<string> GetUserIdById([FromRoute]int id,[FromRoute] string role)
        {
            return await _userRepository.GetUserIdByShortId(id, role);
        }

        //create hub generate connectionid

        [HttpGet("getConnection/{therapistId}/{patientId}")]
        public async Task<Conversation> CreateConnection([FromRoute] string therapistId, [FromRoute] string patientId)
        {
            var conversation = await _context.Conversations
                .Where(c => c.TherapistId == therapistId && c.PatientId == patientId)
                .FirstOrDefaultAsync();
            if (conversation == null)
            {
                var newConversation = new Conversation
                {
                    ConversationId = Guid.NewGuid(),
                    PatientId = patientId,
                    TherapistId = therapistId,

                };
                _context.Conversations.Add(newConversation);
                await _context.SaveChangesAsync();
                return newConversation;
            }
            else
            {
                return conversation;
            }
        }

        //get all patient conversations
        [HttpGet("patientConversations/{patientId}")]
        public async Task<List<Conversation>> GetConversationByPatientId([FromRoute] string patientId)
        {
            var conversations = await _context.Conversations
                .Where(c => c.PatientId == patientId)
                .ToListAsync();
            return conversations;
        }
        [HttpGet("patientConversationsReceiverNames/{patientId}")]
        public async Task<List<ConversationModel>> GetConversationModelByPatientId([FromRoute] string patientId)
        {
            var conversations = await _context.Conversations
               .Where(c => c.PatientId == patientId)
               .ToListAsync();
            var conversationsModel = new List<ConversationModel>();

            foreach(var conversation in conversations)
            {
                var therapist = await _context.Therapists.Where(c=> c.UserId == conversation.TherapistId).FirstOrDefaultAsync();
              
                var conversationModel = new ConversationModel
                {
                    ConversationId = conversation.ConversationId,
                    PatientId = conversation.PatientId,
                    TherapistId = conversation.TherapistId,
                    Therapist = therapist
                };
                conversationsModel.Add(conversationModel);
                
            }
            return conversationsModel;
        }

        //get all therapist conversations
        [HttpGet("therapistConversations/{therapistId}")]
        public async Task<List<Conversation>> GetConversationByTherapistId([FromRoute] string therapistId)
        {
            var conversations = await _context.Conversations
                .Where(c => c.TherapistId == therapistId)
                .ToListAsync();
            return conversations;
        }

        [HttpGet("patientConversation/{conversationId}")]
        public async Task<string> GetReceiverUserIdByConversationId([FromRoute] Guid conversationId)
        {
            var conversation = await _context.Conversations
                .Where(c => c.ConversationId == conversationId)
                .FirstOrDefaultAsync();
            return conversation.TherapistId;

        }
        [HttpGet("therapistConversation/{conversationId}")]
        public async Task<string> GetPatientUserIdByConversationId([FromRoute] Guid conversationId)
        {
            var conversation = await _context.Conversations
                .Where(c => c.ConversationId == conversationId)
                .FirstOrDefaultAsync();
            return conversation.PatientId;

        }
    }
}