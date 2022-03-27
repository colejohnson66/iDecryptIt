/* =============================================================================
 * File:   PListDictionary.cs
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

using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace iDecryptIt.PList;

[PublicAPI]
public class PListDictionary : IPListElement<Dictionary<string, IPListElement>>, IPListElementInternals
{
    public PListElementType Type => PListElementType.Dictionary;

    public bool SerializableAsXml => true;
    public dynamic UntypedValue => Value;
    public Dictionary<string, IPListElement> Value { get; set; }

    public PListDictionary(Dictionary<string, IPListElement> value)
    {
        Value = value;
    }

    internal static PListDictionary ReadXml(XmlNode node)
    {
        Debug.Assert(node.NodeType is XmlNodeType.Element);
        Debug.Assert(node.Name.ToLowerInvariant() is PListHelpers.XML_NAME_DICTIONARY);

        Dictionary<string, IPListElement> children = new(node.ChildNodes.Count / 2);
        string? key = null;
        foreach (XmlNode child in node.ChildNodes)
        {
            if (child.NodeType is not XmlNodeType.Element)
                continue;

            string nodeName = child.Name.ToLowerInvariant();
            if (nodeName is "key")
            {
                if (key is not null)
                    throw new XmlException("Two consecutive <key> entries in a <dict> is not allowed. Each <key> must have a value.");
                key = child.InnerText;
                continue;
            }

            // value node
            if (key is null)
                throw new XmlException("A value encountered without a corresponding <key> entry.");
            children.Add(key, PListHelpers.ParseNode(child));
            key = null;
        }
        return new(children);
    }
}
