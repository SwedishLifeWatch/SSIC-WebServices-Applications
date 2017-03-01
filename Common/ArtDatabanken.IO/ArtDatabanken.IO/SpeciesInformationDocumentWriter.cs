using System;
using System.Collections.Generic;
using System.Text;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;
using MigraDoc.DocumentObjectModel.Tables;

namespace ArtDatabanken.IO
{
    /// <summary>
    /// This class handles export of Species Information Documents (Artfaktablad) to Pdf files. It can also be used for exports to Rtf files.
    /// </summary>
    public class SpeciesInformationDocumentWriter : TaxonInformationFile
    {
        private const String LINE = "______________________________________________________________________________";
        private SpeciesInformationDocument _document;
        private Document _pdfFile;

        private class ParagraphItem
        {
            private Boolean _italic;
            private String _text;

            public ParagraphItem(String text, Boolean italic)
            {
                _text = text;
                _italic = italic;
            }

            public Boolean Italic
            {
                get { return _italic; }
            }

            public String Text
            {
                get { return _text; }
            }
        }

        /// <summary>
        /// Creates an instance of a Species Information Document Pdf file object. This object can then be used when exporting a Species Information Document to pdf or Rtf files.
        /// </summary>
        /// <param name="document">An instance of Species Information Document</param>
        public SpeciesInformationDocumentWriter(SpeciesInformationDocument document)
        {
            _document = document;
            _pdfFile = createPdfDocument(_document);
        }

        /// <summary>
        /// A method that transform a paragraph string to a list of paragraph items with appropriate text format information
        /// </summary>
        /// <param name="paragraph">The text</param>
        /// <param name="italicStrings">A list of strings that should be written with italics</param>
        /// <returns></returns>
        private List<ParagraphItem> GetParagraphItems(String paragraph, List<String> italicStrings)
        {
            List<ParagraphItem> items = new List<ParagraphItem>();
            if (paragraph == "")
            {
                return items;
            }

            if (italicStrings.IsEmpty())
            {
                ParagraphItem item = new ParagraphItem(paragraph, false);
                items.Add(item);
                return items;
            }

            Int32 startPos = 0;
            while (startPos < paragraph.Length)
            {
                Int32 minPos = paragraph.Length;
                Int32 maxPos = -1;
                String italicItemText = "";
                String normalItemText = "";
                foreach (String italicString in italicStrings)
                {
                    if (italicString.IsNotEmpty())
                    {
                        Int32 pos = paragraph.IndexOf(italicString, startPos);
                        if (pos > maxPos)
                        {
                            maxPos = pos;
                        }
                        if (pos < minPos && pos > -1)
                        {
                            minPos = pos;
                            italicItemText = italicString;
                        }
                    }
                }
                normalItemText = paragraph.Substring(startPos, minPos - startPos);
                ParagraphItem normalItem = new ParagraphItem(normalItemText, false);
                items.Add(normalItem);
                if (italicItemText != "")
                {
                    ParagraphItem italicItem = new ParagraphItem(italicItemText, true);
                    items.Add(italicItem);
                }
                startPos = startPos + normalItemText.Length + italicItemText.Length;
            }

            return items;
        }

        private String getUpdateInformation()
        {
            //©
            StringBuilder authorParagraph = new StringBuilder();
            if (_document.AuthorAndYear.IsNotEmpty())
            {
                Int32 pos = _document.AuthorAndYear.IndexOf("ArtDatabanken");

                if (pos - 2 > 0)
                {
                    authorParagraph.Append(_document.AuthorAndYear.Substring(0, pos - 2));
                }
                else
                {
                    authorParagraph.Append(_document.AuthorAndYear);
                }

            }

            authorParagraph.Append(" © ArtDatabanken, SLU ");
            authorParagraph.Append(_document.UpdateDateMaxValue.ToShortDateString());

            return authorParagraph.ToString();
        }

