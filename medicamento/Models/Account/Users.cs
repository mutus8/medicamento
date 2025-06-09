using Microsoft.AspNetCore.Identity;

namespace medicamento.Models.Account
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
