using System;
using System.Text;

namespace ArtDatabanken
{
    /// <summary>
    /// This class creates text from it's sub parts.
    /// </summary>
    public class TextBuilder
    {
        private Int32 _numberOfAndParts;
        private Int32 _remainingNumberOfAndParts;
        private StringBuilder _text;

        /// <summary>
        /// Create a TextBuilder instance.
        /// </summary>
        public TextBuilder()
        {
            _text = new StringBuilder();
            _numberOfAndParts = 0;
            _remainingNumberOfAndParts = 0;
        }

        /// <summary>
        /// Add text, surrounded by brackets.
        /// </summary>
        /// <param name="text">Text to add.</param>
        public void AddAndText(String text)
        {
            if (text.IsNotEmpty() && (0 < _remainingNumberOfAndParts))
            {
                // Add delimiter
                if (_numberOfAndParts == _remainingNumberOfAndParts)
                {
                    _text.Append(" ");
                }
                else if (_remainingNumberOfAndParts == 1)
                {
                    _text.Append(" och ");
                }
                else
                {
                    _text.Append(", ");
                }

                // Add text.
                _text.Append(text);

                // End handling.
                _remainingNumberOfAndParts--;
            }
        }

        /// <summary>
        /// Add text, surrounded by brackets.
        /// </summary>
        /// <param name="text">Text to add.</param>
        public void AddBracketedText(String text)
        {
            if (text.IsNotEmpty())
            {
                _text.Append(" (" + ChangeCase(text.Trim(), false) + ")");
            }
        }

        /// <summary>
        /// Add a sentence.
        /// </summary>
        /// <param name="sentence">Sentence to add.</param>
        public void AddSentence(String sentence)
        {
            if (sentence.IsNotEmpty())
            {
                sentence = ChangeCase(sentence.Trim(), true);

                if (!sentence.EndsWith("."))
                {
                    sentence += ".";
                }

                if (_text.Length == 0)
                {
                    _text.Append(sentence);
                }
                else
                {
                    _text.Append(" " + sentence);
                }
            }
        }

        /// <summary>
        /// Add text.
        /// </summary>
        /// <param name="text">Text to add.</param>
        public void AddText(String text)
        {
            if (text.IsNotEmpty())
            {
                _text.Append(text);
            }
        }

        /// <summary>
        /// Start writing a sentence.
        /// The sentence has a leading text and 0 or more
        /// text parts that are put together with "and".
        /// </summary>
        /// <param name="text">Text to add.</param>
        /// <param name="numberOfAndParts">Number of and parts in the sentence.</param>
        public void BeginAndSentence(String text, Int32 numberOfAndParts)
        {
            if (0 < numberOfAndParts)
            {
                if (_remainingNumberOfAndParts > 0)
                {
                    _numberOfAndParts += numberOfAndParts;
                    _remainingNumberOfAndParts += numberOfAndParts;
                }
                else
                {
                    _numberOfAndParts = numberOfAndParts;
                    _remainingNumberOfAndParts = numberOfAndParts;
                }

                if (0 < _text.Length)
                {
                    _text.Append(" ");
                }

                if (text.IsNotEmpty())
                {
                    text = ChangeCase(text.Trim(), true);
                    _text.Append(text);
                }
            }
        }

        /// <summary>
        /// Start writing a text that has 0 or more
        /// text parts that are put together with "and".
        /// </summary>
        /// <param name="numberOfAndParts">Number of and parts in the sentence.</param>
        public void BeginAndText(Int32 numberOfAndParts)
        {
            if (0 < numberOfAndParts)
            {
                if (_remainingNumberOfAndParts > 0)
                {
                    _numberOfAndParts += numberOfAndParts;
                    _remainingNumberOfAndParts += numberOfAndParts;
                }
                else
                {
                    _numberOfAndParts = numberOfAndParts;
                    _remainingNumberOfAndParts = numberOfAndParts;
                }
                if (0 < _text.Length)
                {
                    _text.Append(" ");
                }
            }
        }

        /// <summary>
        /// Start writing a sentence.
        /// </summary>
        /// <param name="text">Text to add.</param>
        public void BeginSentence(String text)
        {
            if (_text.Length > 0)
            {
                _text.Append(" ");
            }
            if (text.IsNotEmpty())
            {
                text = ChangeCase(text.TrimStart(), true);
                _text.Append(text);
            }
        }

        /// <summary>
        /// Make sure that the first letter in the text
        /// has the required case.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="toUpper">Indicates if first letter should be upper or lower case.</param>
        /// <returns>Updated text.</returns>
        private String ChangeCase(String text, Boolean toUpper)
        {
            if (text.IsNotEmpty())
            {
                if (toUpper)
                {
                    text = text.Substring(0, 1).ToUpper() + text.Substring(1);
                }
                else
                {
                    text = text.Substring(0, 1).ToLower() + text.Substring(1);
                }
            }
            return text;
        }

        /// <summary>
        /// Finish writing a sentence.
        /// </summary>
        public void EndSentence()
        {
            EndSentence(null);
        }

        /// <summary>
        /// Finish writing a sentence.
        /// </summary>
        /// <param name="text">Text to add.</param>
        public void EndSentence(String text)
        {
            if (text.IsNotEmpty())
            {
                text = text.TrimEnd();
                if (_text.ToString().EndsWith(". "))
                {
                    text = ChangeCase(text, true);
                }
                _text.Append(text);
            }
            if (!_text.ToString().EndsWith("."))
            {
                _text.Append(".");
            }
        }

        /// <summary>
        /// Get text.
        /// </summary>
        public override string ToString()
        {
            return _text.ToString();
        }
    }
}
