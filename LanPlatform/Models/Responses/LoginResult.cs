using GabionPlatform.DTO.Accounts;

namespace GabionPlatform.Models.Responses
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public UserAccountDto UserAccount { get; set; }

        public LoginResult()
        {
            Success = false;
            UserAccount = null;
        }
    }
}