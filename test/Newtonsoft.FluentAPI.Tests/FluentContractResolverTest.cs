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
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"access_token\": \"APP_USR-701342797733779-042916-798ad80874b73483062dbf9705b9043f-431427318\",");
            stringBuilder.Append("\"refresh_token\": \"TG-5cc763b6351e0000060e8863-431427318\",");
            stringBuilder.Append("\"live_mode\": true,");
            stringBuilder.Append("\"user_id\": 431427318,");
            stringBuilder.Append("\"token_type\": \"bearer\",");
            stringBuilder.Append("\"expires_in\": 21600,");
            stringBuilder.Append("\"scope\": \"marketplace_off offline_access read reporting_full wallet-payments write\"");
            stringBuilder.Append("}");


            var settings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
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
            Assert.IsTrue(str.Equals("{\r\n  \"first_name\": \"Thiago\",\r\n  \"last_name\": \"Moises\",\r\n  \"user_age\": 20,\r\n  \"status\": \"Deleted\"\r\n}"));
        }
    }
}