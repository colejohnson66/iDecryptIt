using JetBrains.Annotations;

namespace PListLib;

[PublicAPI]
public enum PListElementType
{
    Array,
    Boolean,
    Data,
    Date,
    Dictionary,
    Document,
    Integer,
    Fill,
    Null,
    OrderedSet,
    Real,
    Set,
    String,
    Uid,
    Url,
    Uuid,
}
