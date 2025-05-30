using Service.Enums;

namespace Service.DTOs.Request
{
    public class NotificationAddDTO
    {
        public int UserId { get; set; }

        public string Message { get; set; } = string.Empty;

        public NotificationType NotificationType { get; set; }
    }
}
