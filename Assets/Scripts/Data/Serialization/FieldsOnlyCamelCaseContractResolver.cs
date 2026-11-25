using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SuperMario.Data.Serialization
{
    /// <summary>
    /// Serializes only public fields and writes camelCase names.
    /// The Core value objects expose computed properties (Magnitude, Normalized, MinX...)
    /// that must never be persisted nor re-entered by the serializer.
    /// </summary>
    internal sealed class FieldsOnlyCamelCaseContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            return properties
                .Where(property => property.UnderlyingName != null &&
                                   type.GetField(property.UnderlyingName, BindingFlags.Public | BindingFlags.Instance) != null)
                .ToList();
        }
    }
}
