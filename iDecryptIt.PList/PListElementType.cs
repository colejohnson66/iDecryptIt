using JetBrains.Annotations;

namespace iDecryptIt.PList;

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
