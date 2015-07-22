// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.Internal;

namespace Microsoft.Data.Entity.Query.Internal
{
    public class CollectionConstantPrinter : ConstantPrinterBase, IConstantPrinter
    {
        public override bool TryPrintConstant(object value, IndentedStringBuilder stringBuilder)
        {
            var enumerable = value as System.Collections.IEnumerable;
            if (enumerable != null && !(value is string))
            {
                var appendAction = value is byte[] ? Append : AppendLine;

                appendAction(stringBuilder, value.GetType().DisplayName(fullName: false) + " ");
                appendAction(stringBuilder, "{ ");
                stringBuilder.IncrementIndent();
                foreach (var item in enumerable)
                {
                    appendAction(stringBuilder, item.ToString() + ", ");
                }

                stringBuilder.DecrementIndent();
                appendAction(stringBuilder, "}");

                return true;
            }

            return false;
        }
    }
}
