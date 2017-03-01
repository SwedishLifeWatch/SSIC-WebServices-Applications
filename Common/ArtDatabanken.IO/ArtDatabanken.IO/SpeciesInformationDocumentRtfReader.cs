using System;
using System.Text;
using System.Windows.Forms;

namespace ArtDatabanken.IO
{
    /// <summary>
    /// This class includes methods for updating an existinc insance of a Species Information Document from a rtf file.
    /// </summary>
    public class SpeciesInformationDocumentRtfReader
    {
        /// <summary>
        /// Paragraph identifiers.
        /// </summary>
        public struct ParagraphIdentifier
        {
            /// <summary>Line.</summary>
            public const string LINE = "______________________________________________________________________________";
            /// <summary>Description.</summary>
            public const string DESCRIPTION = "Beskrivning.";
            /// <summary>Distribution.</summary>
            public const string DISTRIBUTION = "Utbredning och status.";
            /// <summary>Ecology.</summary>
            public const string ECOLOGY = "Ekologi.";
            /// <summary>Threats.</summary>
            public const string THREATS = "Hot.";
            /// <summary>Measures.</summary>
            public const string MEASURES = "Åtgärder.";
            /// <summary>Extra.</summary>
            public const string EXTRA = "Övrigt.";
            /// <summary>References.</summary>
            public const string REFERECES = "Litteratur";
        }

        private SpeciesInformationDocument _document;
        private RichTextBox _textBox;
        private String _rtfDocument = "";

        /// <summary>
        /// Creates an instance of a Species Information Document Rtf reader. This object can then update the properties of the species information document based on the content of a specified rtf-file.
        /// </summary>
        /// <param name="document">An instance of Species Information Document</param>
        public SpeciesInformationDocumentRtfReader(SpeciesInformationDocument document)
        {
            _document = document;
            _textBox = new RichTextBox();
        }

        private void updateDocument()
        {
            if (_rtfDocument.IsEmpty())
            {
                return;
            }

            bool referencesFound = false;
            StringBuilder referenceParagraph = new StringBuilder();
            String[] parts = _rtfDocument.Split('\n');
            foreach (String part in parts)
            {
                if (referencesFound)
                {
                    String reference = part.Trim();
                    if (reference != "" && reference != ParagraphIdentifier.LINE)
                    {
                        referenceParagraph.AppendLine(reference);
                    }
                    else
                    {
                        referencesFound = false;
                    }
                }
                else
                {
                    if (part.IndexOf(ParagraphIdentifier.DESCRIPTION) > -1)
                    {
                        String paragraph = part.Substring(ParagraphIdentifier.DESCRIPTION.Length).Trim();
                        _document.DescriptionParagraph = paragraph;
                    }

                    if (part.IndexOf(ParagraphIdentifier.DISTRIBUTION) > -1)
                    {
                        String paragraph = part.Substring(ParagraphIdentifier.DISTRIBUTION.Length).Trim();
                        _document.DistributionParagraph = paragraph;
                    }

                    if (part.IndexOf(ParagraphIdentifier.ECOLOGY) > -1)
                    {
                        String paragraph = part.Substring(ParagraphIdentifier.ECOLOGY.Length).Trim();
                        _document.EcologyParagraph = paragraph;
                    }

                    if (part.IndexOf(ParagraphIdentifier.THREATS) > -1)
                    {
                        String paragraph = part.Substring(ParagraphIdentifier.THREATS.Length).Trim();
                        _document.ThreatsParagraph = paragraph;
                    }

                    if (part.IndexOf(ParagraphIdentifier.MEASURES) > -1)
                    {
                        String paragraph = part.Substring(ParagraphIdentifier.MEASURES.Length).Trim();
                        _document.MeasuresParagraph = paragraph;
                    }

                    if (part.IndexOf(ParagraphIdentifier.EXTRA) > -1)
                    {
                        String paragraph = part.Substring(ParagraphIdentifier.EXTRA.Length).Trim();
                        _document.ExtraParagraph = paragraph;
                    }

                    if (part.IndexOf(ParagraphIdentifier.REFERECES) == 0)
                    {
                        referencesFound = true;
                    }
                }
            }
            _document.ReferenceParagraph = referenceParagraph.ToString();
        }

        /// <summary>
        /// A method that reads a rtf file into the Species Information Document object
        /// </summary>
        /// <param name="fileName">The full name, including path and extension of the file</param>
        public void Read(String fileName)
        {
            _textBox.LoadFile(fileName);
            _rtfDocument = _textBox.Text;
            updateDocument();
        }
    }
}
