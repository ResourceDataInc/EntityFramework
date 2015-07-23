// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Metadata;

namespace Microsoft.Data.Entity.Internal
{
    public class ModelBaseTypeGraphAdapter : Graph<IEntityType>
    {
        private readonly IModel _model;

        public ModelBaseTypeGraphAdapter([NotNull] IModel model)
        {
            _model = model;
        }

        public override IEnumerable<IEntityType> Vertices => _model.EntityTypes;

        public override IEnumerable<IEntityType> GetOutgoingNeighbours(IEntityType from)
            => _model.EntityTypes.Where(et => et.BaseType == from);

        public override IEnumerable<IEntityType> GetIncomingNeighbours(IEntityType to)
            => to.BaseType != null ? new List<IEntityType> { to.BaseType } : Enumerable.Empty<IEntityType>();

        public virtual IReadOnlyList<IEntityType> TopologicalSort()
        {
            var sortedQueue = new List<IEntityType>();
            var unvisitedVertices = new List<IEntityType>();

            foreach (var vertex in Vertices)
            {
                var count = GetIncomingNeighbours(vertex).Count();
                if (count == 0)
                {
                    // Collect vertices without predecessors
                    sortedQueue.Add(vertex);
                }
                else
                {
                    // Track the remaining vertices
                    unvisitedVertices.Add(vertex);
                }
            }

            var index = 0;
            while (sortedQueue.Count < Vertices.Count())
            {
                while (index < sortedQueue.Count)
                {
                    var currentRoot = sortedQueue[index];

                    foreach (var successor in GetOutgoingNeighbours(currentRoot).Where(neighbour => unvisitedVertices.Contains(neighbour)))
                    {
                        // Add vertices reachable by sorted vertices.
                        sortedQueue.Add(successor);
                        unvisitedVertices.Remove(successor);
                    }

                    index++;
                }
            }

            Debug.Assert(unvisitedVertices.Count == 0);

            return sortedQueue;
        }
    }
}

