namespace FooBar.Core
{
    public class CsvToDictionaryConverter
    {
        public Dictionary<string, string[]> Convert(string csvContent, string delimiter)
        {
            string[] records = CreateRecords(csvContent);
            records = FilterEmptyRecords(records);

            var headerColumns = GetHeaderColumns(records, delimiter);
            var valueRecords = GetValueRecords(records);

            return CreateRecordDictionary(headerColumns, valueRecords, delimiter);
        }

        private string[] CreateRecords(string csvContent)
        {
            if (csvContent.Contains("\r\n"))
            {
                return csvContent.Split("\r\n");
            }

            if (csvContent.Contains("\r"))
            {
                return csvContent.Split("\r");
            }

            if (csvContent.Contains("\n"))
            {
                return csvContent.Split("\n");
            }

            return Array.Empty<string>();
        }

        private string[] FilterEmptyRecords(string[] lines)
        {
            return lines.Where(line => line is { Length: > 0 }).ToArray();
        }

        private string[] GetHeaderColumns(string[] lines, string delimiter)
        {
            return lines.First().Split(delimiter);
        }

        private string[] GetValueRecords(string[] records)
        {
            return records[1..];
        }

        private Dictionary<string, string[]> CreateRecordDictionary(string[] headerColumns, string[] valueRecords, string delimiter)
        {
            var recordDictionary = headerColumns.ToDictionary(header => header, _ => new string[valueRecords.Length]);

            for (int valueRecordIndex = 0; valueRecordIndex < valueRecords.Length; valueRecordIndex++)
            {
                string? valueRecord = valueRecords[valueRecordIndex];
                string[] values = valueRecord.Split(delimiter);

                for (int headerColumnsIndex = 0; headerColumnsIndex < headerColumns.Length; headerColumnsIndex++)
                {
                    var currentHeader = headerColumns[headerColumnsIndex];
                    var currentValue = values[headerColumnsIndex];

                    recordDictionary[currentHeader][valueRecordIndex] = currentValue;
                }
            }

            return recordDictionary;
        }

        public string ConvertBack(Dictionary<string, string[]> data, string delimiter)
        {
            string result = "";
            var keys = data.Keys.ToArray();

            foreach (var key in keys)
            {
                if (result.Length == 0)
                {
                    result += key;
                }
                else
                {
                    result += $"{delimiter}{key}";
                }
            }

            result += "\r\n";

            for (int i = 0; i < data.First().Value.Length; i++)
            {
                var lineStr = "";

                foreach (var line in data)
                {
                    if (lineStr.Length == 0)
                    {
                        lineStr += line.Value[i];
                    }
                    else
                    {
                        lineStr += $"{delimiter}{line.Value[i]}";
                    }
                }

                result += $"{lineStr}\r\n";
            }

            return result;
        }
    }
}