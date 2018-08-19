using System;

namespace paperless_api.Models
{
    // Represents a picklist item.
    public class ListItem
    {
        public string ListName {get; set;}
        public string ListText {get; set;}
        public string ListValue {get; set;}
        public bool DefaultFlag {get; set;}
    }
}