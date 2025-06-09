using medicamento.ViewModels.Account;

namespace medicamento.Models.Account.Extensions
{
    public static class UserExtension
    {
        public static UpdateViewModel ToViewModel(this Users users)
        {
            if (users == null) return new();

            return new UpdateViewModel
            {
                FullName = users.FullName,
                UserName = users.UserName,
                Email = users.Email
            };
        }
    }
}
