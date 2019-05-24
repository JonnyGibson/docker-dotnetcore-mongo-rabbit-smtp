using System.ComponentModel.DataAnnotations;

namespace frontend.Data
{
    public class Recipient
    {
        public Recipient(string email)
        {
            Email = email;
        }
        public Recipient ()
        {
            
        }

        [EmailAddress(ErrorMessage="Please enter valid email address")]
        public string Email { get; set; }
    }
}