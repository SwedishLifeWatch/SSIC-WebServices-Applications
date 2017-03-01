using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    public class ExportNameStatusItem
    {
        public bool IsChecked { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public static ExportNameStatusItem Create(ITaxonNameStatus taxonNameStatus)
        {
            var model = new ExportNameStatusItem();
            model.Id = taxonNameStatus.Id;
            model.Name = taxonNameStatus.Name;
            model.IsChecked = false;
            return model;
        }
    }
}
