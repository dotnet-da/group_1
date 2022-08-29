namespace AccountsDLL.Models
{
    public class DeleteResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public DeleteResponse(bool status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
