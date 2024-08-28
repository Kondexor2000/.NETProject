using System.ComponentModel.DataAnnotations;

namespace MyApp.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}