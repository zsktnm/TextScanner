using System.Text;

namespace Zsktnm.TextScanner
{
    public class Scanner
    {
        private readonly TextReader reader;
        protected readonly string whitespaces = " \t\n\r\0";


        public static Scanner FromConsole()
        {
            return new Scanner(Console.In);
        }

        public Scanner(TextReader reader)
        {
            this.reader = reader;
        }

        private bool isWhitespace(int characterCode) =>
            whitespaces.Contains((char)characterCode);


        public char ReadChar()
        {
            return (char)reader.Read();
        }

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

        public string? ReadLine()
        {
            return reader.ReadLine();
        }

        public string ReadToEnd()
        {
            return reader.ReadToEnd();
        }

        public T Read<T>(IFormatProvider? provider = null) where T : IParsable<T>
        {
            string? word = ReadBlock();
            return T.Parse(word ?? string.Empty, provider);
        }

        public bool TryRead<T>(out T value, IFormatProvider? provider = null) where T : IParsable<T>
        {
            return T.TryParse(ReadBlock(), provider, out value!);
        }

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

        public IEnumerable<string> ReadBlocks(bool skipOnErrors = false, IFormatProvider? provider = null)
        {
            string? word;
            while ((word = ReadBlock()) != null)
            {
                yield return word;
            }
        }

        public IEnumerable<string> ReadLines(bool skipOnErrors = false,
            IFormatProvider? provider = null)
        {
            string? line;
            while ((line = ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
