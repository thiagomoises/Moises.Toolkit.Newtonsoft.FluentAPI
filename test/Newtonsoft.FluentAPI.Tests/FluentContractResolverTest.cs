using Newtonsoft.FluentAPI.Resolvers;
using Newtonsoft.FluentAPI.Tests;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Text;

namespace Newtonsoft.FluentAPI.Tests
{

    public class Tests
    {
        private FluentContractResolver _fcr;

        [SetUp]
        public void Setup()
        {
            _fcr = new FluentContractResolver();
            _fcr.AddConfiguration(new TestJsonTypeConfiguration());
        }

        [TearDown]
        public void Clean()
        {
            _fcr = null;
        }

        [Test]
        public void DeserializeObject_Test()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"first_name\": \"Thiago\",");
            stringBuilder.Append("\"last_name\": \"Moises\",");
            stringBuilder.Append("\"user_age\": 20,");
            stringBuilder.Append("\"status\": \"Deleted\",");
            stringBuilder.Append("}");


            var settings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ContractResolver = _fcr
            };

            var model = JsonConvert.DeserializeObject<UserTest>(stringBuilder.ToString(), settings);

            Assert.IsTrue(model.FirstName.Equals("Thiago")
                && model.LastName.Equals("Moises")
                && model.Age == 20
                && model.Status == UserStatus.Deleted);
        }


        [Test]
        public void SerializeObject_Test()
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = _fcr
            };

            var model = new UserTest()
            {
                FirstName = "Thiago",
                LastName = "Moises",
                Age = 20,
                Status =  UserStatus.Deleted,
                City = "São Paulo"
            };
            var str = JsonConvert.SerializeObject(model, settings);
            Assert.AreEqual(str, "{\r\n  \"first_name\": \"Thiago\",\r\n  \"last_name\": \"Moises\",\r\n  \"user_age\": 20,\r\n  \"status\": \"Deleted\",\r\n  \"is_admin\": false\r\n}");
        }
    }
}