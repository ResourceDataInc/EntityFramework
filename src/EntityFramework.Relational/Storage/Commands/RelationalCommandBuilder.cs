// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Internal;
using Microsoft.Data.Entity.Utilities;

namespace Microsoft.Data.Entity.Storage.Commands
{
    public class RelationalCommandBuilder
    {
        private readonly IRelationalTypeMapper _typeMapper;
        private readonly IndentedStringBuilder _stringBuilder = new IndentedStringBuilder();
        private readonly RelationalParameterFactory _parameterFactory;
        private readonly List<RelationalParameter> _parameters = new List<RelationalParameter>();

        public RelationalCommandBuilder([NotNull] IRelationalTypeMapper typeMapper)
        {
            Check.NotNull(typeMapper, nameof(typeMapper));

            _typeMapper = typeMapper;
            _parameterFactory = new RelationalParameterFactory(typeMapper);
        }

        public virtual RelationalCommandBuilder AppendLine()
            => AppendLine(string.Empty);

        public virtual RelationalCommandBuilder Append([NotNull]object o)
        {
            Check.NotNull(o, nameof(o));

            _stringBuilder.AppendLine(o);

            return this;
        }

        public virtual RelationalCommandBuilder AppendLine([NotNull]object o)
        {
            Check.NotNull(o, nameof(o));

            _stringBuilder.AppendLine(o);

            return this;
        }

        public virtual RelationalCommandBuilder Append([NotNull] string sqlFragment, [NotNull] params object[] parameters)
        {
            Check.NotNull(sqlFragment, nameof(sqlFragment));
            Check.NotNull(parameters, nameof(parameters));

            _stringBuilder.AppendLine(MapParameters(sqlFragment, parameters));

            return this;
        }

        public virtual RelationalCommandBuilder AppendLine([NotNull] string sqlFragment, [NotNull] params object[] parameters)
        {
            Check.NotNull(sqlFragment, nameof(sqlFragment));
            Check.NotNull(parameters, nameof(parameters));

            _stringBuilder.AppendLine(MapParameters(sqlFragment, parameters));

            return this;
        }

        public virtual RelationalCommand RelationalCommand
            => new RelationalCommand(_stringBuilder.ToString(), _parameters.ToArray());

        public virtual IDisposable Indent()
            => _stringBuilder.Indent();

        private string MapParameters(string sqlFragment, params object[] parameters)
        {
            var relationalParameters = _parameterFactory.Create(parameters);

            _parameters.AddRange(relationalParameters);

            return string.Format(sqlFragment, relationalParameters.Select(p => p.Name));
        }
    }
}
