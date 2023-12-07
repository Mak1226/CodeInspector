/******************************************************************************
* Filename    = InsightsUtility.cs
*
* Author      = Sahil
*
* Product     = Analyzer
* 
* Project     = Cloud
*
* Description = Contains serialization and deserialization for some data structures
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ServerlessFunc
{
    public class InsightsUtility
    {
        /// <summary>
        /// Converts a byte array into a list of strings.
        /// </summary>
        /// <param name="byteArray">The byte array to convert.</param>
        /// <returns>A list of strings.</returns>
        public static List<string> ByteToList( byte[] byteArray )
        {
            string concatenatedString = Encoding.UTF8.GetString( byteArray );

            // Split the concatenated string back into individual strings
            string[] stringArray = concatenatedString.Split( new string[] { Environment.NewLine } , StringSplitOptions.None );

            // Convert the string array to a List<string>
            List<string> stringList = new( stringArray );

            return stringList;
        }

        /// <summary>
        /// Converts a list of strings into a byte array.
        /// </summary>
        /// <param name="list">The list of strings to convert.</param>
        /// <returns>A byte array.</returns>
        public static byte[] ListToByte( List<string> list )
        {
            string concatenatedString = string.Join( Environment.NewLine , list );

            byte[] byteArray = Encoding.UTF8.GetBytes( concatenatedString );

            return byteArray;
        }

        /// <summary>
        /// Converts a list of tuples containing strings and strings into a byte array.
        /// </summary>
        /// <param name="list">The list of tuples to convert.</param>
        /// <returns>A byte array.</returns>
        public static byte[] ListTupleToByte( List<Tuple<string , string>> list )
        {
            string jsonString = JsonSerializer.Serialize( list );
            return Encoding.UTF8.GetBytes( jsonString );
        }

        /// <summary>
        /// Converts an analysis file (a byte array) into a dictionary.
        /// </summary>
        /// <param name="analysisFile">The analysis file to convert.</param>
        /// <returns>A dictionary.</returns>
        public static Dictionary<string , List<AnalyzerResult>> ConvertAnalysisFileToDictionary( byte[] analysisFile )
        {
            string jsonString = Encoding.UTF8.GetString( analysisFile );
            Dictionary<string , List<AnalyzerResult>> dictionary = JsonSerializer.Deserialize<Dictionary<string , List<AnalyzerResult>>>( jsonString );
            return dictionary;
        }

        /// <summary>
        /// Converts a dictionary into an analysis file (a byte array).
        /// </summary>
        /// <param name="data">The dictionary to convert.</param>
        /// <returns>A byte array.</returns>
        public static byte[] ConvertDictionaryToAnalysisFile( Dictionary<string , List<AnalyzerResult>> data )
        {
            string jsonString = JsonSerializer.Serialize( data );
            return Encoding.UTF8.GetBytes( jsonString );
        }



        public static byte[] ConvertDictionaryToAnalysisFile1( Dictionary<string , List<Analyzer.AnalyzerResult>> data )
        {
            string jsonString = JsonSerializer.Serialize( data );
            return Encoding.UTF8.GetBytes( jsonString );
        }
    }
}
