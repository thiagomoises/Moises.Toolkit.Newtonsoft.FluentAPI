using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.FluentAPI.Builders
{
    public abstract class IJsonTypeBuilder
    {
        internal abstract void ApplyConfiguration(MemberInfo member, JsonProperty contract);
    }
}