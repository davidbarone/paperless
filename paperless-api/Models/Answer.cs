using System;

namespace paperless_api.Models
{
    public class Answer
    {                        
        public int Id {get; set;}
        public int QuestionId {get; set;}
        public int PackId {get; set;}
        public int Seq {get; set;}
        public string AnswerByProd {get; set;}
        public string AnswerByTech {get; set;}
        public string Value {get; set;}
        public string Comment {get; set;}
        public DateTime AnswerDt {get; set;}
    }
}