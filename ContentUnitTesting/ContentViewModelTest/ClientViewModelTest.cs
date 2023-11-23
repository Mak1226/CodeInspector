/******************************************************************************
 * Filename     = ClientViewModelTest.cs
 * 
 * Author       = Susan
 *
 * Product      = Analyzer
 * 
 * Project      = ContentUnitTesting
 *
 * Description  = Unit tests for ClientViewModel
*****************************************************************************/
using Analyzer;
using Content.ViewModel;
using ContentUnitTesting.ContentClientServerTest;
using Content.Model;

namespace ContentUnitTesting.ContentViewModelTest
{
    [TestClass]
    public class ClientViewModelTest
    {
        private string _testDirectory;

        [TestInitialize]
        public void TestInitialize()
        {
            _testDirectory = Directory.GetParent( Environment.CurrentDirectory ).Parent.Parent.FullName;
        }

        /// <summary>
        /// Test case to verify that the constructor of ContentClientViewModel initializes properties correctly.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange
            ContentClient contentClient = new( new MockCommunicator() , "sessionId" );

            // Act
            ContentClientViewModel viewModel = new( contentClient );

            // Assert
            Assert.IsNotNull( viewModel, "View model is initialized properly" );
            Assert.IsNotNull( viewModel.DataList, "Datalist is initialized properly" );
        }

        /// <summary>
        /// Test case to handle null AnalyzerResult and ensure DataList returns an empty list.
        /// </summary>
        [TestMethod]
        public void Null_AnalyzerResult()
        {
            ContentClient contentClient = new( new MockCommunicator() , "sessionId" );
            ContentClientViewModel viewModel = new( contentClient );
            contentClient.AnalyzerResultChanged.Invoke( null );

            Assert.AreEqual(viewModel.DataList.Count, 0 );

        }

        /// <summary>
        /// Test case to confirm that the DataList property returns an empty list when AnalyzerResults is null.
        /// </summary>
        [TestMethod]
        public void DataList_PropertyReturnsEmptyListWhenAnalyzerResultsIsNull()
        {
            // Arrange
            ContentClient contentClient = new( new MockCommunicator() , "sessionId" );
            ContentClientViewModel viewModel = new( contentClient );

            // Simulate analyzer result change
            var analyzerResults = new Dictionary<string , List<AnalyzerResult>>
            {
                {
                    "file1.txt", new List<AnalyzerResult>
                    {
                        new AnalyzerResult("analyzer1", 1, "Error1"),
                        new AnalyzerResult("analyzer2", 0, "NoError"),
                    }
                },
                {
                    "file2.txt", new List<AnalyzerResult>
                    {
                        new AnalyzerResult("analyzer3", 2, "Error2"),
                        new AnalyzerResult("analyzer4", 1, "Error3"),
                    }
                }
            };
                                                                          
            // Act
            contentClient.AnalyzerResultChanged?.Invoke( analyzerResults );

            List<Tuple<string , List<Tuple<string , int , string>>>> dataList = new()
            {
                new Tuple<string, List<Tuple<string, int, string>>>("file1.txt", new List<Tuple<string, int, string>>
                {
                    new Tuple<string, int, string>("analyzer1", 1, "Error1"),
                    new Tuple<string, int, string>("analyzer2", 0, "NoError"),
                }),
                new Tuple<string, List<Tuple<string, int, string>>>("file2.txt", new List<Tuple<string, int, string>>
                {
                    new Tuple<string, int, string>("analyzer3", 2, "Error2"),
                    new Tuple<string, int, string>("analyzer4", 1, "Error3"),
                })
            };

            Assert.AreNotEqual( dataList , viewModel.DataList );

        }

        /// <summary>
        /// Test case to set the SelectedItem property and verify PropertyChanged event is raised.
        /// </summary>
        [TestMethod]
        public void SetSelectedItem()
        {
            // Arrange
            ContentClient contentClient = new( new MockCommunicator() , "sessionId" );
            ContentClientViewModel viewModel = new( contentClient );
            bool eventRaised = false;

            viewModel.PropertyChanged += ( sender , e ) =>
            {
                if (e.PropertyName == nameof( viewModel.SelectedItem ))
                {
                    eventRaised = true;
                }
            };

            // Act
            viewModel.SelectedItem = new Tuple<string , List<Tuple<string , int , string>>>( "file1.txt" , new List<Tuple<string , int , string>>
            {
                new Tuple<string, int, string>("analyzer1", 1, "Error1"),
                new Tuple<string, int, string>("analyzer2", 0, "NoError"),
            } );

                    // Assert
                    Assert.IsTrue( eventRaised , "PropertyChanged event not raised for SelectedItem" );
                    Assert.AreEqual( "file1.txt" , viewModel.SelectedItem.Item1 );
                    Assert.AreEqual( 2 , viewModel.SelectedItem.Item2.Count );
                    // Add additional assertions as needed for the content of Item2

        }

        /// <summary>
        /// Test case to handle file upload and ensure the encoding is not null in the communicator.
        /// </summary>
        [TestMethod]
        public void fileUpload()
        {
            // Arrange
            string filePath = Path.Combine( _testDirectory , "TestDlls/PersistentStorage.dll" );
            MockCommunicator mockCommunicator = new();
            ContentClient contentClient = new( mockCommunicator , "sessionId" );
            ContentClientViewModel viewModel = new( contentClient );

            viewModel.HandleUpload( filePath );

            Assert.IsNotNull( mockCommunicator.GetEncoding() );
        }




    }
}
