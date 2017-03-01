using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Reference
{
    /// <summary>
    /// View model for presenting a reference
    /// </summary>
    public class ReferenceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [AllowHtml]
        public String Name { get; set; }
        
        [Required]
        [Range(0, 3000, ErrorMessage = "Year must be between 0 and 3000")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "Text is required")]
        [AllowHtml]
        public String Text { get; set; }

        public string Usage { get; set; }

        public int UsageTypeId { get; set; }

        public ReferenceViewModel()
        {
        }

        public ReferenceViewModel(int id, string name, int year, string text, string usage, int usageTypeId)
            : this(id, name, year, text)
        {
            Usage = usage;
            UsageTypeId = usageTypeId;            
        }

        public ReferenceViewModel(int id, string name, int year, string text)
        {
            Id = id;
            Name = name;
            Year = year;
            Text = text;            
        }        
    }
}