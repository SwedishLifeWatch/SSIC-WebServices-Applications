using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class TaxonNameAuthorViewModel
    {
        public string Name { get; set; }
        public string Author { get; set; }

        public TaxonNameAuthorViewModel(string name, string author)
        {
            Name = name;
            Author = author;
        }
    }
}
