using System.ComponentModel.DataAnnotations;

namespace medicamento.ViewModels.Account
{
    public class UpdateViewModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Name is required.")]
        public string? UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string? Email { get; set; } = string.Empty;

        public string? Role { get; set; }
    }
}
