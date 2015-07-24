// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.Data.Entity.Storage;
using Microsoft.Data.Entity.Storage.Commands;

namespace Microsoft.Data.Entity.Infrastructure
{
    public class RelationalSqlExecutor
    {
        private ISqlStatementExecutor _statementExecutor;
        private IRelationalConnection _connection;
        private IRelationalTypeMapper _typeMapper;

        public RelationalSqlExecutor(
            [NotNull] ISqlStatementExecutor statementExecutor,
            [NotNull] IRelationalConnection connection,
            [NotNull] IRelationalTypeMapper typeMapper)
        {
            _statementExecutor = statementExecutor;
            _connection = connection;
            _typeMapper = typeMapper;
        }

        public virtual void ExecuteSqlCommand([NotNull] string sql, [NotNull] params object[] parameters)
            => _statementExecutor.ExecuteNonQuery(
                _connection,
                new RelationalCommandBuilder(_typeMapper).Append(sql, parameters).RelationalCommand);
    }
}
