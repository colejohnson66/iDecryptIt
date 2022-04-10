/* =============================================================================
 * File:   PListDocument.cs
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
using System.Xml;

namespace PListLib;

[PublicAPI]
public class PListDocument : IPListElement<IPListElement>
{
    public PListElementType Type => PListElementType.Document;
    public bool SerializableAsXml => Value.SerializableAsXml;
    public dynamic UntypedValue => Value;
    public IPListElement Value { get; set; }

    public PListDocument(string contents)
    {
        Value = ReadXml(
            new()
            {
                InnerXml = contents,
            });
    }

    private static IPListElement ReadXml(XmlDocument doc)
    {
        foreach (XmlNode node in doc.ChildNodes)
        {
            if (node.NodeType is not XmlNodeType.Element)
                continue;

            if (node.Name.ToLowerInvariant() is not "plist")
                throw new XmlException("Root node is not a <plist> element.");

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType is not XmlNodeType.Element)
                    continue;

                // CoreFoundation ignores anything but the first child node, so return that one
                return PListHelpers.ParseNode(child);
            }
        }
        throw new XmlException("No root node.");
    }
}
