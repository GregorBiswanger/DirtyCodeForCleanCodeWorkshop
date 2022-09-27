using FluentAssertions;
using FooBar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooBar.Tests
{
    public class CsvToDictionaryConverterTests
    {
        [Fact]
        public void Should_convert_CSV_to_Dictionary()
        {
            // Arrange
            var csvContent = "header1;header2\nvalue 1;value 2";
            var delimiter = ";";
            var expectedDictionary = new Dictionary<string, string[]>
            {
                {"header1", new string[] { "value 1" } },
                {"header2", new string[] { "value 2" } }
            };
            var csvToDictionaryConverter = new CsvToDictionaryConverter();

            // Act
            var result = csvToDictionaryConverter.Convert(csvContent, delimiter);

            // Assert
            expectedDictionary.All(keyValuePair =>
            {
                if (result.ContainsKey(keyValuePair.Key))
                {
                    var value = result[keyValuePair.Key];
                    return value[0] == keyValuePair.Value[0];
                }

                return false;
            }).Should().BeTrue();
            result.Count.Should().Be(2);
        }
    }
}
