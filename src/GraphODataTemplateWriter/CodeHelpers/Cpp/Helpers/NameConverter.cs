// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A helper performs a name convertions.
    /// </summary>
    internal static class NameConverter
    {
        /// <summary>
        /// Performs a capitalization of a first char in the name.
        /// </summary>
        /// <param name="name">The name to be capitalized.</param>
        /// <returns>The capitalized name.</returns>
        public static string CapitalizeName(string name)
        {
            char[] nameArray = name.ToCharArray();
            nameArray[0] = char.ToUpper(nameArray[0]);

            return new string(nameArray);
        }

        /// <summary>
        /// Sanitizes a name to prevent a conflict with a reserved keyword.
        /// </summary>
        /// <param name="name">The name to be sanitized.</param>
        /// <returns>The sanitized name.</returns>
        public static string SanitizeName(string name)
        {
            if (Array.IndexOf(_reservedKeywords, name) > -1)
            {
                return $"{name}_";
            }

            return name;
        }

        /// <summary>
        /// Splits a camel cased string to separated lower cased tokens.
        /// </summary>
        /// <param name="name">The name to be split.</param>
        /// <returns>The split string.</returns>
        public static string SplitName(string name)
        {
            string splitName = Regex.Replace(name, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);

            return splitName.ToLowerInvariant();
        }

        /// <summary>
        /// Defines a list of reserved keywords in C++20 standard.
        /// </summary>
        private static string[] _reservedKeywords = new string[]
        {
            "alignas",
            "alignof",
            "and",
            "and_eq",
            "asm",
            "atomic_cancel",
            "atomic_commit",
            "atomic_noexcept",
            "auto",
            "bitand",
            "bitor",
            "bool",
            "break",
            "case",
            "catch",
            "char",
            "char8_t",
            "char16_t",
            "char32_t",
            "class",
            "compl",
            "concept",
            "const",
            "consteval",
            "constexpr",
            "const_cast",
            "continue",
            "co_await",
            "co_return",
            "co_yield",
            "decltype",
            "default",
            "delete",
            "do",
            "double",
            "dynamic_cast",
            "else",
            "enum",
            "explicit",
            "export",
            "extern",
            "false",
            "float",
            "for",
            "friend",
            "goto",
            "if",
            "inline",
            "int",
            "long",
            "mutable",
            "namespace",
            "new",
            "noexcept",
            "not",
            "not_eq",
            "nullptr",
            "operator",
            "or",
            "or_eq",
            "private",
            "protected",
            "public",
            "reflexpr",
            "register",
            "reinterpret_cast",
            "requires",
            "return",
            "short",
            "signed",
            "sizeof",
            "static",
            "static_assert",
            "static_cast",
            "struct",
            "switch",
            "synchronized",
            "template",
            "this",
            "thread_local",
            "throw",
            "true",
            "try",
            "typedef",
            "typeid",
            "typename",
            "union",
            "unsigned",
            "using",
            "virtual",
            "void",
            "volatile",
            "wchar_t",
            "while",
            "xor",
            "xor_eq"
        };
    }
}
