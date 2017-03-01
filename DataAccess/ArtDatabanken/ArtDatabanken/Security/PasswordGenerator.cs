using System;
using System.Security.Cryptography;
using System.Text;

namespace ArtDatabanken.Security
{
    /// <summary>
    /// This class generates a password. It is not validated to create strong passwords.
    /// This script was found at http://www.codeproject.com/csharp/pwdgen.asp
    /// </summary>
    public class PasswordGenerator
    {
        private const int DefaultMinimum = 8;
        private const int DefaultMaximum = 30;
        private const int UBoundDigit = 61; // Det högsta index i pwdCharArray som är t.o.m. siffrorna.

        private RNGCryptoServiceProvider rng;
        private int minSize;
        private int maxSize;
        private bool hasRepeating;
        private bool hasConsecutive;
        private bool hasSymbols;
        private bool includeSymbol;
        private string exclusionSet;
        //private char[] pwdCharArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?".ToCharArray();
        private char[] pwdCharArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&()=+?".ToCharArray();



        /// <summary>
        /// Minimilängden för lösenordet. Default är 8.
        /// </summary>
        public int Minimum
        {
            get { return this.minSize; }
            set
            {
                this.minSize = value;
                if (PasswordGenerator.DefaultMinimum > this.minSize)
                {
                    this.minSize = PasswordGenerator.DefaultMinimum;
                }
            }
        }

        /// <summary>
        /// Maxlängden för lösenordet. Default är 30
        /// </summary>
        public int Maximum
        {
            get { return this.maxSize; }
            set
            {
                this.maxSize = value;
                if (this.minSize >= this.maxSize)
                {
                    this.maxSize = PasswordGenerator.DefaultMaximum;
                }
            }
        }

        /// <summary>
        /// Gets eller sets huruvida symboler ska uteslutas ur lösenordet. Default är true
        /// <c>true</c> om de ska uteslutas; annars, <c>false</c>.
        /// </summary>
        public bool ExcludeSymbols
        {
            get { return this.hasSymbols; }
            set { this.hasSymbols = value; }
        }

        /// <summary>
        /// Gets eller sets huruvida en symbol ska vara med i lösenordet. Default är true
        /// <c>true</c> om det ska vara en symbol; annars, <c>false</c>.
        /// </summary>
        public bool IncludeSymbol
        {
            get { return this.includeSymbol; }
            set { this.includeSymbol = value; }
        }

        /// <summary>
        /// Gets eller sets huruvida tecken kan upprepas i lösenordet. Default är true.
        /// <c>true</c> om de får det; annars, <c>false</c>.
        /// </summary>
        public bool RepeatCharacters
        {
            get { return this.hasRepeating; }
            set { this.hasRepeating = value; }
        }

        /// <summary>
        /// Gets eller sets huruvida flera efterföljande tecken i lösenordet får vara samma. Default är false;
        /// <c>true</c> om det är giltigt; annars, <c>false</c>.
        /// </summary>
        public bool ConsecutiveCharacters
        {
            get { return this.hasConsecutive; }
            set { this.hasConsecutive = value; }
        }

        /// <summary>
        /// Gets eller sets de tecken som skall uteslutas ur lösenordet.
        /// Sträng med tecknen som ska utesluta tex.  abcde123[]}\
        /// </summary>
        public string Exclusions
        {
            get { return this.exclusionSet; }
            set { this.exclusionSet = value; }
        }


        /// <summary>
        /// Create a PasswordGenerator instance.
        /// </summary>
        public PasswordGenerator()
        {
            this.Minimum = DefaultMinimum;
            this.Maximum = DefaultMaximum;
            this.ConsecutiveCharacters = false;
            this.RepeatCharacters = true;
            this.ExcludeSymbols = true;
            this.Exclusions = null;
            this.IncludeSymbol = true;

            rng = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Genererar ett lösenord
        /// </summary>
        public string Generate()
        {
            // Pick random length between minimum and maximum   
            int pwdLength = GetCryptographicRandomNumber(this.Minimum, this.Maximum);

            int symbolPosition = GetCryptographicRandomNumber(this.Minimum, pwdLength - 1);

            StringBuilder pwdBuffer = new StringBuilder();
            pwdBuffer.Capacity = this.Maximum;

            // Generate random characters
            char lastCharacter, nextCharacter;

            // Initial dummy character flag
            lastCharacter = nextCharacter = '\n';

            for (int i = 0; i < pwdLength; i++)
            {
                if (i == symbolPosition)
                {
                    nextCharacter = GetRandomSymbol();
                }
                else
                {
                    nextCharacter = GetRandomCharacter();
                }

                if (false == this.ConsecutiveCharacters)
                {
                    while (lastCharacter == nextCharacter)
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                if (false == this.RepeatCharacters)
                {
                    string temp = pwdBuffer.ToString();
                    int duplicateIndex = temp.IndexOf(nextCharacter);
                    while (-1 != duplicateIndex)
                    {
                        nextCharacter = GetRandomCharacter();
                        duplicateIndex = temp.IndexOf(nextCharacter);
                    }
                }

                if ((null != this.Exclusions))
                {
                    while (-1 != this.Exclusions.IndexOf(nextCharacter))
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                pwdBuffer.Append(nextCharacter);
                lastCharacter = nextCharacter;
            }

            if (null != pwdBuffer)
            {
                return pwdBuffer.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Get a random number.
        /// Assumes lBound >= 0 and lBound less than uBound.
        /// </summary>
        /// <param name="lBound">Lower bound for random number.</param>
        /// <param name="uBound">Upper bound for random number.</param>
        /// <returns>Returns an int >= lBound and less than uBound.</returns>
        protected int GetCryptographicRandomNumber(int lBound, int uBound)
        {
            uint urndnum;
            byte[] rndnum = new Byte[4];
            if (lBound >= uBound)
            {
                return lBound;
            }
            else if (lBound == uBound - 1)
            {
                // test for degenerate case where only lBound can be returned
                return lBound;
            }

            uint xcludeRndBase = (uint.MaxValue -
                (uint.MaxValue % (uint)(uBound - lBound)));

            do
            {
                rng.GetBytes(rndnum);
                urndnum = System.BitConverter.ToUInt32(rndnum, 0);
            } while (urndnum >= xcludeRndBase);

            return (int)(urndnum % (uBound - lBound)) + lBound;
        }

        /// <summary>
        /// Get a random character.
        /// </summary>
        /// <returns>Returns a random character.</returns>
        protected char GetRandomCharacter()
        {
            int upperBound = pwdCharArray.GetUpperBound(0);

            if (true == this.ExcludeSymbols)
            {
                upperBound = PasswordGenerator.UBoundDigit;
            }

            int randomCharPosition = GetCryptographicRandomNumber(
                pwdCharArray.GetLowerBound(0), upperBound);

            char randomChar = pwdCharArray[randomCharPosition];

            return randomChar;
        }

        /// <summary>
        /// Get a random symbol character.
        /// </summary>
        /// <returns>Returns a random symbol character.</returns>
        protected char GetRandomSymbol()
        {
            int lowerBound = PasswordGenerator.UBoundDigit + 1;
            int randomCharPosition = GetCryptographicRandomNumber(lowerBound, pwdCharArray.GetUpperBound(0));
            char randomChar = pwdCharArray[randomCharPosition];
            return randomChar;
        }
    }
}
