using Content.Model;

namespace ContentUnitTesting.ContentClientServerTest
{
    [TestClass]
    public class ContentServerTest
    {
        MockCommunicator _communicator;
        MockAnalyzer _analyzer;
        [TestInitialize]
        public void TestInitializer()
        {
            _communicator = new MockCommunicator();
            _analyzer = new MockAnalyzer(); 
        }
        [TestMethod]
        public void Method1()
        {
            ContentServer contentServer = new ContentServer(_communicator, _analyzer);
            contentServer.HandleRecieve("","testSessionID");
        }
    }
}
