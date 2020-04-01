// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A helper performs resolving C++ types. 
    /// </summary>
    internal static class TypeResolver
    {
        /// <summary>
        /// Resolves ODCM type name to corresponding C++ type.
        /// </summary>
        /// <param name="odcmTypeName">The ODCM type name.</param>
        /// <returns>The resolved basic C++ type or user-defined type as a fallback.</returns>
        public static string ResolveType(string odcmTypeName)
        {
            string basicType;

            if (_odcmTypeNameToBasicTypeMap.TryGetValue(odcmTypeName, out basicType))
            {
                return basicType;
            }

            return NameConverter.CapitalizeName(odcmTypeName);
        }

        /// <summary>
        /// Defines mapping ODCM type name to basic C++ type.
        /// </summary>
        private static readonly IReadOnlyDictionary<string, string> _odcmTypeNameToBasicTypeMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Binary", BasicType.WideString },
                { "Boolean", BasicType.Boolean },
                { "Byte", BasicType.UnsignedChar },
                { "Date", BasicType.WideString },
                { "DateTimeOffset", BasicType.WideString },
                { "Double", BasicType.Double },
                { "Duration", BasicType.WideString },
                { "Float", BasicType.Float },
                { "Guid", BasicType.WideString },
                { "Int16", BasicType.SignedInt16 },
                { "Int32", BasicType.SignedInt32 },
                { "Int64", BasicType.SignedInt64 },
                { "Json", BasicType.WideString },
                { "NsDictionary", BasicType.Dictionary },
                { "Single", BasicType.Float },
                { "Stream", BasicType.WideString },
                { "String", BasicType.WideString },
                { "TimeOfDay", BasicType.WideString }
            };
    }
}
