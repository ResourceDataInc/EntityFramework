// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Data.Common;

namespace Microsoft.Data.Entity.Storage.Commands
{
    public class RelationalParameter
    {
        private readonly RelationalTypeMapping _typeMapping;
        private readonly object _value;
        private readonly bool? _isNullable;


        public RelationalParameter(
            RelationalTypeMapping typeMapping,
            string name,
            object value,
            bool? isNullable = null)
        {
            Name = name;
            _value = value;
            _typeMapping = typeMapping;
            _isNullable = isNullable;
        }

        public virtual string Name { get; }

        public virtual void AddDbParameter(DbCommand command)
            => _typeMapping.CreateParameter(command, Name, _value, _isNullable);
    }
}
