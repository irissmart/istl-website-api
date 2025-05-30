namespace Service.DTOs.Response
{
    public class NotificationDTO
    {
        public int Id { get; set; }

        public int ReceiverId { get; set; }

        public string Message { get; set; } = null!;

        public int TypeId { get; set; }

        public bool IsRead { get; set; }

        public DateTime Created { get; set; }
    }
}
