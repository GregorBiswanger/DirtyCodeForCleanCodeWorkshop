using FluentAssertions;
using FooBar.Core;

namespace FooBar.Tests
{
    public class FooBarConverterTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var content = "key1;key2;key3;key4\r\nfirst value 1;first value 2;first value 3;first value 4\r\nsecond value 1;second value 2;second value 3;second value 4\r\n";
            var expectedResult = new Dictionary<string, string[]>
            {
                { "key1", new string[] { "first value 1", "second value 1" } },
                { "key2", new string[] { "first value 2", "second value 2" } },
                { "key3", new string[] { "first value 3", "second value 3" } },
                { "key4", new string[] { "first value 4", "second value 4" } },
            };
            var foobarConverter = new FooBarConverter();

            // Act
            Dictionary<string, string[]> result = foobarConverter.Convert(content, ";");

            // Assert
            expectedResult.All(keyValuePair => result.Contains(keyValuePair));
        }

        [Fact]
        public void Test2()
        {
            // Arrange
            var expectedResult = "key1;key2;key3;key4\r\nfirst value 1;first value 2;first value 3;first value 4\r\nsecond value 1;second value 2;second value 3;second value 4\r\n";
            var content = new Dictionary<string, string[]>
            {
                { "key1", new string[] { "first value 1", "second value 1" } },
                { "key2", new string[] { "first value 2", "second value 2" } },
                { "key3", new string[] { "first value 3", "second value 3" } },
                { "key4", new string[] { "first value 4", "second value 4" } },
            };
            var foobarConverter = new FooBarConverter();

            // Act
            string result = foobarConverter.ConvertBack(content, ";");

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}