// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.Data.Entity.Internal;
using Microsoft.Data.Entity.Query.Sql;
using Microsoft.Data.Entity.Query.Expressions;

namespace Microsoft.Data.Entity.Query.Internal
{
    public class CommandBuilderPrinter : ConstantPrinterBase, IConstantPrinter
    {
        public override bool TryPrintConstant(object value, IndentedStringBuilder stringBuilder)
        {
            var commandBuilder = value as CommandBuilder;
            if (commandBuilder != null)
            {
                stringBuilder.AppendLine("SelectExpression: ");
                stringBuilder.IncrementIndent();

                var sqlGeneratorFactoryInfo = commandBuilder.GetType().GetField("_sqlGeneratorFactory", BindingFlags.NonPublic | BindingFlags.Instance);
                var sqlGeneratorFactory = (Func<ISqlQueryGenerator>)sqlGeneratorFactoryInfo.GetValue(commandBuilder);
                var sqlGenerator = sqlGeneratorFactory.Invoke();

                SelectExpression selectExpression = null;
                if (sqlGenerator is DefaultQuerySqlGenerator)
                {
                    var selectExpressionInfo = typeof(DefaultQuerySqlGenerator).GetField("_selectExpression", BindingFlags.NonPublic | BindingFlags.Instance);
                    selectExpression = (SelectExpression)selectExpressionInfo.GetValue(sqlGenerator);
                }
                else if (sqlGenerator is RawSqlQueryGenerator)
                {
                    var selectExpressionInfo = typeof(RawSqlQueryGenerator).GetField("_selectExpression", BindingFlags.NonPublic | BindingFlags.Instance);
                    selectExpression = (SelectExpression)selectExpressionInfo.GetValue(sqlGenerator);
                }

                var sql = selectExpression.ToString();
                var lines = sql.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    stringBuilder.AppendLine(line);
                }

                stringBuilder.DecrementIndent();

                return true;
            }

            return false;
        }
    }
}
