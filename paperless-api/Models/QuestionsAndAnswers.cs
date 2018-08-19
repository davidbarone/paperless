using System;

namespace paperless_api.Models
{
    public class QuestionsAndAnswers
    {                        
        public string SurrogateId {get; set;}
        public int QuestionId {get; set;}
        public int FormId {get; set;}
        public int PackId {get; set;}
        public string QuestionType {get; set;}
        public string QuestionGroup {get; set;}
        public string ListName {get; set;}
        public string Prompt {get; set;}
        public bool MultiValueFlag {get; set;}
        public int FrequencyMins {get; set;}
        public bool AnsweredFlag {get; set;}
        public int AnswerId {get; set;}
        public DateTime AnswerDt {get; set;}
        public string AnswerByProd {get; set;}
        public string AnswerByTech {get; set;}
        public string Value {get; set;}
        public string Comment {get; set;}
    }
}