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
*****************************************************************************/
using Analyzer;
using Networking.Serialization;
using System.Text.Json;
using Logging;

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
            Logger.Debug( "[AnalyzerResultSerializer.cs] : Seriailize" );
            if (genericObject == null)
            {
                throw new ArgumentNullException(nameof(genericObject));
            }

            else if (genericObject.GetType() == typeof(AnalyzerResult))
            {
                var analyzerResult = genericObject as AnalyzerResult;
                string serializedAnalyzerResult = $"AnalyzerID&:{analyzerResult.AnalyserID}||\nVerdict&:{analyzerResult.Verdict}||\nErrorMessage&:{analyzerResult.ErrorMessage}\n";

                return serializedAnalyzerResult;
            }

            else if (genericObject.GetType() == typeof(Dictionary<string, List<AnalyzerResult>>))
            {
                var dictionary = genericObject as Dictionary<string, List<AnalyzerResult>>;
                var serializedDictionary = new Dictionary<string, List<string>>();
                foreach (KeyValuePair<string , List<AnalyzerResult>> kvp in dictionary)
                {
                    var serializedList = new List<string>();
                    foreach (AnalyzerResult analyzerResult in kvp.Value)
                    {
                        // Recursively call Serialize for each AnalyzerResult in the list
                        serializedList.Add(Serialize(analyzerResult));
                    }

                    serializedDictionary.Add(kvp.Key, serializedList);
                }

                return JsonSerializer.Serialize(serializedDictionary);
            }

            return "Invalid type";
        }
        private T DeserializeAnalyzerResult<T>(string serializedString)
        {
            Logger.Debug( "[AnalyzerResultSerializer.cs] : DeserializeAnalyzerResult" );
            // Implement custom deserialization logic for AnalyzerResult
            string[] lines = serializedString.Split("||\n", StringSplitOptions.RemoveEmptyEntries);

            string analyserID = string.Empty;
            int verdict = 0;
            string errorMessage = string.Empty;

            foreach (string line in lines)
            {
                string[] parts = line.Split( "&:" , StringSplitOptions.RemoveEmptyEntries );
                if (parts.Length == 2)
                {
                    string propertyName = parts[0].Trim();
                    string propertyValue = parts[1].Trim();

                    switch (propertyName)
                    {
                        case "AnalyzerID":
                            analyserID = propertyValue;
                            break;
                        case "Verdict":
                            if (int.TryParse(propertyValue, out int parsedVerdict ))
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

            AnalyzerResult analyzerResult = new(analyserID, verdict, errorMessage);

            return (T)(object)analyzerResult;
        }
        public T Deserialize<T>(string serializedString)
        {
            Logger.Inform( "[AnalyzerResultSerializer.cs] : Deserialize" );
            if (string.IsNullOrWhiteSpace(serializedString))
            {
                throw new ArgumentException("Serialized string is null or empty.", nameof(serializedString));
            }

            Dictionary<string , List<AnalyzerResult>> deserializedDict = new();
            Dictionary<string , List<string>>? serializedDictionary = JsonSerializer.Deserialize<Dictionary<string , List<string>>>( serializedString );
            foreach (KeyValuePair<string , List<string>> kvp in serializedDictionary)
            {
                var deserializedList = kvp.Value.Select(item => DeserializeAnalyzerResult<AnalyzerResult>(item)).ToList();
                deserializedDict.Add(kvp.Key, deserializedList);
            }

            return (T)(object)deserializedDict;
        }

    }
}
