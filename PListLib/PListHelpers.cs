/* =============================================================================
 * File:   PListHelpers.cs
 * Author: Cole Tobin
 * =============================================================================
 * Copyright (c) 2022 Cole Tobin
 *
 * This file is part of iDecryptIt.
 *
 * iDecryptIt is free software: you can redistribute it and/or modify it under
 *   the terms of the GNU General Public License as published by the Free
 *   Software Foundation, either version 3 of the License, or (at your option)
 *   any later version.
 *
 * iDecryptIt is distributed in the hope that it will be useful, but WITHOUT
 *   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 *   FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
 *   more details.
 *
 * You should have received a copy of the GNU General Public License along with
 *   iDecryptIt. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */

using System.Diagnostics;
using System.Xml;

namespace PListLib;

internal static class PListHelpers
{
    internal const string XML_NAME_ARRAY = "array";
    internal const string XML_NAME_BOOL_TRUE = "true";
    internal const string XML_NAME_BOOL_FALSE = "false";
    internal const string XML_NAME_DATA = "data";
    internal const string XML_NAME_DATE = "date";
    internal const string XML_NAME_DICTIONARY = "dict";
    internal const string XML_NAME_INTEGER = "integer";
    internal const string XML_NAME_REAL = "real";
    internal const string XML_NAME_STRING = "string";

    internal static IPListElement ParseNode(XmlNode node)
    {
        Debug.Assert(node.NodeType is XmlNodeType.Element);

        return node.Name.ToLowerInvariant() switch
        {
            XML_NAME_ARRAY => PListArray.ReadXml(node),
            XML_NAME_BOOL_TRUE or XML_NAME_BOOL_FALSE => PListBoolean.ReadXml(node),
            XML_NAME_DATA => PListData.ReadXml(node),
            XML_NAME_DATE => PListDate.ReadXml(node),
            XML_NAME_DICTIONARY => PListDictionary.ReadXml(node),
            XML_NAME_INTEGER => PListInteger.ReadXml(node),
            XML_NAME_REAL => PListReal.ReadXml(node),
            XML_NAME_STRING => PListString.ReadXml(node),
            _ => throw new XmlException($"Unknown PList element node type: {node.Name}."),
        };
    }
}
