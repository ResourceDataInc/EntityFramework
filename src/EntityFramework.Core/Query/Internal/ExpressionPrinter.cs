// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Data.Entity.Internal;

namespace Microsoft.Data.Entity.Query.Internal
{
    public class ExpressionPrinter
    {
        public string Print(Expression expression, List<IConstantPrinter> constantPrinters)
        {
            var stringBuilder = new IndentedStringBuilder();
            var visitor = new ExpressionPrintingVisitor(stringBuilder, constantPrinters);
            visitor.Visit(expression);

            var result = "TRACKED: " + visitor.TrackedQuery + Environment.NewLine;
            result += stringBuilder.ToString();

            return result;
        }
    }
}
