namespace IRecharge_API.Entities
{

    public class TokenResponses
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public User user { get; set; }
        public string token_type { get; set; }
        public int token_validity { get; set; }
        public string token { get; set; }
    }

}
