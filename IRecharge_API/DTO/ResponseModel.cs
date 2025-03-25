namespace IRecharge_API.DTO
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public required string Message { get; set; }
        public object Data { get; set; } // Add this property to fix the error
    }
}
