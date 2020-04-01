// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using System.Collections.Generic;
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A C++ model enumeration.
    /// </summary>
    public sealed class ModelEnumEntity : BaseEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="ModelEnumEntity"/> class
        /// based on passed ODCM enumeration.
        /// </summary>
        /// <param name="odcmEnum">The ODCM enumeration.</param>
        public ModelEnumEntity(OdcmEnum odcmEnum)
            : base(odcmEnum)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            IncludeBlock includesBlock = new IncludeBlock();

            includesBlock.AppendFile(IncludeFile.StdString, isSystem: true);
            includesBlock.AppendFile(IncludeFile.CppRestSdkJson, isSystem: true);
            includesBlock.AppendLine();

            includesBlock.AppendFile(IncludeFile.GraphSdkStringUtils, isSystem: false);

            return includesBlock.ToString();
        }

        /// <inheritdoc/>
        public override string GenerateEntityHeader()
        {
            string entityName = GetEntityName();

            long minEnumValue;
            long maxEnumValue;

            GetEnumMinAndMaxValues(out minEnumValue, out maxEnumValue);

            string enumBaseType = GetEnumBaseType(minEnumValue, maxEnumValue);

            using (CodeBlock codeBlock = new CodeBlock(1))
            {
                codeBlock.AppendLine($"/// <summary>");
                codeBlock.AppendLine($"/// {entityName} model enumeration.");
                codeBlock.AppendLine($"/// </summary>");
                codeBlock.AppendLine($"enum class {entityName} : {enumBaseType}", newLine: false);

                return codeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates members of the enumeration.
        /// </summary>
        /// <returns>The string contains enumeration members separated by a new line.</returns>
        public string GenerateEnumMembers()
        {
            using (CodeBlock membersCodeBlock = new CodeBlock(2))
            {
                OdcmEnum odcmEnum = GetOdcmTypeAsEnum();
                IList<OdcmEnumMember> odcmEnumMembers = odcmEnum.Members;

                for (int i = 0; i < odcmEnumMembers.Count; ++i)
                {
                    OdcmEnumMember odcmEnumMember = odcmEnumMembers[i];

                    string enumValueName = NameConverter.CapitalizeName(odcmEnumMember.Name);
                    long? enumValue = odcmEnumMember.Value;

                    string enumMember = string.Format("{0}{1}{2}",
                        enumValueName,
                        enumValue.HasValue ? $" = {enumValue.Value}" : "",
                        i < odcmEnumMembers.Count - 1 ? "," : "");

                    if (i < odcmEnumMembers.Count - 1)
                    {
                        membersCodeBlock.AppendLine(enumMember);
                        membersCodeBlock.AppendLine();
                    }
                    else
                    {
                        membersCodeBlock.AppendLine(enumMember, newLine: false);
                    }
                }

                return membersCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Whether the enumeration entity contains flags only.
        /// </summary>
        /// <returns>True if enumeration entity contains flags only or False in other case.</returns>
        public bool IsEnumFlagsType()
        {
            OdcmEnum odcmEnum = GetOdcmTypeAsEnum();

            return odcmEnum.IsFlags;
        }

        /// <summary>
        /// Generates a bitwise "OR" operator for the enumeration entity.
        /// </summary>
        /// <returns>The string contains the operator definition.</returns>
        public string GenerateBitwiseOrOperator()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(1))
            {
                methodCodeBlock.AppendLine($"{entityName} operator|(const {entityName} lhs, const {entityName} rhs) noexcept");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    bodyCodeBlock.AppendLine($"return static_cast<{entityName}>(static_cast<std::underlying_type_t<{entityName}>>(lhs) | static_cast<std::underlying_type_t<{entityName}>>(rhs));");
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates a bitwise "AND" operator for the enumeration entity.
        /// </summary>
        /// <returns>The string contains the operator definition.</returns>
        public string GenerateBitwiseAndOperator()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(1))
            {
                methodCodeBlock.AppendLine($"{entityName} operator&(const {entityName} lhs, const {entityName} rhs) noexcept");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    bodyCodeBlock.AppendLine($"return static_cast<{entityName}>(static_cast<std::underlying_type_t<{entityName}>>(lhs) & static_cast<std::underlying_type_t<{entityName}>>(rhs));");
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates a bitwise "OR" assignment operator for the enumeration entity.
        /// </summary>
        /// <returns>The string contains the operator definition.</returns>
        public string GenerateBitwiseOrAssignmentOperator()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(1))
            {
                methodCodeBlock.AppendLine($"{entityName}& operator|=({entityName}& lhs, const {entityName} rhs) noexcept");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    bodyCodeBlock.AppendLine($"lhs = static_cast<{entityName}>(static_cast<std::underlying_type_t<{entityName}>>(lhs) | static_cast<std::underlying_type_t<{entityName}>>(rhs));");
                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine($"return lhs;");
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates a function to parse enumeration value.
        /// </summary>
        /// <returns>The string contains function definition.</returns>
        public string GenerateParseValueFunction()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(1))
            {
                methodCodeBlock.AppendLine($"inline bool ParseEnumValue(const std::wstring_view value, {entityName}& object) noexcept");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    bodyCodeBlock.AppendLine("const auto trimmedValue{ Utils::TrimString(value) };");
                    bodyCodeBlock.AppendLine();

                    OdcmEnum odcmEnum = GetOdcmTypeAsEnum();
                    IList<OdcmEnumMember> odcmEnumMembers = odcmEnum.Members;

                    for (int i = 0; i < odcmEnumMembers.Count; ++i)
                    {
                        OdcmEnumMember odcmEnumMember = odcmEnumMembers[i];

                        string enumMember = odcmEnumMember.Name;
                        string enumValueName = NameConverter.CapitalizeName(enumMember);

                        string ifCondition = i == 0 ? "if" : "else if";
                        bodyCodeBlock.AppendLine($"{ifCondition} (trimmedValue == L\"{enumMember}\")");

                        using (CodeBlock returnCodeBlock = new CodeBlock(bodyCodeBlock))
                        {
                            returnCodeBlock.AppendLine($"object = {entityName}::{enumValueName};");
                            returnCodeBlock.AppendLine();
                            returnCodeBlock.AppendLine("return true;");
                        }
                    }

                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine("return false;");
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates a JSON deserialize function for enumeration object.
        /// </summary>
        /// <returns>The string contains function definition.</returns>
        public string GenerateDeserializeFunction()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(1))
            {
                methodCodeBlock.AppendLine($"inline bool Deserialize(const web::json::value& jsonValue, {entityName}& object) noexcept");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    if (IsEnumFlagsType())
                    {
                        bodyCodeBlock.AppendLine($"object = static_cast<{entityName}>(0);");
                        bodyCodeBlock.AppendLine($"{entityName} enumObject{{ static_cast<{entityName}>(0) }}");
                        bodyCodeBlock.AppendLine();
                        bodyCodeBlock.AppendLine("const auto enumValues{ Utils::SplitLine(jsonValue.as_string() };");
                        bodyCodeBlock.AppendLine("for (const auto& enumValue : enumValues)");

                        using (CodeBlock foreachStatementCodeBlock = new CodeBlock(bodyCodeBlock))
                        {
                            foreachStatementCodeBlock.AppendLine("if (!ParseEnumValue(enumValue, enumObject))");

                            using (CodeBlock returnStatementCodeBlock = new CodeBlock(foreachStatementCodeBlock))
                            {
                                returnStatementCodeBlock.AppendLine("return false;");
                            }

                            foreachStatementCodeBlock.AppendLine();
                            foreachStatementCodeBlock.AppendLine("object |= enumObject;");
                        }

                        bodyCodeBlock.AppendLine();
                        bodyCodeBlock.AppendLine("return true;");
                    }
                    else
                    {
                        bodyCodeBlock.AppendLine("const auto enumValue{ jsonValue.as_string() };");
                        bodyCodeBlock.AppendLine();
                        bodyCodeBlock.AppendLine("return ParseEnumValue(enumValue, object);");
                    }
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Gets an enumeration minimum and maximum values.
        /// </summary>
        /// <param name="minEnumValue">The minimum enumeration value (out parameter).</param>
        /// <param name="maxEnumValue">The maximum enumeration value (out parameter).</param>
        private void GetEnumMinAndMaxValues(out long minEnumValue, out long maxEnumValue)
        {
            minEnumValue = long.MaxValue;
            maxEnumValue = long.MinValue;

            OdcmEnum odcmEnum = GetOdcmTypeAsEnum();
            IEnumerable<OdcmEnumMember> odcmEnumMembers = odcmEnum.Members;

            foreach (OdcmEnumMember odcmEnumMember in odcmEnumMembers)
            {
                long? odcmEnumValue = odcmEnumMember.Value;
                long enumValue = odcmEnumValue.HasValue ? odcmEnumValue.Value : 0;

                if (maxEnumValue < enumValue)
                {
                    maxEnumValue = enumValue;
                }

                if (minEnumValue > enumValue)
                {
                    minEnumValue = enumValue;
                }
            }
        }

        /// <summary>
        /// Determines convenient base type for an enumeration.
        /// </summary>
        /// <param name="minEnumValue">The minimum enumeration value.</param>
        /// <param name="maxEnumValue">The maximum enumeration value.</param>
        /// <returns>The string contains enumeration base type.</returns>
        private string GetEnumBaseType(long minEnumValue, long maxEnumValue)
        {
            if (minEnumValue < -uint.MinValue)
            {
                return BasicType.SignedInt64;
            }
            else if (minEnumValue >= 0 && maxEnumValue > uint.MaxValue)
            {
                return BasicType.UnsignedInt64;
            }
            else if (minEnumValue < -ushort.MinValue)
            {
                return BasicType.SignedInt32;
            }
            else if (minEnumValue >= 0 && maxEnumValue > ushort.MaxValue)
            {
                return BasicType.UnsignedInt32;
            }
            else if (minEnumValue < -byte.MinValue)
            {
                return BasicType.SignedInt16;
            }
            else if (minEnumValue >= 0 && maxEnumValue > byte.MaxValue)
            {
                return BasicType.UnsignedInt16;
            }
            else if (minEnumValue < 0)
            {
                return BasicType.SignedInt8;
            }

            return BasicType.UnsignedInt8;
        }
    }
}
