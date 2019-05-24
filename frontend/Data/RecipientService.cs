using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace frontend.Data
{
    public class RecipientService
    {
        private static List<Recipient> Recipients = new List<Recipient>
        {
           
        };
        public async Task<Recipient[]> GetRecipientsAsync()
        {
            return await Task.FromResult(
                Recipients.ToArray()
                );
        }

        public void AddRecipient(string email)
        {
            Recipients.Add(new Recipient(email));
        }

        public void Clear()
        {
            Recipients.Clear();
        }
    }
}