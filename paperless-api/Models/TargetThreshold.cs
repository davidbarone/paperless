using System;

namespace paperless_api.Models
{
    // Represents a target.
    public class TargetThreshold
    {
        public int Id {get; set;}
        public string TargetName {get; set;}
        public int EvaluationOrder {get; set;}
        public string MaterialNoRegex {get; set;}
        public string LineNoRegex {get; set;}
        public float RedThreshold1 {get; set;}
        public float RedThreshold2 {get; set;}
        public float GreenThreshold1 {get; set;}
        public float GreenThreshold2 {get; set;}
        public int Seq {get; set;}
    }
}