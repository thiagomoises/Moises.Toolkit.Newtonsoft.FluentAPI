using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Newtonsoft.FluentAPI.Builders
{
    public class JsonTypeBuilder<TJsonModel> : IJsonTypeBuilder
    {
        private List<IJsonPropertyBuilder> _propertyBuilder { get; } = new List<IJsonPropertyBuilder>();
        internal IEnumerable<IJsonPropertyBuilder> PropertyBuilder { get; private set; }

        public JsonTypeBuilder()
        {
            PropertyBuilder = _propertyBuilder.AsEnumerable();
        }

        public JsonPropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TJsonModel, TProperty>> propertyExpression)
        {
            var propertyBuilder = new JsonPropertyBuilder<TProperty>((propertyExpression.Body as System.Linq.Expressions.MemberExpression).Member);
            _propertyBuilder.Add(propertyBuilder);
            return propertyBuilder;
        }

        internal override void ApplyConfiguration(MemberInfo member, JsonProperty contract)
        {
            var rule = this.PropertyBuilder.Where(r => r != null && r.PropertyInfo.Name == member.Name).FirstOrDefault();
            if (rule != null)
            {
                ApplyConfiguration(rule, contract);
            }
        }

        private void ApplyConfiguration(IJsonPropertyBuilder jsonPropertyBuilder, JsonProperty contract)
        { 
            var props = typeof(JsonProperty).GetProperties();
            foreach (var rule in jsonPropertyBuilder.RegisteredRules)
            {
                var property = props.Where(x => x.Name == rule.Key).FirstOrDefault();
                if (property != null)
                {
                    var value = rule.Value;
                    if (property.PropertyType == value.GetType() || property.PropertyType  == value.GetType().BaseType)
                    {
                        property.SetValue(contract, value);
                    }
                }
            }
        }
    }
}
