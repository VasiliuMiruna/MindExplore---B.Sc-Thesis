namespace therapy_app_backend.BLL.Models
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
       
    }
}
