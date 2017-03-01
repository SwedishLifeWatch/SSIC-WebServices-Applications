using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    public class ExportNameType
    {        
        public bool IsChecked { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public static ExportNameType Create(ITaxonNameCategory taxonNameType)
        {
            var model = new ExportNameType();
            model.Id = taxonNameType.Id;
            model.Name = taxonNameType.Name;
            model.IsChecked = false;
            return model;
        }
    }
}
