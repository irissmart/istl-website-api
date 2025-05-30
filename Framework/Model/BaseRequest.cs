namespace Framework.Model
{
    public class BaseRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Text { get; set; }
    }
}