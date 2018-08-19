using System;

namespace paperless_api.Models
{
    // Represents a question in a form.
    public class Question
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public char QuestionType { get; set; }
        public string ListName { get; set; }
        public string Prompt { get; set; }
        public string TargetName {get; set; }
        public DateTime EffectiveDt { get; set; }
        public DateTime ExpiryDt { get; set; }
    }
}