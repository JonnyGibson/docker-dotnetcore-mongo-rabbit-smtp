using System.ComponentModel.DataAnnotations;

namespace frontend.Data
{
    public class Recipient
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        public Recipient()
        {
        }

        public Recipient(string email)
        {
            Email = email;
        }
    }
}