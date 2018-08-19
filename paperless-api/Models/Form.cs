using System;

namespace paperless_api.Models
{
    public class Form
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public string AuthorisedBy {get; set;}
        public string CreatedBy {get; set;}
        public DateTime CreatedDt {get; set;}
        public string UpdatedBy {get; set;}
        public DateTime UpdatedDt {get; set;}
        public float Version {get; set;}
        public DateTime EffectiveDt {get; set;}
        public DateTime ExpiryDt {get; set;}
    }
}