using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassportVerification
{
    public class PassportCheckDigit
    {

        private readonly static Dictionary<char, byte> _charEncodings;

        /// <summary>
        /// Create encodings. Will be created once on startup (including IIS recycles)
        /// </summary>
        static PassportCheckDigit()
        {
            const byte asciiAlphaOffset = 55;   //the check digit calculation expects A=10...Z=35, which is the ASCII code offeset by -55
            const byte alphaCodeStart = 10;
            const byte alphaCodeEnd = 35;

            _charEncodings = new Dictionary<char, byte>();
            _charEncodings.Add('<', 0); // Filler char encoded to 0

            //digits 0-9 encoded to 0-9
            for (byte digit = 0; digit < 10; digit++)
            {
                _charEncodings.Add(digit.ToString()[0], digit);
            }

            //uppercase alphas encoded to 10-35
            for (byte code = alphaCodeStart; code <= alphaCodeEnd; code++)
            {
                _charEncodings.Add((char)(code + asciiAlphaOffset), code);
            }

            //all other characters are invalid.
        }

        /// <summary>
        /// Calculates the CheckDigit of the given field using the formula:
        ///   - Get char encoding from encodings dictionary
        ///   - apply weightings by multiplying by each character code (7, 3, 1, 7, 3, 1, 7, 3, 1...)
        ///   - Sum each value
        ///   - Final CheckDigit is the remainder of the sum / 10
        /// </summary>
        /// <param name="field">Field value from which CheckDigit is to be calculated.  Valid characters are ([0-9A-Z<])</param>
        /// <returns>Calculated CheckDigit.</returns>
        internal static int GetCheckDigit(string field)
        {
            const byte weight1 = 7;             //weighting for every 3rd character starting at position 1
            const byte weight2 = 3;             //weighting for every 3rd character starting at position 2

            //validate input
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException();
            }
            if (field.Any(c => !_charEncodings.Keys.Contains(c)))
            {
                throw new ArgumentException("Valid characters are <, 0-9, A-Z");
            }

            int checkDigit = 0;
            char[] fieldCharacters = field.ToCharArray();

            for (int charIndex = 0; charIndex < fieldCharacters.Length; charIndex++)
            {
                byte charEncoding = _charEncodings[fieldCharacters[charIndex]];
                
                //apply weightings
                if (charIndex % 3 == 0)
                {
                    //every 3rd character starting at poition 1 (index 0) to be multiplied by first weighting
                    checkDigit += (charEncoding * weight1);
                }
                else if ((charIndex - 1) % 3 == 0)
                {
                    //every 3rd character starting at poition 2 (index 1) to be multiplied by second weighting
                    checkDigit += (charEncoding * weight2);
                }
                else
                {
                    //every 3rd character starting at poition 3 (index 2) has a weighting of 1, 
                    //for efficiency we will not define this - simply not mulitiply
                    checkDigit += charEncoding;
                }

            }
            //the checkDigit is the remainder of the final total / 10.
            return checkDigit % 10;
        }


        ///=====================================================================================///
        /// This following has been left to show evidence of performance consideration          ///
        /// to an alternate approch to this function                                            ///
        /// see also \PassportVerificationApi\PassportVerificationTests\CheckDigitUnitTests.cs  ///
        ///=====================================================================================///

        /// <summary>
        /// Calculates the CheckDigit of the given field using the formula:
        ///   - Encode alphas to number codes (A=10...Z=35)
        ///   - Convert Filler to zero
        ///   - Multiply relevant weightings by each character (7, 3, 1, 7, 3, 1, 7, 3, 1...)
        ///   - Sum each value
        ///   - Final CheckDigit is the remainder of the sum / 10
        /// </summary>
        /// <param name="field">Field value from which CheckDigit is to be calculated.  This is a assumed to be pre-validated ([0-9A-Z<])</param>
        /// <returns>Calculated CheckDigit</returns>
        internal static int GetCheckDigitAlternate(string field)
        {
            const byte asciiDigitOffset = 48;   //the check digit calculation expects 0=0...9=9, which is the ASCII code offeset by -48
            const byte asciiAlphaOffset = 55;   //the check digit calculation expects A=10...Z=35, which is the ASCII code offeset by -55
            const byte asciiFillerChar = 60;    //ascii code for "<" character used for blanks
            const byte weight1 = 7;             //weighting for every 3rd character starting at position 1
            const byte weight2 = 3;             //weighting for every 3rd character starting at position 2

            //validate input
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException();
            }
            if (field.Any(c => !_charEncodings.Keys.Contains(c)))
            {
                throw new ArgumentException("Valid characters are <, 0-9, A-Z");
            }

            int checkDigit = 0;

            //get the array of ASCII numeric representations of each character
            byte[] charEncodings = Encoding.ASCII.GetBytes(field.ToCharArray());

            for (int charIndex = 0; charIndex < field.Length; charIndex++)
            {

                if (charEncodings[charIndex] == asciiFillerChar)
                {
                    //add nothing to the check digit for the filler char ("<") 
                    continue;
                }

                //apply ascii offset
                if (charEncodings[charIndex] < asciiDigitOffset + 10)
                {
                    //apply digit offset (-48)
                    charEncodings[charIndex] -= asciiDigitOffset;
                }
                else
                {
                    //apply alpha offset (-55)
                    charEncodings[charIndex] -= asciiAlphaOffset;
                }

                //apply weightings
                if (charIndex % 3 == 0)
                {
                    //every 3rd character starting at poition 1 (index 0) to be multiplied by first weighting
                    checkDigit += (charEncodings[charIndex] * weight1);
                }
                else if ((charIndex - 1) % 3 == 0)
                {
                    //every 3rd character starting at poition 2 (index 1) to be multiplied by second weighting
                    checkDigit += (charEncodings[charIndex] * weight2);
                }
                else
                {
                    //add current value to check digit
                    checkDigit += charEncodings[charIndex];
                }
            }

            //the checkDigit is the remainder of the final total / 10.
            return checkDigit % 10;
        }

    }
}
