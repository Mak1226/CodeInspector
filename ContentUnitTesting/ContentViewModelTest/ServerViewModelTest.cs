/******************************************************************************
 * Filename     = ServerViewModelTest.cs
 * 
 * Author       = Susan
 *
 * Product      = Analyzer
 * 
 * Project      = ContentUnitTesting
 *
 * Description  = Unit tests for ServerViewModel
*****************************************************************************/
using Content.Model;
using Content.ViewModel;
using ContentUnitTesting.ContentClientServerTest;
using Analyzer;
using System.Diagnostics;

namespace ContentUnitTesting.ContentViewModelTest
{
    [TestClass]
    public class ServerViewModelTest
    {
        /// <summary>
        /// Test case to assert that the ContentServerViewModel constructor initializes correctly.
        /// </summary>
        [TestMethod]
        public void ContentServerViewModel_Constructor_Initialization()
        {
            // Arrange
            MockAnalyzer mockAnalyzer = new();
            MockCommunicator mockCommunicator = new();
            ContentServer contentServerMock = new( mockCommunicator, mockAnalyzer, "sessionID");

            // Act
            ContentServerViewModel viewModel = new( contentServerMock );

            // Assert
            Assert.IsNotNull( viewModel );
            // Add more assertions based on your constructor logic
        }

        /// <summary>
        /// Test case to ensure that the DataList property returns the correct data when AnalyzerResultChanged event is invoked.
        /// </summary>
        [TestMethod]
        public void ContentServerViewModel_DataList_ReturnsCorrectData()
        {
            // Arrange
            MockAnalyzer mockAnalyzer = new();
            MockCommunicator mockCommunicator = new();
            ContentServer mockContentServer = new( mockCommunicator , mockAnalyzer , "sessionID" );
            ContentServerViewModel viewModel = new( mockContentServer );

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

            mockContentServer.AnalyzerResultChanged.Invoke( analyzerResults );

            // Act
            List<Tuple<string , List<Tuple<string , int , string>>>> dataList = viewModel.DataList;

            // Assert
            Assert.IsNotNull( dataList );
            Assert.AreEqual( 2 , dataList.Count );

            // Assert data for file1.txt
            Tuple<string , List<Tuple<string , int , string>>>? file1Data = dataList.Find( data => data.Item1 == "file1.txt" );
            Assert.IsNotNull( file1Data );
            Assert.AreEqual( 2 , file1Data.Item2.Count );

            // Assert data for file2.txt
            Tuple<string , List<Tuple<string , int , string>>>? file2Data = dataList.Find( data => data.Item1 == "file2.txt" );
            Assert.IsNotNull( file2Data );
            Assert.AreEqual( 2 , file2Data.Item2.Count );
        }

        /// <summary>
        /// Test case to verify that the DataList getter returns an empty list when AnalyzerResults is null.
        /// </summary>
        [TestMethod]
        public void DataList_Getter_ReturnsEmptyListWhenAnalyzerResultsIsNull()
        {
            // Arrange
            MockAnalyzer mockAnalyzer = new();
            MockCommunicator mockCommunicator = new();
            ContentServer mockContentServer = new( mockCommunicator , mockAnalyzer , "sessionID" );
            ContentServerViewModel viewModel = new( mockContentServer );
            mockContentServer.AnalyzerResultChanged.Invoke( null );

            // Act
            List<Tuple<string , List<Tuple<string , int , string>>>> dataList = viewModel.DataList;

            // Assert
            Assert.IsNotNull( dataList );
            Assert.AreEqual( 0 , dataList.Count );
        }

        /// <summary>
        /// Test case to ensure that the PropertyChanged event is raised when setting the SelectedItem property.
        /// </summary>
        [TestMethod]
        public void SelectedItem_SetValue_PropertyChangedEventRaised()
        {
            // Arrange
            MockAnalyzer mockAnalyzer = new();
            MockCommunicator mockCommunicator = new();
            ContentServer mockContentServer = new( mockCommunicator , mockAnalyzer , "sessionID" );
            ContentServerViewModel viewModel = new( mockContentServer );
            
            bool eventRaised = false;
            viewModel.PropertyChanged += ( sender , args ) =>
            {
                if (args.PropertyName == "SelectedItem")
                {
                    eventRaised = true;
                }
            };

            // Act
            viewModel.SelectedItem = new Tuple<string , List<Tuple<string , int , string>>>( "TestItem" , new List<Tuple<string , int , string>>() );

            // Assert
            Assert.IsTrue( eventRaised , "PropertyChanged event should be raised for SelectedItem." );
        }

