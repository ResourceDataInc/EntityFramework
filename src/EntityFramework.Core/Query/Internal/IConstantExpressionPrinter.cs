// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.Internal;

namespace Microsoft.Data.Entity.Query.Internal
{
    public interface IConstantPrinter
    {
        bool TryPrintConstant(object value, IndentedStringBuilder stringBuilder);
    }
}
