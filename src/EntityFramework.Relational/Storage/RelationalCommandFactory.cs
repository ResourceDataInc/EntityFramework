// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Storage
{
    public class RelationalCommandFactory
    {
        private readonly IRelationalConnection _connection;

        public RelationalCommandFactory(IRelationalConnection connection)
        {
            _connection = connection;
        }
    }
}
