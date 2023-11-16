/******************************************************************************
 * Filename     = AnalyzerSerializer.cs
 * 
 * Author       = Susan
 *
 * Product      = Analyzer
 * 
 * Project      = Content
 *
 * Description  = Serializer for Analyzer Results
*****************************************************************************/using Analyzer;
using Networking.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace Content.Encoder
{
    /// <summary>
    /// Serialises an object by converting it into an XML
    /// For now it's just Tuple<Dictionary<string, string>
    /// </summary>
    internal class AnalyzerResultSerializer : ISerializer
    {
        public string Serialize<T>(T genericObject)
        {
            if (genericObject == null)
            {
                throw new ArgumentNullException(nameof(genericObject));
            }

            else if (genericObject.GetType() == typeof(AnalyzerResult))
            {
                var analyzerResult = genericObject as AnalyzerResult;
                string serializedAnalyzerResult = $"AnalyzerID:{analyzerResult.AnalyserID}||\nVerdict:{analyzerResult.Verdict}||\nErrorMessage:{analyzerResult.ErrorMessage}\n";

                return serializedAnalyzerResult;
            }

            else if (genericObject.GetType() == typeof(Dictionary<string, List<AnalyzerResult>>))
            {
                var dictionary = genericObject as Dictionary<string, List<AnalyzerResult>>
                var serializedDictionary = new Dictionary<string, List<string>>();
                foreach (var kvp in dictionary)
                {
                    var serializedList = new List<string>();
                    foreach (var analyzerResult in kvp.Value)
                    {
                        // Recursively call Serialize for each AnalyzerResult in the list
                        serializedList.Add(Serialize(analyzerResult));
                    }

                    serializedDictionary.Add(kvp.Key, serializedList);
                }

                return JsonSerializer.Serialize(serializedDictionary);
            }

            return "Invlaid type";
        }
        private T DeserializeAnalyzerResult<T>(string serializedString)
        {
            // Implement custom deserialization logic for AnalyzerResult
            var lines = serializedString.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            string analyserID = string.Empty;
            int verdict = 0;
            string errorMessage = string.Empty;

            foreach (var line in lines)
            {
                var parts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    var propertyName = parts[0].Trim();
                    var propertyValue = parts[1].Trim();

                    switch (propertyName)
                    {
                        case "AnalyzerID":
                            analyserID = propertyValue;
                            break;
                        case "Verdict":
                            if (int.TryParse(propertyValue, out var parsedVerdict))
                            {
                                verdict = parsedVerdict;
                            }
                            break;
                        case "ErrorMessage":
                            errorMessage = propertyValue;
                            break;
                            // Handle other properties as needed
                    }
                }
            }

            AnalyzerResult analyzerResult = new AnalyzerResult(analyserID, verdict, errorMessage);

            return (T)(object)analyzerResult;
        }
        public T Deserialize<T>(string serializedString)
        {
            if (string.IsNullOrWhiteSpace(serializedString))
            {
                throw new ArgumentException("Serialized string is null or empty.", nameof(serializedString));
            }

            var deserializedDict = new Dictionary<string, List<string>>();
            var serializedDictionary = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(serializedString);
            foreach (var kvp in serializedDictionary)
            {
                var deserializedList = kvp.Value.Select(item => DeserializeAnalyzerResult<AnalyzerResult>(item)).ToList();
                deserializedDict.Add(kvp.Key, deserializedList);
            }

            return (T)(object)deserializedDict;
        }

    }
}
