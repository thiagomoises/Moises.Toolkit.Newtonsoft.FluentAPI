using Newtonsoft.FluentAPI.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Newtonsoft.FluentAPI.Abstracts
{
    public interface IJsonTypeConfiguration<TModel>
    {
        void Configure(JsonTypeBuilder<TModel> jsonTypeBuilder);
    }    
}
