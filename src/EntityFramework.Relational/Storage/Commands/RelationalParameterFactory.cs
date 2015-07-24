// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Data.Entity.Storage.Commands
{
    public class RelationalParameterFactory
    {
        private readonly IRelationalTypeMapper _typeMapper;
        private int parameterIndex;

        public RelationalParameterFactory(IRelationalTypeMapper typeMapper)
        {
            _typeMapper = typeMapper;
        }

        public IReadOnlyList<RelationalParameter> Create(params object[] parameters)
        {
            var relationalParameters = new List<RelationalParameter>();

            foreach (var parameter in parameters)
            {
                relationalParameters.Add(
                    new RelationalParameter(
                        _typeMapper.GetDefaultMapping(parameter),
                        ParameterPrefix + "p" + parameterIndex++,
                        parameter,
                        parameter.GetType().IsNullableType()));
            }

            return relationalParameters;
        }

        protected virtual string ParameterPrefix => "@";
    }
}
