// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Data.Entity.Internal;

namespace Microsoft.Data.Entity.Query.Internal
{
    public class EntityTrackingInfoListPrinter : ConstantPrinterBase, IConstantPrinter
    {
        public override bool TryPrintConstant(object value, IndentedStringBuilder stringBuilder)
        {
            var trackingInfoList = value as List<EntityTrackingInfo>;
            if (trackingInfoList != null)
            {
                var appendAction = trackingInfoList.Count() > 2 ? AppendLine : Append;

                appendAction(stringBuilder, "{ ");
                stringBuilder.IncrementIndent();

                for (int i = 0; i < trackingInfoList.Count; i++)
                {
                    var entityTrackingInfo = trackingInfoList[i];
                    var separator = i == trackingInfoList.Count -1 ? " " : ", ";
                    stringBuilder.Append("itemType: " + entityTrackingInfo.QuerySource.ItemType.Name);
                    appendAction(stringBuilder, separator);
                }

                stringBuilder.DecrementIndent();
                appendAction(stringBuilder, "}");

                return true;
            }

            return false;
        }
    }
}
