using System;

namespace paperless_api.Models
{
    public class Pack
    {
        public int Id {get; set;}
        public string LineNo {get; set;}
        public string MaterialNo {get; set;}
        public DateTime ProductionDate {get; set;}
    }
}