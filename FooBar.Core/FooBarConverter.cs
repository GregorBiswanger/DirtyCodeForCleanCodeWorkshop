namespace FooBar.Core
{
    public class FooBarConverter
    {
        public Dictionary<string, string[]> Convert(string data, string delimiter)
        {
            string[] array = new string[0];

            if (data.Contains("\r\n"))
            {
                array = data.Split("\r\n");
            }
            else if (data.Contains("\r"))
            {
                array = data.Split("\r");
            }
            else if (data.Contains("\n"))
            {
                array = data.Split("\n");
            }

            array = array.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            var keyNames = array[0].Split(delimiter);
            var dataArray = array[1..];

            var result = new Dictionary<string, string[]>();

            for (int i = 0; i < dataArray.Length; i++)
            {
                var d = dataArray[i];
                string[] entries = d.Split(delimiter);
                for (int idx = 0; idx < keyNames.Length; idx++)
                {
                    if (!result.ContainsKey(keyNames[idx]))
                    {
                        result.Add(keyNames[idx], new string[] { entries[idx] });
                    }
                    else
                    {
                        result[keyNames[idx]] = result[keyNames[idx]].Concat(new string[] { entries[idx] }).ToArray();
                    }
                }
            }

            return result;
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