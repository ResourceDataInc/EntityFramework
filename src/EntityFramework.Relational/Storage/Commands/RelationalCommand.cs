// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Data.Common;

namespace Microsoft.Data.Entity.Storage.Commands
{
    public class RelationalCommand
    {
        public RelationalCommand(
            string commandText,
            params RelationalParameter[] parameters)
        {
            CommandText = commandText;
            Parameters = parameters;
        }

        public string CommandText { get; }

        public IReadOnlyList<RelationalParameter> Parameters { get; }

        public DbCommand CreateDbCommand(IRelationalConnection connection)
        {
            var command = connection.DbConnection.CreateCommand();
            command.CommandText = CommandText;

            if (connection.Transaction != null)
            {
                command.Transaction = connection.Transaction.DbTransaction;
            }

            if (connection.CommandTimeout != null)
            {
                command.CommandTimeout = (int)connection.CommandTimeout;
            }

            foreach(var parameter in Parameters)
            {
                parameter.AddDbParameter(command);
            }

            return command;
        }
    }
}
