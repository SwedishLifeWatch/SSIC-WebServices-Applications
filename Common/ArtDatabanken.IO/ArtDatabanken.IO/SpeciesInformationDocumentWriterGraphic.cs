using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Web;
using ArtDatabanken;
using ArtDatabanken.Data.ArtDatabankenService;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;
using MigraDoc.DocumentObjectModel.Tables;


namespace ArtDatabanken.IO
{
    /// <summary>
    /// This class handles export of Species Information Documents (Artfaktablad) to Pdf files. It can also be used for exports to Rtf files.
    /// </summary>
    public class SpeciesInformationDocumentWriterGraphic : TaxonInformationFile
    {
        private const String LINE = "_________________________________________";
        private Data.ArtDatabankenService.SpeciesInformationDocument _document;
        private Document _pdfFile;
        private System.Drawing.Image _taxonImage;
        private System.Drawing.Image _mapImage;

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
        /// <param name="document"> A Species information document object.</param>
        /// <param name="mapImage">Bitmap representing a distribution map.</param>
        /// <param name="taxonImage">Taxon image.</param>
        public SpeciesInformationDocumentWriterGraphic(Data.ArtDatabankenService.SpeciesInformationDocument document, System.Drawing.Image mapImage, System.Drawing.Image taxonImage)
        {
            _document = document;
            _mapImage = mapImage;
            _taxonImage = taxonImage;
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
        private Document createPdfDocument(Data.ArtDatabankenService.SpeciesInformationDocument document)
        {
            Document pdfFile = new Document();

            pdfFile.DefaultPageSetup.TopMargin = Unit.FromCentimeter(1.2);
            pdfFile.DefaultPageSetup.RightMargin = Unit.FromCentimeter(1.2);
            pdfFile.DefaultPageSetup.BottomMargin = Unit.FromCentimeter(2.6);
            pdfFile.DefaultPageSetup.FooterDistance = Unit.FromCentimeter(0.8);
            pdfFile.DefaultPageSetup.LeftMargin = Unit.FromCentimeter(1.2);
            pdfFile.DefaultPageSetup.DifferentFirstPageHeaderFooter = true;
            pdfFile.DefaultPageSetup.OddAndEvenPagesHeaderFooter = true;

            pdfFile.Info.Title = "Arfaktablad om " + document.Taxon.Label + " [DyntaxaTaxonId: " + document.Taxon.Id.ToString() + "]";
            pdfFile.Info.Subject = "Arfaktablad med beskrivning, utbredningsuppgifter, ekologi, hot och naturvårdsåtgärder, litteratur";
            pdfFile.Info.Author = document.AuthorAndYear;

            // Get the predefined style Normal.
            Style style = pdfFile.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Arial";
            style.Font.Size = Unit.FromPoint(10);

            //Colors
            MigraDoc.DocumentObjectModel.Color colorOfRedlistPeriodBox = Color.FromCmyk(35, 25, 25, 5); // light grey
            MigraDoc.DocumentObjectModel.Color colorOfRedlistCategoryBox = Color.FromCmyk(5, 95, 100, 0); // Red
            MigraDoc.DocumentObjectModel.Color colorOfFrameLine = Color.FromCmyk(50, 10, 75, 0); // light green
            MigraDoc.DocumentObjectModel.Color colorOfHeaderTitleText = Color.FromCmyk(80, 60, 70, 80); // dark grey
            MigraDoc.DocumentObjectModel.Color colorOfHeaderText = Color.FromCmyk(35, 25, 25, 5); // light grey
            

            Section section = pdfFile.AddSection();

            // Create main frame first
            TextFrame firstMainFrame = section.Headers.FirstPage.AddTextFrame();
            firstMainFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            firstMainFrame.RelativeVertical = RelativeVertical.Margin;
            firstMainFrame.Height = "26cm";
            firstMainFrame.Width = "18,6cm";
            firstMainFrame.LineFormat.Width = 1.5;
            firstMainFrame.LineFormat.Color = colorOfFrameLine;
            // End header first

            // Create header odd
            TextFrame mainFrame = section.Headers.Primary.AddTextFrame();
            mainFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            mainFrame.RelativeVertical = RelativeVertical.Margin;
            mainFrame.Height = "26cm";
            mainFrame.Width = "18,6cm";
            mainFrame.LineFormat.Width = 1.5;
            mainFrame.LineFormat.Color = colorOfFrameLine;
            // End header odd

            // Create header even
            TextFrame evenMainFrame = section.Headers.EvenPage.AddTextFrame();
            evenMainFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            evenMainFrame.RelativeVertical = RelativeVertical.Margin;
            evenMainFrame.Height = "26cm";
            evenMainFrame.Width = "18,6cm";
            evenMainFrame.LineFormat.Width = 1.5;
            evenMainFrame.LineFormat.Color = colorOfFrameLine;
            // End header even

            //Create Line text on first page
            Paragraph borderLineTextFrameFirstPageText;
            TextFrame borderLineTextFrameFirstPage;
            borderLineTextFrameFirstPage = section.AddTextFrame();
            borderLineTextFrameFirstPage.MarginTop = -7;
            borderLineTextFrameFirstPage.MarginLeft = "133mm";
            borderLineTextFrameFirstPageText = borderLineTextFrameFirstPage.AddParagraph("ARTFAKTABLAD");
            borderLineTextFrameFirstPageText.Format.Font.Color = colorOfFrameLine;
            borderLineTextFrameFirstPageText.Format.Font.Size = Unit.FromPoint(12);
            borderLineTextFrameFirstPage.Height = 0;

            // Header 1
            Paragraph commonName;
            if (document.CommonName.IsNotEmpty())
            {
                //Swedish name with first letter upper case in title
                commonName = section.AddParagraph(char.ToUpper(document.CommonName[0]) + document.CommonName.Substring(1));
                commonName.Format.Font.Color = colorOfHeaderTitleText;
            }
            else
            {
                commonName = section.AddParagraph("(Svenskt namn saknas)");
                commonName.Format.Font.Color = colorOfHeaderText;
            }
            commonName.Format.Font.Size = Unit.FromPoint(20);
            commonName.Format.Alignment = ParagraphAlignment.Left;
            commonName.Format.LeftIndent = "0.5cm";
            commonName.Format.RightIndent = "0.5cm";
            commonName.Format.SpaceAfter = "0.2cm";
            commonName.Format.SpaceBefore = "1cm";


            //Header2            
            Paragraph scientificName = section.AddParagraph(document.ScientificName);
            scientificName.Format.Font.Size = Unit.FromPoint(12);
            scientificName.Format.Font.Color = colorOfHeaderTitleText;
            scientificName.Format.Font.Italic = true;
            scientificName.Format.Alignment = ParagraphAlignment.Left;
            scientificName.Format.LeftIndent = "0.5cm";
            scientificName.Format.RightIndent = "0.5cm";
            scientificName.Format.SpaceAfter = "1.2cm";
            scientificName.Format.SpaceBefore = "0.2cm";
            if (document.Taxon.Author.IsNotEmpty())
            {

                scientificName.AddFormattedText(" " + document.Taxon.Author, TextFormat.NotItalic);
            }

            //Redlist information
            Table redlistSpeciesInformation = section.AddTable();
            //redlistSpeciesInformation.Format.LeftIndent = "0.5cm";
            //redlistSpeciesInformation.Borders.Width = 1.5;
            //redlistSpeciesInformation.Borders.Color = colorOfFrameLine;

            Column columnLeft = redlistSpeciesInformation.AddColumn(Unit.FromCentimeter(9.2));
            columnLeft.Format.Alignment = ParagraphAlignment.Left;
            Column columnRight = redlistSpeciesInformation.AddColumn(Unit.FromCentimeter(9.2));
            columnRight.Format.Alignment = ParagraphAlignment.Left;

            Row row = redlistSpeciesInformation.AddRow();
            Cell cellTaxonInfo = row.Cells[0];
            Cell cellCriteria = row.Cells[1];

            List<ParagraphItem> paragraphItems = null;

            //Taxonomic information
            cellTaxonInfo = row.Cells[0];
            Paragraph taxonomicInformation = cellTaxonInfo.AddParagraph();
            taxonomicInformation.Format.Font.Color = colorOfHeaderText;
            taxonomicInformation.Format.Font.Size = Unit.FromPoint(8);
            taxonomicInformation.Format.LeftIndent = "0.5cm";
            taxonomicInformation.Format.RightIndent = "0.5cm";
            taxonomicInformation.Format.SpaceAfter = "0.2cm";
            cellTaxonInfo.VerticalAlignment = VerticalAlignment.Bottom;

            paragraphItems = GetParagraphItems(document.AutomaticTaxonomicParagraph, document.ItalicStringsInText);
            foreach (ParagraphItem item in paragraphItems)
            {
                if (item.Italic)
                {
                    taxonomicInformation.AddFormattedText(item.Text, TextFormat.Italic);
                    taxonomicInformation.AddText(" ");
                }
                else
                {
                    taxonomicInformation.AddText(item.Text);
                    taxonomicInformation.AddText(" ");
                }
            }

            //Redlist Criteria
            if (document.RedlistCriteria.IsNotEmpty())
            {
                Paragraph redlistCriteriaText = cellCriteria.AddParagraph(document.RedlistCriteria);
                redlistCriteriaText.Format.Font.Color = colorOfHeaderText;
                redlistCriteriaText.Format.Font.Size = Unit.FromPoint(8);
                redlistCriteriaText.Format.LeftIndent = "4cm";
                redlistCriteriaText.Format.SpaceAfter = "0.2cm";
                redlistCriteriaText.Format.Alignment = ParagraphAlignment.Left;
                cellCriteria.VerticalAlignment = VerticalAlignment.Bottom;
            }

            // Create Image 
            if (_taxonImage.IsNotNull())
            {
                Paragraph taxonParagraph = section.AddParagraph();
                taxonParagraph.Format.Alignment = ParagraphAlignment.Left;
                taxonParagraph.Format.LeftIndent = "0.5cm";
                taxonParagraph.Format.RightIndent = "0.5cm";
                taxonParagraph.Format.SpaceAfter = "0.2cm";
                _taxonImage.Save("taxon.png");
                Image taxonImage = taxonParagraph.AddImage("taxon.png");
                taxonImage.Height = Unit.FromCentimeter(9);
                taxonImage.LockAspectRatio = true;
                taxonImage.WrapFormat.Style = WrapStyle.TopBottom;
            }

            // Create Map 
            if (_mapImage.IsNotNull())
            {
                Paragraph mapParagraph = section.AddParagraph();
                mapParagraph.Format.Alignment = ParagraphAlignment.Right;
                mapParagraph.Format.LeftIndent = "0.5cm";
                mapParagraph.Format.RightIndent = "0.5cm";
                mapParagraph.Format.SpaceAfter = "0.2cm";
                _mapImage.Save("map.png");
                Image mapImage = mapParagraph.AddImage("map.png");
                mapImage.Height = Unit.FromCentimeter(9);
                mapImage.LockAspectRatio = true;
                mapImage.WrapFormat.Style = WrapStyle.TopBottom;
            }

            //Main document paragraphs
            Paragraph paragraph;

            if (_document.DescriptionParagraph.IsNotEmpty())
            {
                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "0.5cm";
                paragraph.Format.LeftIndent = "0.5cm";
                paragraph.Format.RightIndent = "0.5cm";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddFormattedText("Beskrivning".ToUpper(), TextFormat.Bold);
                paragraph.AddLineBreak();
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
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
                paragraph.Format.SpaceBefore = "0.5cm";
                paragraph.Format.LeftIndent = "0.5cm";
                paragraph.Format.RightIndent = "0.5cm";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddFormattedText("Utbredning och status".ToUpper(), TextFormat.Bold);
                paragraph.AddLineBreak();
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
                paragraph.Format.SpaceBefore = "0.5cm";
                paragraph.Format.LeftIndent = "0.5cm";
                paragraph.Format.RightIndent = "0.5cm";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddFormattedText("Ekologi".ToUpper(), TextFormat.Bold);
                paragraph.AddLineBreak();
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
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
                paragraph.Format.SpaceBefore = "0.5cm";
                paragraph.Format.LeftIndent = "0.5cm";
                paragraph.Format.RightIndent = "0.5cm";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddFormattedText("Hot".ToUpper(), TextFormat.Bold);
                paragraph.AddLineBreak();
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
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
                paragraph.Format.SpaceBefore = "0.5cm";
                paragraph.Format.LeftIndent = "0.5cm";
                paragraph.Format.RightIndent = "0.5cm";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddFormattedText("Åtgärder".ToUpper(), TextFormat.Bold);
                paragraph.AddLineBreak();
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
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
                paragraph.Format.SpaceBefore = "0.5cm";
                paragraph.Format.LeftIndent = "0.5cm";
                paragraph.Format.RightIndent = "0.5cm";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddFormattedText("Övrigt".ToUpper(), TextFormat.Bold);
                paragraph.AddLineBreak();
                paragraph.Format.Alignment = ParagraphAlignment.Justify;
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
                paragraph.Format.SpaceBefore = "0.5cm";
                paragraph.Format.LeftIndent = "0.5cm";
                paragraph.Format.RightIndent = "0.5cm";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddFormattedText("Litteratur".ToUpper(), TextFormat.Bold);
                paragraph.AddLineBreak();


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
            cellTaxonInfo = row.Cells[0];
            Paragraph authorParagraph = cellTaxonInfo.AddParagraph();

            authorParagraph.AddText(getUpdateInformation());

            // Create footer first page
            IOResources.IOResource.slu_logotyp_web1.Save("slu.png");
            Image firstFooterImage = section.Footers.FirstPage.AddImage("slu.png");
            firstFooterImage.Height = Unit.FromCentimeter(1.9);
            firstFooterImage.LockAspectRatio = true;
            firstFooterImage.RelativeVertical = RelativeVertical.Line;
            firstFooterImage.RelativeHorizontal = RelativeHorizontal.Margin;
            firstFooterImage.WrapFormat.Style = WrapStyle.None;

            Paragraph firstFooterParagraph = section.Footers.FirstPage.AddParagraph();
            firstFooterParagraph.AddLineBreak();
            firstFooterParagraph.AddText("ArtDatabanken");
            firstFooterParagraph.Format.Font.Size = 16;
            firstFooterParagraph.Format.Font.Bold = true;
            firstFooterParagraph.Format.Font.Name = "Arial";
            firstFooterParagraph.Format.Alignment = ParagraphAlignment.Right;
            // end footer first page

            // Create footer
            Paragraph footerParagraph = section.Footers.Primary.AddParagraph();
            footerParagraph.AddText("www.slu.se/artdatabanken");
            footerParagraph.Format.Font.Size = 13;
            footerParagraph.Format.Font.Bold = true;
            footerParagraph.Format.Alignment = ParagraphAlignment.Center;

            Paragraph footerPageNo = section.Footers.Primary.AddParagraph();
            footerPageNo.AddPageField();
            footerPageNo.Format.Font.Color = colorOfFrameLine;
            footerPageNo.Format.Font.Size = 13;
            footerPageNo.Format.Alignment = ParagraphAlignment.Right;
            // end footer

            // Create even footer
            Paragraph EvenFooterParagraph = section.Footers.EvenPage.AddParagraph();
            EvenFooterParagraph.AddText("www.slu.se/artdatabanken");
            EvenFooterParagraph.Format.Font.Size = 13;
            EvenFooterParagraph.Format.Font.Bold = true;
            EvenFooterParagraph.Format.Alignment = ParagraphAlignment.Center;

            Paragraph evenfooterPageNo = section.Footers.EvenPage.AddParagraph();
            evenfooterPageNo.AddPageField();
            evenfooterPageNo.Format.Font.Color = colorOfFrameLine;
            evenfooterPageNo.Format.Font.Size = 13;
            evenfooterPageNo.Format.Alignment = ParagraphAlignment.Left;
            // end even footer

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
