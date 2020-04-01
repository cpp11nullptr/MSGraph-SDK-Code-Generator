// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using System.Collections.Generic;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A C++ model type.
    /// </summary>
    public sealed class ModelTypeEntity : BaseEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="ModelTypeEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        /// <param name="odcmClass">The ODCM class.</param>
        public ModelTypeEntity(OdcmClass odcmClass)
            : base(odcmClass)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            IncludeBlock includesBlock = new IncludeBlock();


            IEnumerable<string> membersTypes = GetMembersTypes();

            includesBlock.AppendFilesFromTypes(membersTypes, includeSystemOnly: true);
            includesBlock.AppendFile(IncludeFile.CppRestSdkJson, isSystem: true);
            includesBlock.AppendLine();

            includesBlock.AppendFilesFromTypes(membersTypes, includeSystemOnly: false);
            includesBlock.AppendFile(IncludeFile.GraphSdkStringUtils, isSystem: false);

            return includesBlock.ToString();
        }

        /// <inheritdoc/>
        public override string GenerateEntityHeader()
        {
            string entityName = GetEntityName();

            using (CodeBlock codeBlock = new CodeBlock(1))
            {
                codeBlock.AppendLine($"/// <summary>");
                codeBlock.AppendLine($"/// {entityName} model type.");
                codeBlock.AppendLine($"/// </summary>");

                if (HasBaseEntity())
                {
                    string baseEntityName = GetBaseEntityName();

                    codeBlock.AppendLine($"class {entityName}");
                    codeBlock.AppendLineShifted($": public {baseEntityName}", newLine: false);
                }
                else
                {
                    codeBlock.AppendLine($"class {entityName}", newLine: false);
                }

                return codeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates members of the type.
        /// </summary>
        /// <remarks>
        /// Each member will contain a comment block.
        /// </remarks>
        /// <returns>The string contains type members separated by a new line.</returns>
        public string GenerateTypeMembers()
        {
            using (CodeBlock membersCodeBlock = new CodeBlock(2))
            {
                bool hasBaseEntity = HasBaseEntity();

                OdcmClass odcmClass = GetOdcmTypeAsClass();
                IList<OdcmProperty> odcmProperties = odcmClass.Properties;

                for (int i = 0; i < odcmProperties.Count; ++i)
                {
                    OdcmProperty odcmProperty = odcmProperties[i];
                    OdcmType odcmType = odcmProperty.Projection.Type;

                    string memberType = TypeResolver.ResolveType(odcmType.Name);
                    string memberName = NameConverter.SanitizeName(odcmProperty.Name);

                    string memberSummary = odcmProperty.LongDescription;

                    if (string.IsNullOrWhiteSpace(memberSummary))
                    {
                        string splitName = NameConverter.SplitName(odcmProperty.Name);

                        memberSummary = $"The {splitName}.";
                    }

                    membersCodeBlock.AppendLine($"/// <summary>");
                    membersCodeBlock.AppendLine($"/// {memberSummary}");
                    membersCodeBlock.AppendLine($"/// </summary>");

                    bool appendNewLine = i < odcmProperties.Count - 1 || !hasBaseEntity;

                    if (!odcmProperty.IsCollection)
                    {
                        membersCodeBlock.AppendLine($"{memberType} {memberName};", newLine: appendNewLine);
                    }
                    else
                    {
                        membersCodeBlock.AppendLine($"{BasicType.Vector}<{memberType}> {memberName};", newLine: appendNewLine);
                    }

                    if (appendNewLine)
                    {
                        membersCodeBlock.AppendLine();
                    }
                }

                if (!hasBaseEntity)
                {
                    membersCodeBlock.AppendLine($"/// <summary>");
                    membersCodeBlock.AppendLine($"/// Open data protocol payload.");
                    membersCodeBlock.AppendLine($"/// </summary>");
                    membersCodeBlock.AppendLine($"{BasicType.WideString} odata;");
                    membersCodeBlock.AppendLine();

                    membersCodeBlock.AppendLine($"/// <summary>");
                    membersCodeBlock.AppendLine($"/// Additional data.");
                    membersCodeBlock.AppendLine($"/// </summary>");
                    membersCodeBlock.AppendLine($"{BasicType.Dictionary} additionalData;", newLine: false);
                }

                return membersCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates a JSON deserialize function for type object.
        /// </summary>
        /// <returns>The string contains function definition.</returns>
        public string GenerateDeserializeFunction()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(1))
            {
                bool hasBaseEntity = HasBaseEntity();

                methodCodeBlock.AppendLine($"inline bool Deserialize(const web::json::value& jsonValue, {entityName}& object) noexcept");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    if (hasBaseEntity)
                    {
                        string baseEntityName = GetBaseEntityName();

                        WriteDeserializeSinglePropertyBlock(bodyCodeBlock, propertyValue: "jsonValue", memberName: $"static_cast<{baseEntityName}&>(object)");

                        bodyCodeBlock.AppendLine();
                    }

                    OdcmClass odcmClass = GetOdcmTypeAsClass();

                    foreach (OdcmProperty odcmProperty in odcmClass.Properties)
                    {
                        string propertyName = odcmProperty.Name;
                        string memberName = NameConverter.SanitizeName(propertyName);

                        if (!odcmProperty.IsCollection)
                        {
                            WriteDeserializeSinglePropertyBlock(bodyCodeBlock, propertyValue: $"jsonValue.at(U\"{propertyName}\")", memberName: $"object.{memberName}");
                        }
                        else
                        {
                            OdcmType odcmType = odcmProperty.Projection.Type;

                            string memberType = TypeResolver.ResolveType(odcmType.Name);

                            WriteDeserializeCollectionPropertyBlock(bodyCodeBlock, propertyValue: $"jsonValue.at(U\"{propertyName}\")", memberName: $"object.{memberName}", memberType);
                        }

                        bodyCodeBlock.AppendLine();
                    }

                    if (!hasBaseEntity)
                    {
                        WriteDeserializeSinglePropertyBlock(bodyCodeBlock, propertyValue: "jsonValue.at(U\"@odata.type\")", memberName: "object.odata");

                        bodyCodeBlock.AppendLine();
                    }

                    bodyCodeBlock.AppendLine("return true;");
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Gets type of members of the type.
        /// </summary>
        /// <returns>The string type member types.</returns>
        private IEnumerable<string> GetMembersTypes()
        {
            HashSet<string> membersTypes = new HashSet<string>();

            OdcmClass odcmClass = GetOdcmTypeAsClass();

            foreach (OdcmProperty odcmProperty in odcmClass.Properties)
            {
                OdcmType odcmType = odcmProperty.Projection.Type;

                string memberType = TypeResolver.ResolveType(odcmType.Name);

                membersTypes.Add(memberType);
            }

            if (HasBaseEntity())
            {
                string baseEntityName = GetBaseEntityName();

                membersTypes.Add(baseEntityName);
            }
            else
            {
                membersTypes.Add(BasicType.Any);
                membersTypes.Add(BasicType.WideString);
                membersTypes.Add(BasicType.Dictionary);
            }

            return membersTypes;
        }

        /// <summary>
        /// A helper function to write a deserialization of scalar type member.
        /// </summary>
        /// <param name="codeBlock">The code block to write into.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="memberName">The member name.</param>
        private void WriteDeserializeSinglePropertyBlock(CodeBlock codeBlock, string propertyValue, string memberName)
        {
            codeBlock.AppendLine($"if (!Deserialize({propertyValue}, {memberName})");

            using (CodeBlock returnStatementCodeBlock = new CodeBlock(codeBlock))
            {
                returnStatementCodeBlock.AppendLine("return false;");
            }
        }

        /// <summary>
        /// A helper function to write a deserialization of collection type member.
        /// </summary>
        /// <param name="codeBlock">The code block to write into.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="memberName">The member name.</param>
        /// <param name="memberType">The member type.</param>
        private void WriteDeserializeCollectionPropertyBlock(CodeBlock codeBlock, string propertyValue, string memberName, string memberType)
        {
            codeBlock.AppendLine($"if (!Deserialize<{memberType}>({propertyValue}, {memberName})");

            using (CodeBlock returnStatementCodeBlock = new CodeBlock(codeBlock))
            {
                returnStatementCodeBlock.AppendLine("return false;");
            }
        }
    }
}
