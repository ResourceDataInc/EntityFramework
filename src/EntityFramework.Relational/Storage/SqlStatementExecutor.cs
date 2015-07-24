// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Internal;
using Microsoft.Data.Entity.Storage.Commands;
using Microsoft.Data.Entity.Utilities;
using Microsoft.Framework.Logging;

namespace Microsoft.Data.Entity.Storage
{
    public class SqlStatementExecutor : ISqlStatementExecutor
    {
        private readonly LazyRef<ILogger> _logger;

        public SqlStatementExecutor([NotNull] ILoggerFactory loggerFactory)
        {
            Check.NotNull(loggerFactory, nameof(loggerFactory));

            _logger = new LazyRef<ILogger>(loggerFactory.CreateLogger<SqlStatementExecutor>);
        }

        protected virtual ILogger Logger => _logger.Value;

        public virtual void ExecuteNonQuery(
            [NotNull] IRelationalConnection connection,
            [NotNull] RelationalCommand relationalCommand)
            => ExecuteNonQuery(connection, new[] { relationalCommand });

        public virtual void ExecuteNonQuery(
            [NotNull] IRelationalConnection connection,
            [NotNull] IEnumerable<RelationalCommand> relationalCommands)
        {
            Check.NotNull(connection, nameof(connection));
            Check.NotNull(relationalCommands, nameof(relationalCommands));

            Execute(
                connection,
                () =>
                {
                    foreach (var relationalCommand in relationalCommands)
                    {
                        var command = relationalCommand.CreateDbCommand(connection);
                        Logger.LogCommand(command);

                        command.ExecuteNonQuery();
                    }
                    return null;
                });
        }

        public virtual Task ExecuteNonQueryAsync(
            [NotNull] IRelationalConnection connection,
            [NotNull] RelationalCommand relationalCommand,
            CancellationToken cancellationToken = default(CancellationToken))
            => ExecuteNonQueryAsync(connection, new[] { relationalCommand }, cancellationToken);

        public virtual Task ExecuteNonQueryAsync(
            [NotNull] IRelationalConnection connection,
            [NotNull] IEnumerable<RelationalCommand> relationalCommands,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(connection, nameof(connection));
            Check.NotNull(relationalCommands, nameof(relationalCommands));

            return ExecuteAsync(
                connection,
                async () =>
                {
                    foreach (var relationalCommand in relationalCommands)
                    {
                        var command = relationalCommand.CreateDbCommand(connection);
                        Logger.LogCommand(command);

                        await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                    return Task.FromResult<object>(null);
                },
                cancellationToken);
        }




        public virtual Task<object> ExecuteScalarAsync(
            IRelationalConnection connection,
            DbTransaction transaction,
            string sql,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(connection, nameof(connection));
            Check.NotNull(sql, nameof(sql));

            return ExecuteAsync(
                connection,
                () =>
                    {
                        var command = new SqlBatch(sql).CreateCommand(connection, transaction);
                        Logger.LogCommand(command);

                        return command.ExecuteScalarAsync(cancellationToken);
                    },
                cancellationToken);
        }

        protected virtual async Task<object> ExecuteAsync(
            IRelationalConnection connection,
            Func<Task<object>> action,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(connection, nameof(connection));

            await connection.OpenAsync(cancellationToken);

            try
            {
                return await action();
            }
            finally
            {
                connection.Close();
            }
        }





        public virtual object ExecuteScalar(
            IRelationalConnection connection,
            DbTransaction transaction,
            string sql)
        {
            Check.NotNull(connection, nameof(connection));
            Check.NotNull(sql, nameof(sql));

            return Execute(
                connection,
                () =>
                    {
                        var command = new SqlBatch(sql).CreateCommand(connection, transaction);
                        Logger.LogCommand(command);

                        return command.ExecuteScalar();
                    });
        }

        protected virtual object Execute(
            IRelationalConnection connection,
            Func<object> action)
        {
            Check.NotNull(connection, nameof(connection));

            // TODO Deal with suppressing transactions etc.
            connection.Open();

            try
            {
                return action();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
