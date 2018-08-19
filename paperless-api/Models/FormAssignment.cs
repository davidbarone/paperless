using System;

namespace paperless_api.Models
{
    public class FormAssignment
    {
        public int ID {get; set;}
        public string FORM_NAME_REGEX {get; set;}
        public string LINE_NO_REGEX {get; set;}
        public string MATERIAL_NO_REGEX {get; set;}
    }
}