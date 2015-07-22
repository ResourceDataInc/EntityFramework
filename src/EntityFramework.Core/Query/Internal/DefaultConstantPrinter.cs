// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.Internal;

namespace Microsoft.Data.Entity.Query.Internal
{
    public class DefaultConstantPrinter : ConstantPrinterBase, IConstantPrinter
    {
        public override bool TryPrintConstant(object value, IndentedStringBuilder stringBuilder)
        {
            var stringValue = "null";
            if (value != null)
            {
                stringValue = value.ToString() != value.GetType().ToString()
                    ? value.ToString()
                    : value.GetType().Name;
            }

            stringBuilder.Append("VALUE(" + stringValue + ")");

            return true;
        }
    }
}
