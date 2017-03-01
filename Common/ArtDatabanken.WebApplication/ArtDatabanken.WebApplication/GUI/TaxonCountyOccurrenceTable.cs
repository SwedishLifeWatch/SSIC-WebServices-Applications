using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.GUI
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TaxonCountyOccurrenceTable runat=server></{0}:TaxonCountyOccurrenceTable>")]
    public class TaxonCountyOccurrenceTable : CompositeControl
    {
        protected HtmlTable _table;
        private TaxonCountyOccurrenceList _countyOccurrencies;

        /// <summary>
        /// Create a TaxonCountyOccurrenceTable instance.
        /// </summary>
        public TaxonCountyOccurrenceTable()
        {
            this.EnableViewState = false;
            _countyOccurrencies = null;
            _table = new HtmlTable();
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue(null)]
        [Localizable(true)]
        public TaxonCountyOccurrenceList CountyOccurrencies
        {
            get
            {
                return _countyOccurrencies;
            }

            set
            {
                HtmlTableCell cell;
                HtmlTableRow row;
                Label label;
                Label countyOccurrenceLabel;

                _countyOccurrencies = value;
                if (_countyOccurrencies.IsNotEmpty())
                {
                    // Add explanation.
                    row = new HtmlTableRow();
                    cell = new HtmlTableCell();
                    cell.InnerText = "Förekomst per län i Sverige";
                    cell.ColSpan = _countyOccurrencies.Count;
                    row.Cells.Add(cell);
                    _table.Rows.Add(row);

                    // Add county information.
                    row = new HtmlTableRow();
                    foreach (TaxonCountyOccurrence countyOccurrence in _countyOccurrencies)
                    {
                        label = new Label();
                        label.Text = countyOccurrence.County.Identifier;
                        label.ToolTip = countyOccurrence.County.Name;
                        cell = new HtmlTableCell();
                        cell.Controls.Add(label);
                        row.Cells.Add(cell);
                    }
                    _table.Rows.Add(row);

                    // Add county occurrence information.
                    row = new HtmlTableRow();
                    foreach (TaxonCountyOccurrence countyOccurrence in _countyOccurrencies)
                    {
                        countyOccurrenceLabel = new Label();
                        switch (countyOccurrence.OriginalCountyOccurrence)
                        {
                            case "O":
                                countyOccurrenceLabel.Text = Properties.Resources.RedListCountyOccurrenceUncertainSymbol;
                                countyOccurrenceLabel.ToolTip = Properties.Resources.RedListCountyOccurrenceUncertainText;
                                break;
                            case "T":
                                countyOccurrenceLabel.Text = Properties.Resources.RedListCountyOccurrenceTemporarySymbol;
                                countyOccurrenceLabel.ToolTip = Properties.Resources.RedListCountyOccurrenceTemporaryText;
                                break;
                            case "U":
                                countyOccurrenceLabel.Text = Properties.Resources.RedListCountyOccurrenceExtinctSymbol;
                                countyOccurrenceLabel.ToolTip = Properties.Resources.RedListCountyOccurrenceExtinctText;
                                break;
                            case "X":
                                countyOccurrenceLabel.Text = Properties.Resources.RedListCountyOccurrenceResidentSymbol;
                                countyOccurrenceLabel.ToolTip = Properties.Resources.RedListCountyOccurrenceResidentText;
                                break;
                            default:
                                countyOccurrenceLabel.Text = "&nbsp";
                                break;
                        }
                        cell = new HtmlTableCell();
                        cell.Controls.Add(countyOccurrenceLabel);
                        row.Cells.Add(cell);
                    }
                    _table.Rows.Add(row);
                }
            }
        }

        /// <summary>
        /// Add child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Add(_table);
        }
    }
}
