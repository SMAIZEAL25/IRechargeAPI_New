namespace IRecharge_API.Entities
{


   
    public class TokenResponses
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public Users user { get; set; }
        public string token_type { get; set; }
        public int token_validity { get; set; }
        public string token { get; set; }
    }

    public class Users
    {
        public int id { get; set; }
        public string name { get; set; }
        public object Firstname { get; set; }
        public object Lastname { get; set; }
        public string email { get; set; }
        public object referal { get; set; }
        public object email_verified_at { get; set; }
        public object current_team_id { get; set; }
        public object profile_photo_path { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string bonus_balance { get; set; }
        public string balance { get; set; }
        public string profile_photo_url { get; set; }
    }


}
