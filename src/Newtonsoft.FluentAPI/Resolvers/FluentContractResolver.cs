using Newtonsoft.FluentAPI.Abstracts;
using Newtonsoft.FluentAPI.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Newtonsoft.FluentAPI.Resolvers
{
    public class FluentContractResolver: DefaultContractResolver
    {
        private List<IJsonTypeBuilder> Builders { get; set; }

        public FluentContractResolver()
        {
            Builders = new List<IJsonTypeBuilder>();
        }

        public void AddConfiguration<TModel>(IJsonTypeConfiguration<TModel> jsonTypeConfiguration)
        {
            var jsonTypeBuilder = new JsonTypeBuilder<TModel>();
            jsonTypeConfiguration.Configure(jsonTypeBuilder);
            Builders.Add(jsonTypeBuilder);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var contract = base.CreateProperty(member, memberSerialization);
            Builders.Where(x => x.GetType().GenericTypeArguments[0] == member.DeclaringType).ToList().ForEach(x=> x.ApplyConfiguration(member, contract));
            return contract;
        }
    }
}
