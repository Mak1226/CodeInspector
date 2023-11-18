using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ServerlessFunc
{
    public class InsightsUtility
    {
        public static List<string> ByteToList(byte[] byteArray)
        {
            string concatenatedString = Encoding.UTF8.GetString(byteArray);

            // Split the concatenated string back into individual strings
            string[] stringArray = concatenatedString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Convert the string array to a List<string>
            List<string> stringList = new List<string>(stringArray);

            return stringList;
        }
        public static byte[] ListToByte(List<string> list)
        {
            string concatenatedString = string.Join(Environment.NewLine, list);

            byte[] byteArray = Encoding.UTF8.GetBytes(concatenatedString);

            return byteArray;
        }

        public static byte[] ListTupleToByte(List<Tuple<string,string>> list) 
        {
            string jsonString = JsonSerializer.Serialize(list);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        public static Dictionary<string, List<AnalyzerResult>> ConvertAnalysisFileToDictionary(byte[] analysisFile)
        {
            string jsonString = Encoding.UTF8.GetString(analysisFile);
            Dictionary<string, List<AnalyzerResult>> dictionary = JsonSerializer.Deserialize<Dictionary<string, List<AnalyzerResult>>>(jsonString);
            return dictionary;
        }

        public static byte[] ConvertDictionaryToAnalysisFile(Dictionary<string, List<AnalyzerResult>> data)
        {
            string jsonString = JsonSerializer.Serialize(data);
            return Encoding.UTF8.GetBytes(jsonString);
        }

    }
}
