/******************************************************************************
* Author      = Sreelakshmi
*
* Product     = Analyzer
* 
* Project     = Content
*
* Description = Class the send and recieve data to and from analyzer
*****************************************************************************/

namespace Content
{
    /// <summary>
    /// Class that handles communication between content and analyzer
    /// Expected functionality : 
    /// Runs a thread that continuously puts uploaded files for analysis and
    /// writes them back to a file once done
    /// </summary>
    internal class AnalyserQuery
    {
        /// <summary>
        /// Handles the uploading of student files and teacher files for analysis.
        /// It initializes an analyzer, loads DLL files, and retrieves analysis results
        /// before sending them to the analyzer team.
        /// </summary>
        /// <param name="studentFilePath">A list of file paths to student files for analysis.</param>
        /// <param name="teacherFilePath">The file path to the teacher's reference file.</param>
        public void HandleUpload (string studentFilePath, string teacherFilePath)
        {
            //IAnalyzer analyzer = new Analyzer;
            //analyzer.LoadDLLFile(studentFilePath, teacherFilePath);
            //Tuple<IDictionary<string, string>, int> analysisResult = analyzer.GetAnalysis();
            //SendAnalysisResults(analysisResult);
            Console.WriteLine("Sending files to analyzer team");
        }

        /// <summary>
        /// Sends the analysis results to the content team.
        /// This function is a placeholder for the actual implementation
        /// of sending the analysis results to the content team.
        /// </summary>
        /// <param name="analysisResults">A Tuple containing analysis results, including a dictionary of string results and an integer.</param>
        static void SendAnalysisResults(Tuple<IDictionary<string, string>, int> analysisResults)
        {
            // Replace this with your code to send the analysis results to the content team
            Console.WriteLine("Sending analysis results to the content team...");
            // Your implementation here
        }

       
    }

}
