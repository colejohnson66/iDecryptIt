/* =============================================================================
 * File:   PListData.cs
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
using System;
using System.Diagnostics;
using System.Xml;

namespace iDecryptIt.PList;

[PublicAPI]
public class PListData : IPListElement<byte[]>, IPListElementInternals
{
    public PListElementType Type => PListElementType.Data;
    public bool SerializableAsXml => true;
    public dynamic UntypedValue => Value;
    public byte[] Value { get; set; }

    public PListData(byte[] value)
    {
        Value = new byte[value.Length];
        Array.Copy(value, Value, value.Length);
    }
    public PListData(string base64)
    {
        base64 = base64
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("\t", "")
            .Replace(" ", "");
        Value = Convert.FromBase64String(base64);
    }

    internal static PListData ReadXml(XmlNode node)
    {
        Debug.Assert(node.NodeType is XmlNodeType.Element);
        Debug.Assert(node.Name.ToLowerInvariant() is PListHelpers.XML_NAME_DATA);

        return new(node.InnerText);
    }
}
