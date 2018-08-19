using System;

namespace paperless_api.Models
{
    public class Section
    {
        public int Id {get; set;}
        public int FormId {get; set;}
        public string Name {get; set;}
        public bool TabularFlag {get; set;}
        public int FrequencyMins {get; set;}
    }
}