using System;

namespace BookRecomenderApi.Models
{
    public class Email
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}