using System;

namespace GameLogic
{
    // The interface represents the serialization/deserialization to/from FEN notation.
    // Please refer to the FEN notation (https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation).
    public interface IFENSerializable<T>
    {
        // Serialize to FEN notation.
        string SerializeToFEN(T objectToSerialize);

        // Deserialize from FEN notation.
        T DeserializeFromFEN(string fenNotation);
    }
}
