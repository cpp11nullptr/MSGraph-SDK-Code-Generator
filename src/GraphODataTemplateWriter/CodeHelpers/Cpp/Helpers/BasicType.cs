// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers
{
    /// <summary>
    /// Defines basic C++ types.
    /// </summary>
    internal static class BasicType
    {
        public const string Boolean = "bool";

        public const string SignedChar = "char";

        public const string SignedInt8 = "std::int8_t";

        public const string SignedInt16 = "std::int16_t";

        public const string SignedInt32 = "std::int32_t";

        public const string SignedInt64 = "std::int64_t";

        public const string UnsignedChar = "unsigned char";

        public const string UnsignedInt8 = "std::uint8_t";

        public const string UnsignedInt16 = "std::uint16_t";

        public const string UnsignedInt32 = "std::uint32_t";

        public const string UnsignedInt64 = "std::uint64_t";

        public const string Float = "float";

        public const string Double = "double";

        public const string String = "std::string";

        public const string WideString = "std::wstring";

        public const string Any = "std::any";

        public const string Vector = "std::vector";

        public const string Map = "std::map";

        public const string Dictionary = "std::map<std::wstring, std::any>";
    }
}