        /// <summary>
        /// This method creates the document layout for this pdf file
        /// </summary>
        /// <param name="document">A species information document object holding all the information that should be included in the pdf file</param>
        /// <returns>A formated pdf document</returns>
        private Document createPdfDocument(SpeciesInformationDocument document)
        {
            Document pdfFile = new Document();
            pdfFile.Info.Title = "Arfaktablad om " + document.Taxon.GetScientificNameAndAuthorAndCommonName() + " [DyntaxaTaxonId: " + document.Taxon.Id + "]";
            pdfFile.Info.Subject = "Arfaktablad med beskrivning, utbredningsuppgifter, ekologi, hot och naturvårdsåtgärder, litteratur";
            pdfFile.Info.Author = document.AuthorAndYear;

            // Get the predefined style Normal.
            Style style = pdfFile.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Times New Roman";
            style.Font.Size = Unit.FromPoint(11.5);

            Section section = pdfFile.AddSection();

            //Header1
            Paragraph scientificName = section.AddParagraph(document.ScientificName);
            scientificName.Format.Font.Name = "Arial";
            scientificName.Format.Font.Bold = true;
            scientificName.Format.Font.Size = Unit.FromPoint(16);
            scientificName.Format.Alignment = ParagraphAlignment.Center;

            //Header2
            if (document.CommonName.IsNotEmpty())
            {
                Paragraph commonName = section.AddParagraph(document.CommonName);
                commonName.Format.Font.Name = "Arial";
                commonName.Format.Font.Size = Unit.FromPoint(16);
                commonName.Format.Alignment = ParagraphAlignment.Center;
            }

            //Redlist information
            Table redlistInformationBox = section.AddTable();
            redlistInformationBox.Borders.Width = 0;
            //redlistInformationBox.Borders.Bottom.Width = Unit.FromPoint(0.25);
            
            Column columnOrganismGroup = redlistInformationBox.AddColumn(Unit.FromCentimeter(8));
            columnOrganismGroup.Format.Alignment = ParagraphAlignment.Left;
            Column columnRedlistInformation = redlistInformationBox.AddColumn(Unit.FromCentimeter(8));
            columnRedlistInformation.Format.Alignment = ParagraphAlignment.Right;
            Row row = redlistInformationBox.AddRow();
            Cell cell = row.Cells[0];
            Paragraph organismGroupName = cell.AddParagraph(document.OrganismGroup);
            organismGroupName.Format.Font.Name = "Arial";
            organismGroupName.Format.Font.Size = Unit.FromPoint(11.5);
            cell = row.Cells[1];
            Paragraph redlistInformationText = cell.AddParagraph();
            Font fontBig = new Font("Arial", Unit.FromPoint(11.5));
            Font fontSmall = new Font("Arial", Unit.FromPoint(9.5));
            Font fontCriterion = new Font("Arial", Unit.FromPoint(9));
            if (document.RedlistCategoryName.IsNotEmpty())
            {
                redlistInformationText.AddFormattedText(document.RedlistCategoryName.Substring(0, 1).ToUpper(), fontBig);
                redlistInformationText.AddFormattedText(document.RedlistCategoryName.Substring(1).ToUpper(), fontSmall);
                redlistInformationText.AddFormattedText(" (", fontBig);
                fontBig.Bold = true;
                redlistInformationText.AddFormattedText(document.RedlistCategoryShortString, fontBig);
                fontBig.Bold = false;
                redlistInformationText.AddFormattedText(")", fontBig);
                if (document.RedlistCriteria != "")
                {
                    redlistInformationText.AddLineBreak();
                    redlistInformationText.AddFormattedText(document.RedlistCriteria, fontCriterion);
                }
            }

            List<ParagraphItem> paragraphItems = null;

            section.AddParagraph(LINE);
 
            //Taxonomic information
            Paragraph taxonomicInformation = section.AddParagraph();
            taxonomicInformation.Format.Alignment = ParagraphAlignment.Justify;
            taxonomicInformation.Format.Font.Size = Unit.FromPoint(10);
            paragraphItems = GetParagraphItems(document.AutomaticTaxonomicParagraph, document.ItalicStringsInText);
            foreach (ParagraphItem item in paragraphItems)
            {
                if (item.Italic)
                {
                    taxonomicInformation.AddFormattedText(item.Text, TextFormat.Italic);
                }
                else
                {
                    taxonomicInformation.AddText(item.Text);
                }
            }

            //Main document paragraphs
            Paragraph paragraph;
            
            if (_document.DescriptionParagraph.IsNotEmpty())
            {
                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "5mm";
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
                paragraph.AddFormattedText("Beskrivning. ", TextFormat.Bold);
                paragraphItems = GetParagraphItems(document.DescriptionParagraph, document.ItalicStringsInText);
                foreach (ParagraphItem item in paragraphItems)
                {
                    if (item.Italic)
                    {
                        paragraph.AddFormattedText(item.Text, TextFormat.Italic);
                    }
                    else
                    {
                        paragraph.AddText(item.Text);
                    }
                }
            }

            if (_document.DistributionParagraph.IsNotEmpty())
            {
                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "5mm";
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
                paragraph.AddFormattedText("Utbredning och status. ", TextFormat.Bold);
                paragraphItems = GetParagraphItems(document.DistributionParagraph, document.ItalicStringsInText);
                foreach (ParagraphItem item in paragraphItems)
                {
                    if (item.Italic)
                    {
                        paragraph.AddFormattedText(item.Text, TextFormat.Italic);
                    }
                    else
                    {
                        paragraph.AddText(item.Text);
                    }
                }
            }

            if (_document.EcologyParagraph.IsNotEmpty())
            {
                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "5mm";
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
                paragraph.AddFormattedText("Ekologi. ", TextFormat.Bold);
                paragraphItems = GetParagraphItems(document.EcologyParagraph, document.ItalicStringsInText);
                foreach (ParagraphItem item in paragraphItems)
                {
                    if (item.Italic)
                    {
                        paragraph.AddFormattedText(item.Text, TextFormat.Italic);
                    }
                    else
                    {
                        paragraph.AddText(item.Text);
                    }
                }
            }

            if (_document.ThreatsParagraph.IsNotEmpty())
            {
                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "5mm";
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
                paragraph.AddFormattedText("Hot. ", TextFormat.Bold);
                paragraphItems = GetParagraphItems(document.ThreatsParagraph, document.ItalicStringsInText);
                foreach (ParagraphItem item in paragraphItems)
                {
                    if (item.Italic)
                    {
                        paragraph.AddFormattedText(item.Text, TextFormat.Italic);
                    }
                    else
                    {
                        paragraph.AddText(item.Text);
                    }
                }
            }

            if (_document.MeasuresParagraph.IsNotEmpty())
            {
                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "5mm";
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
                paragraph.AddFormattedText("Åtgärder. ", TextFormat.Bold);
                paragraphItems = GetParagraphItems(document.MeasuresParagraph, document.ItalicStringsInText);
                foreach (ParagraphItem item in paragraphItems)
                {
                    if (item.Italic)
                    {
                        paragraph.AddFormattedText(item.Text, TextFormat.Italic);
                    }
                    else
                    {
                        paragraph.AddText(item.Text);
                    }
                }
            }

            if (_document.ExtraParagraph.IsNotEmpty())
            {
                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "5mm";
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
                paragraph.AddFormattedText("Övrigt. ", TextFormat.Bold);
                paragraphItems = GetParagraphItems(document.ExtraParagraph, document.ItalicStringsInText);
                foreach (ParagraphItem item in paragraphItems)
                {
                    if (item.Italic)
                    {
                        paragraph.AddFormattedText(item.Text, TextFormat.Italic);
                    }
                    else
                    {
                        paragraph.AddText(item.Text);
                    }
                }
            }

            //Reference list
            if (_document.ReferenceParagraph.IsNotEmpty())
            {
                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "5mm";
                paragraph.AddFormattedText("Litteratur", TextFormat.Bold);
                
                string[] references = _document.ReferenceParagraph.Split('\n');

                foreach (string reference in references)
                {
                    Paragraph referenceParagraph = section.AddParagraph();
                    referenceParagraph.Format.LeftIndent = "1cm";
                    referenceParagraph.Format.FirstLineIndent = "-1cm";
                    referenceParagraph.Format.Alignment = ParagraphAlignment.Justify;
                    referenceParagraph.Format.Font.Size = Unit.FromPoint(10);
                    paragraphItems = GetParagraphItems(reference, document.ItalicStringsInReferences);
                    foreach (ParagraphItem item in paragraphItems)
                    {
                        if (item.Italic)
                        {
                            referenceParagraph.AddFormattedText(item.Text, TextFormat.Italic);
                        }
                        else
                        {
                            referenceParagraph.AddText(item.Text);
                        }
                    }
                }
            }

            paragraph = section.AddParagraph(LINE);
            paragraph.Format.SpaceBefore = "4mm";

            //Author information
            Table authorInformationBox = section.AddTable();
            //authorInformationBox.Format.SpaceBefore = "5mm";
            authorInformationBox.Borders.Width = 0;
            //authorInformationBox.Borders.Top.Width = Unit.FromPoint(0.25);
            Column columnAuthor = authorInformationBox.AddColumn(Unit.FromCentimeter(16));
            columnAuthor.Format.Alignment = ParagraphAlignment.Justify;
            row = authorInformationBox.AddRow();
            cell = row.Cells[0];
            Paragraph authorParagraph = cell.AddParagraph();

            authorParagraph.AddText(getUpdateInformation());

            return pdfFile;
        }

        /// <summary>
        /// A method that saves a Species Information Document as a pdf-file
        /// </summary>
        /// <param name="fileName">The full name, including path and extension of the file</param>
        public void SavePdf(String fileName)
        {
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = _pdfFile;
            renderer.RenderDocument();
            //Save the file
            renderer.PdfDocument.Save(fileName);
        }

        /// <summary>
        /// A method that saves a Species Information Document as a Rich Text Format (RTF) file
        /// </summary>
        /// <param name="fileName">The full name, including path and extension of the file</param>
        public void SaveRtf(String fileName)
        {
            RtfDocumentRenderer renderer = new RtfDocumentRenderer();
            //Save the file
            renderer.Render(_pdfFile, fileName, null);
        }
    }
}
