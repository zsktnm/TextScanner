using System.Globalization;
using Zsktnm.TextScanner;

namespace Zsktnm.TextScanner.Tests
{
    public class FilesTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SingleIntValue()
        {
            using (StreamReader reader = new("Inputs/single_int.txt"))
            {
                Scanner scanner = new Scanner(reader);
                int expected = 1234;
                int actual = scanner.Read<int>();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void SingleIntValueWithSpaces()
        {
            using (StreamReader reader = new("Inputs/single_int_spaces.txt"))
            {
                Scanner scanner = new Scanner(reader);
                int expected = 1234;
                int actual = scanner.Read<int>();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void SingleWord()
        {
            using (StreamReader reader = new("Inputs/single_word.txt"))
            {
                Scanner scanner = new Scanner(reader);
                string expected = "hello!!!";
                string? actual = scanner.ReadBlock();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void NothingToReadEmptyFile()
        {
            using (StreamReader reader = new("Inputs/empty.txt"))
            {
                Scanner scanner = new Scanner(reader);
                string? actual = scanner.ReadBlock();
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        public void NothingToReadOnlySpaces()
        {
            using (StreamReader reader = new("Inputs/only_spaces.txt"))
            {
                Scanner scanner = new Scanner(reader);
                string? actual = scanner.ReadBlock();
                Assert.That(actual, Is.Empty);
            }
        }

        [Test]
        public void SingleDouble()
        {
            using (StreamReader reader = new("Inputs/single_double.txt"))
            {
                Scanner scanner = new Scanner(reader);
                double expected = 3.14;
                double actual = scanner.Read<double>(CultureInfo.GetCultureInfo("en"));
                Assert.That(actual, Is.EqualTo(expected).Within(0.001));
            }
        }

        [Test]
        public void SingleDoubleWithComma()
        {
            using (StreamReader reader = new("Inputs/single_double_with_comma.txt"))
            {
                Scanner scanner = new Scanner(reader);
                double expected = 3.14;
                double actual = scanner.Read<double>(CultureInfo.GetCultureInfo("ru"));
                Assert.That(actual, Is.EqualTo(expected).Within(0.001));
            }
        }

        [Test]
        public void SingleLine()
        {
            using (StreamReader reader = new("Inputs/single_double_with_comma.txt"))
            {
                Scanner scanner = new Scanner(reader);
                string expected = "3,14";
                string? actual = scanner.ReadLine();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void SingleDate()
        {
            using (StreamReader reader = new("Inputs/single_date.txt"))
            {
                Scanner scanner = new Scanner(reader);
                DateTime expected = new DateTime(2000, 11, 30);
                DateTime actual = scanner.Read<DateTime>(CultureInfo.GetCultureInfo("ru"));
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void MultipleIntsReadAll()
        {
            using (StreamReader reader = new("Inputs/multiple_ints.txt"))
            {
                Scanner scanner = new Scanner(reader);
                int[] expected = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                int[] actual = scanner.ReadValues<int>().ToArray();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void MultipleIntsReadFirstFive()
        {
            using (StreamReader reader = new("Inputs/multiple_ints.txt"))
            {
                Scanner scanner = new Scanner(reader);
                int[] expected = { 1, 2, 3, 4, 5 };
                int[] actual = scanner.ReadValues<int>().Take(5).ToArray();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void MultipleIntsDontSkipErrors()
        {
            using (StreamReader reader = new("Inputs/multiple_ints_with_errors.txt"))
            {
                Scanner scanner = new Scanner(reader);
                Assert.Throws<FormatException>(() => scanner.ReadValues<int>().Take(5).ToArray());
            }
        }

        [Test]
        public void MultipleIntsSkipErrors()
        {
            using (StreamReader reader = new("Inputs/multiple_ints_with_errors.txt"))
            {
                Scanner scanner = new Scanner(reader);
                int[] expected = { 1, 2, 3, 4, 6, 8, 9 };
                int[] actual = scanner.ReadValues<int>(skipOnErrors: true).ToArray();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void MultipleIntsWithErrorsAndDefaultValue()
        {
            using (StreamReader reader = new("Inputs/multiple_ints_with_errors.txt"))
            {
                Scanner scanner = new Scanner(reader);
                int[] expected = { 1, 2, 3, 4, -1, 6, -1, 8, 9 };
                int[] actual = scanner.ReadValues(-1).ToArray();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void MultipleIntsWithErrorsSkipErrorsTakeSix()
        {
            using (StreamReader reader = new("Inputs/multiple_ints_with_errors.txt"))
            {
                Scanner scanner = new Scanner(reader);
                int[] expected = { 1, 2, 3, 4, 6, 8 };
                int[] actual = scanner.ReadValues<int>(skipOnErrors: true).Take(6).ToArray();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void ReadThreeLines()
        {
            using (StreamReader reader = new("Inputs/multiple_ints.txt"))
            {
                Scanner scanner = new Scanner(reader);
                string[] expected = { "1 2 3", "\t", "4 5 6" };
                string[] actual = scanner.ReadLines().Take(3).ToArray();
                Assert.That(actual, Is.EqualTo(expected));
            }
        }



    }
}