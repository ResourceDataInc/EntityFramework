// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.Internal;

namespace Microsoft.Data.Entity.Query.Internal
{
    public abstract class ConstantPrinterBase : IConstantPrinter
    {
        public Action<IndentedStringBuilder, string> AppendLine => (sb, s) => sb.AppendLine(s);
        public Action<IndentedStringBuilder, string> Append => (sb, s) => sb.Append(s);

        public abstract bool TryPrintConstant(object value, IndentedStringBuilder stringBuilder);
    }
}
