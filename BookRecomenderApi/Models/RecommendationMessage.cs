using System;

namespace BookRecomenderApi.Models
{
    public class RecommendationMessage
    {
        public string RecipientEmail { get; set; }
        public Recomendation Recommendation { get; set; }

        public RecommendationMessage(string recipientEmail, Recomendation recommendation)
        {
            RecipientEmail = recipientEmail ?? throw new ArgumentNullException(nameof(recipientEmail));
            Recommendation = recommendation ?? throw new ArgumentNullException(nameof(recommendation));
        }
    }
} 