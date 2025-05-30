using Framework.Model;
using Framework.Service;
using Infrastructure;
using Infrastructure.ModelsX;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Enums;
using Service.Hubs;
using Service.Interface;

namespace Service
{
    public class NotificationService : BaseDatabaseService<IrisContext>, INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(IrisContext context, IHubContext<NotificationHub> hubContext) : base(context)
        {
            _hubContext = hubContext;
        }

        public async Task<BaseResponse<List<NotificationDTO>>> GetAllAsync(int? userId, BaseRequest dto)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.Notifications
                    .Where(x => x.IsActive);

                if (userId != null)
                {
                    query = query.Where(x => x.ReceiverId == userId);
                }

                query = query.OrderByDescending(x => x.CreatedOn);

                return await GetPaginatedResultAsync(query, dto.PageNumber, dto.PageSize);
            }, entity => new NotificationDTO
            {
                Id = entity.Id,
                ReceiverId = entity.ReceiverId,
                TypeId = entity.TypeId,
                Message = entity.Message,
                IsRead = entity.IsRead,
                Created = entity.CreatedOn
            });
        }

        public async Task<BaseResponse<Task>> AddAsync(int userId, NotificationAddDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (string.IsNullOrEmpty(request.Message))
                {
                    InitMessageResponse("BadRequest");
                    return;
                }

                await LogNotification(userId, request.UserId, request.Message, request.NotificationType);
                await SendNotification(userId, request.UserId, request.Message, request.NotificationType);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var dbEntities = await _context.Notifications.Where(x => x.ReceiverId == userId && !x.IsRead).ToListAsync();

                if (dbEntities.Count == 0) return;

                dbEntities.ForEach(x => x.IsRead = true);

                await _context.SaveChangesAsync(userId);
            });
        }

        private async Task LogNotification(int senderId, int receiverId, string message, NotificationType notificationType)
        {
            await _context.Notifications.AddAsync(new Notification
            {
                ReceiverId = receiverId,
                TypeId = (int)notificationType,
                Message = message,
                IsRead = false
            });
            await _context.SaveChangesAsync(senderId);
        }

        private async Task SendNotification(int senderId, int receiverId, string message, NotificationType notificationType)
        {
            await _hubContext.Clients.All.SendAsync("notification", new { Message = message, Type = notificationType, SenderID = senderId, ReceiverId = receiverId });
        }

    }
}
