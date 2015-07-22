// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Data.Entity.Internal;
using System.Collections.Generic;
using Microsoft.Data.Entity.Metadata;

namespace Microsoft.Data.Entity.Query.Internal
{
    public class MetadataPropertyCollectionPrinter : ConstantPrinterBase, IConstantPrinter
    {
        public override bool TryPrintConstant(object value, IndentedStringBuilder stringBuilder)
        {
            var properties = value as IEnumerable<IPropertyBase>;
            if (properties != null)
            {
                var appendAction = properties.Count() > 2 ? AppendLine : Append;

                appendAction(stringBuilder, value.GetType().DisplayName(fullName: false) + " ");
                appendAction(stringBuilder, "{ ");

                stringBuilder.IncrementIndent();
                foreach (var property in properties)
                {
                    appendAction(stringBuilder, property.DeclaringEntityType.ClrType.Name + "." + property.Name + ", ");
                }

                stringBuilder.DecrementIndent();
                stringBuilder.Append("}");

                return true;
            }

            return false;
        }
    }
}