        /// <summary>
        /// Test case to verify that the SelectedItem getter returns the default value when not set.
        /// </summary>
        [TestMethod]
        public void SelectedItem_Getter_ReturnsDefaultValueWhenNotSet()
        {
            // Arrange
            MockAnalyzer mockAnalyzer = new();
            MockCommunicator mockCommunicator = new();
            ContentServer mockContentServer = new( mockCommunicator , mockAnalyzer , "sessionID" );
            ContentServerViewModel viewModel = new( mockContentServer );

            // Act
            Tuple<string , List<Tuple<string , int , string>>> selectedItem = viewModel.SelectedItem;

            // Assert
            Assert.IsNull( selectedItem );
        }

        /// <summary>
        /// Test case to check if the PropertyChanged event is raised when setting the ConfigOptionsList property.
        /// </summary>
        [TestMethod]
        public void ConfigOptionsList_SetValue_PropertyChangedEventRaised()
        { 
            // Arrange
            MockAnalyzer mockAnalyzer = new();
            MockCommunicator mockCommunicator = new();
            ContentServer mockContentServer = new( mockCommunicator , mockAnalyzer , "sessionID" );
            ContentServerViewModel viewModel = new( mockContentServer );

            bool eventRaised = false;
            viewModel.PropertyChanged += ( sender , args ) =>
            {
                if (args.PropertyName == "ConfigOptionsList")
                {
                    eventRaised = true;
                }
            };

            // Act
            viewModel.ConfigOptionsList = new List<AnalyzerConfigOption> { new AnalyzerConfigOption { AnalyzerId = 1 , Description = "Option1" , IsSelected = true } };

            // Assert
            Assert.IsTrue( eventRaised , "PropertyChanged event should be raised for ConfigOptionsList." );
        }

        /// <summary>
        /// Test case to ensure that ConfigureAnalyzer calls ContentServer.Configure with the correct parameters.
        /// </summary>
        [TestMethod]
        public void ConfigureAnalyzer_CallsContentServerConfigure()
        {
            // Arrange
            MockAnalyzer mockAnalyzer = new();
            MockCommunicator mockCommunicator = new();
            ContentServer mockContentServer = new( mockCommunicator , mockAnalyzer , "" );
            ContentServerViewModel viewModel = new( mockContentServer );
            viewModel.SetSessionID( "sessionID" );

            // Act
            var inputTeacherOptions = new Dictionary<int , bool> { { 1 , true } , { 2 , false } };
            viewModel.ConfigureAnalyzer( inputTeacherOptions );

            IDictionary<int , bool> teacherOptions = mockAnalyzer.GetTeacherOptions();

            // Assert
            Assert.AreEqual(inputTeacherOptions, teacherOptions );
        }

        /// <summary>
        /// Test case to validate that the ConfigOptionsList getter returns the expected value.
        /// </summary>
        [TestMethod]
        public void ConfigOptionsList_Getter_ReturnsExpectedValue()
        {
            // Arrange
            var expectedValue = new List<AnalyzerConfigOption> { new AnalyzerConfigOption { AnalyzerId = 1 , Description = "Option 1" , IsSelected = true } };
            MockAnalyzer mockAnalyzer = new();
            MockCommunicator mockCommunicator = new();
            ContentServer mockContentServer = new( mockCommunicator , mockAnalyzer , "sessionID" );
            ContentServerViewModel viewModel = new( mockContentServer )
            {
                ConfigOptionsList = expectedValue
            };

            // Act
            List<AnalyzerConfigOption> actualValue = viewModel.ConfigOptionsList;

            // Assert
            CollectionAssert.AreEqual( expectedValue , actualValue );
        }
    }
}
