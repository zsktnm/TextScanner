using System.Text;

namespace Zsktnm.TextScanner
{
    public class Scanner
    {
        private readonly TextReader reader;
        protected readonly string whitespaces = " \t\n\r\0";


        /// <summary>
        /// Create scanner from console
        /// </summary>
        public static Scanner FromConsole()
        {
            return new Scanner(Console.In);
        }

        /// <summary>
        /// Create new instance of scanner.
        /// </summary>
        /// <param name="reader">TextReader realization</param>
        public Scanner(TextReader reader)
        {
            this.reader = reader;
        }

        private bool isWhitespace(int characterCode) =>
            whitespaces.Contains((char)characterCode);


        /// <summary>
        /// Read one character
        /// </summary>
        public char ReadChar()
        {
            return (char)reader.Read();
        }

        /// <summary>
        /// Read one block of symbols. Block is the sequence of characters delimited by whitespaces
        /// </summary>
        /// <returns>Returns first block of symbols from current position</returns>
        public string? ReadBlock()
        {
            var result = new StringBuilder();
            int count = 0;

            while (isWhitespace(reader.Peek()))
            {
                reader.Read();
                count++;
            }

            while (true)
            {
                int input = reader.Read();
                if (isWhitespace((char)input) || input == -1)
                {
                    break;
                }
                result.Append((char)input);
                count++;
            }

            if (count == 0)
            {
                return null;
            }

            return result.ToString();
        }


        /// <summary>
        /// Read one line from current position.
        /// </summary>
        public string? ReadLine()
        {
            return reader.ReadLine();
        }

        /// <summary>
        /// Read the input to end from current position.
        /// </summary>
        public string ReadToEnd()
        {
            return reader.ReadToEnd();
        }


        /// <summary>
        /// Read first value of T from current position. 
        /// </summary>
        /// <typeparam name="T">T is the value of IParsable&lt;T&gt;</typeparam>
        /// <param name="provider">IFormatProvider to convertation by `Parse` method</param>
        /// <returns>Returns first value of type T</returns>
        public T Read<T>(IFormatProvider? provider = null) where T : IParsable<T>
        {
            string? word = ReadBlock();
            return T.Parse(word ?? string.Empty, provider);
        }

        /// <summary>
        /// Read first value of T from current position. 
        /// If convertation is success returns true, otherwise returns false.
        /// </summary>
        /// <typeparam name="T">T is the value of IParsable&lt;T&gt;</typeparam>
        /// <param name="value">Output parameter to store the result of convertation</param>
        /// <param name="provider">IFormatProvider to convertation by `TryParse` method</param>
        /// <returns>Returns true if convertation is success, false if not.</returns>
        public bool TryRead<T>(out T value, IFormatProvider? provider = null) where T : IParsable<T>
        {
            return T.TryParse(ReadBlock(), provider, out value!);
        }


        /// <summary>
        /// Read multiple values of type T from input. 
        /// </summary>
        /// <typeparam name="T">T is the value of IParsable&lt;T&gt;</typeparam>
        /// <param name="skipOnErrors">Skip incorrect inputs if true. Throws FormatException if false.</param>
        /// <param name="provider">IFormatProvider to convertation by `TryParse` method</param>
        /// <returns>Enumerable of type T</returns>
        /// <exception cref="FormatException">Throws on incorrect input, when `skipOnErrors` param is false</exception>
        public IEnumerable<T> ReadValues<T>(bool skipOnErrors = false, IFormatProvider? provider = null)
            where T : IParsable<T>
        {
            string? word;
            while ((word = ReadBlock()) != null)
            {
                T result;
                if (word == string.Empty)
                {
                    continue;
                }
                if (T.TryParse(word, provider, out result!))
                {
                    yield return result;
                }
                else if (!skipOnErrors)
                {
                    throw new FormatException("Invalid format of value type " + result.GetType().Name);
                }
            }
        }

        /// <summary>
        /// Read multiple values of type T from input. 
        /// </summary>
        /// <typeparam name="T">T is the value of IParsable&lt;T&gt;</typeparam>
        /// <param name="defaultValue">Default value, used on incorrect inputs</param>
        /// <param name="provider">IFormatProvider to convertation by `TryParse` method</param>
        /// <returns>Enumerable of type T</returns>
        public IEnumerable<T> ReadValues<T>(T defaultValue, IFormatProvider? provider = null)
            where T : IParsable<T>
        {
            string? word;
            while ((word = ReadBlock()) != null)
            {
                T result;
                if (word == string.Empty)
                {
                    continue;
                }
                if (T.TryParse(word, provider, out result!))
                {
                    yield return result;
                }
                else
                {
                    yield return defaultValue;
                }
            }
        }


        /// <summary>
        /// Read multiple blocks from input. Block is the sequence of characters delimited by whitespaces
        /// </summary>
        /// <returns>Enumerable of strings</returns>
        public IEnumerable<string> ReadBlocks()
        {
            string? word;
            while ((word = ReadBlock()) != null)
            {
                yield return word;
            }
        }

        /// <summary>
        /// Read multiple lines from input.
        /// </summary>
        /// <returns>Enumerable of strings</returns>
        public IEnumerable<string> ReadLines()
        {
            string? line;

            while ((line = ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
