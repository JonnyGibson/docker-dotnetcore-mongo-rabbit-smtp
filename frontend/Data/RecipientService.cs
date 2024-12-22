using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace frontend.Data
{
    public class RecipientService
    {
        private static readonly List<Recipient> Recipients = new();

        public async Task<Recipient[]> GetRecipientsAsync()
        {
            try
            {
                return await Task.FromResult(Recipients.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get recipients", ex);
            }
        }

        public void AddRecipient(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            try
            {
                Recipients.Add(new Recipient(email));
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add recipient: {email}", ex);
            }
        }

        public void Clear()
        {
            try
            {
                Recipients.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to clear recipients", ex);
            }
        }
    }
}