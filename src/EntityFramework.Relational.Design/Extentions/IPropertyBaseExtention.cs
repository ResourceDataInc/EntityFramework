using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Metadata;

namespace Microsoft.Data.Entity.Relational.Design.Extentions
{
    public static class IPropertyBaseExtention
    {
        public static string PropertyName(this IProperty propertyBase)
        {
            var name = propertyBase.Name.Replace("_", "");

            var className = propertyBase.DeclaringEntityType.Name.Replace("_", "");

            var regex = new Regex($"^{className}");

            var shortName = regex.Replace(name, "");

            return Regex.IsMatch(shortName, @"^\d+$") ? name : shortName;
        }
    }

}
