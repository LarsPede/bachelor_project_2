using BachelorModelViewController.Data;
using BachelorModelViewController.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    class ApproveAdminUser
    {
        private MongoOperations _mongoOperations;

        [TestInitialize]
        public void Initalize()
        {
            // connection to live db.
            IOptions<MongoSettings> settings = Options.Create<MongoSettings>(new MongoSettings { ConnectionString = "mongodb://ec2-18-221-88-228.us-east-2.compute.amazonaws.com:27017", Database = "cupid" });
            _mongoOperations = new MongoOperations(settings);
        }

        [DataTestMethod]
        [DataRow("Unit Test Collection New")]
        public void ApproveAdminUserMethod(string value)
        {
            Assert.IsTrue(true);
        }
    }
}
