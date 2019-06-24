using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.FluentAPI.Builders
{
    public interface IJsonPropertyBuilder
    {
        IEnumerable<KeyValuePair<string, object>> RegisteredRules { get; }
        MemberInfo PropertyInfo { get;}
    }
    public interface IJsonPropertyBuilder<TJsonPropertyBuilder> : IJsonPropertyBuilder
    {
        TJsonPropertyBuilder HasFieldName(string name);
        TJsonPropertyBuilder HasConverter(JsonConverter jsonConverter);
        TJsonPropertyBuilder IsIgnored(bool ignore = true);
        TJsonPropertyBuilder AddDefaultValueHandling(DefaultValueHandling defaultValueHandling);
        TJsonPropertyBuilder AddNullValueHandling(NullValueHandling nullValueHandling);
    }

    public class JsonPropertyBuilder<TProperty> : IJsonPropertyBuilder<JsonPropertyBuilder<TProperty>>
    {
        public const string CONVERTER_KEY = "Converter";
        public const string PROPERTY_NAME_KEY = "PropertyName";
        public const string DEFAULT_VALUE_HANDING_KEY = "DefaultValueHandling";
        public const string NULL_VALUE_HANDING_KEY = "NullValueHandling";
        public const string IGNORED_KEY = "Ignored";

        private Dictionary<string, object> _rule { get; } = new Dictionary<string, object>();
        public MemberInfo PropertyInfo { get; protected set; }

        public IEnumerable<KeyValuePair<string, object>> RegisteredRules
        {
            get
            {
                return _rule.AsEnumerable();
            }
        }

        public JsonPropertyBuilder(MemberInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        public JsonPropertyBuilder<TProperty> HasFieldName(string name)
        {
            AddRule(PROPERTY_NAME_KEY, name);
            return this;
        }

        public JsonPropertyBuilder<TProperty> AddDefaultValueHandling(DefaultValueHandling defaultValueHandling)
        {
            AddRule(DEFAULT_VALUE_HANDING_KEY, defaultValueHandling);
            return this;
        }

        public JsonPropertyBuilder<TProperty> AddNullValueHandling(NullValueHandling  nullValueHandling)
        {
            AddRule(NULL_VALUE_HANDING_KEY, nullValueHandling);
            return this;
        }
        
        public JsonPropertyBuilder<TProperty> IsIgnored(bool ignore = true)
        {
            AddRule(IGNORED_KEY, ignore);
            return this;
        }

        public JsonPropertyBuilder<TProperty> HasConverter(JsonConverter jsonConverter)
        {
            AddRule(CONVERTER_KEY, jsonConverter);
            return this;
        }

        protected void AddRule(string key, object value)
        {
            if (!_rule.ContainsKey(key))
            {
                _rule.Add(key, value);
            }
            else
            {
                _rule[key] = value;
            }
        }
    }
}
