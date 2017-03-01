using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    public class ExportSpeciesFactFactorValue
    {
        public bool IsChecked { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }                

        public static ExportSpeciesFactFactorValue Create(FactorFieldEnumValue factorFieldEnumValue, bool isChecked)
        {
            var model = new ExportSpeciesFactFactorValue();
            model.IsChecked = isChecked;
            model.Id = factorFieldEnumValue.KeyInt.Value;
            model.Name = factorFieldEnumValue.OriginalLabel;
            return model;            
        }

        public static ExportSpeciesFactFactorValue CreateValueMissingFactorValue(bool isChecked)
        {
            var model = new ExportSpeciesFactFactorValue();
            model.IsChecked = isChecked;
            model.Id = -1;
            model.Name = Resources.DyntaxaResource.SharedMissingValue;
            return model;
        }

        public static ExportSpeciesFactFactorValue Create(FactorFieldEnumValue factorFieldEnumValue)
        {
            return Create(factorFieldEnumValue, false);
        }
    }
}
