// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A request builder entity.
    /// </summary>
    public sealed class RequestBuilderEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="RequestBuilderEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        /// <param name="odcmClass">The ODCM class.</param>
        public RequestBuilderEntity(OdcmClass odcmClass)
            : base(odcmClass)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();
            string requestBuilderInterfaceEntityName = GetRequestBuilderInterfaceEntityName();

            IncludeBlock includesBlock = new IncludeBlock();

            includesBlock.AppendFile(IncludeFile.StdMemory, isSystem: true);
            includesBlock.AppendFile(IncludeFile.StdString, isSystem: true);
            includesBlock.AppendLine();

            includesBlock.AppendFile($"{requestInterfaceEntityName}.h", isSystem: false);
            includesBlock.AppendFile($"{requestBuilderInterfaceEntityName}.h", isSystem: false);
            includesBlock.AppendLine();

            includesBlock.AppendFile(IncludeFile.GraphSdkBaseClientInterface, isSystem: false);
            includesBlock.AppendFile(IncludeFile.GraphSdkBaseRequestBuilder, isSystem: false);

            return includesBlock.ToString();
        }

        /// <inheritdoc/>
        public override string GenerateEntityHeader()
        {
            string entityName = GetEntityName();
            string requestBuilderEntityName = GetRequestBuilderEntityName();
            string baseClassesList = GetBaseClassesList();

            using (CodeBlock codeBlock = new CodeBlock(1))
            {
                codeBlock.AppendLine($"/// <summary>");
                codeBlock.AppendLine($"/// A builder to create a request for {entityName} entity.");
                codeBlock.AppendLine($"/// </summary>");
                codeBlock.AppendLine($"class {requestBuilderEntityName} final");
                codeBlock.AppendLineShifted($": {baseClassesList}", newLine: false);

                return codeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the request builder contructor.
        /// </summary>
        /// <returns>The string contains constructor definition.</returns>
        public string GenerateConstructor()
        {
            string requestBuilderEntityName = GetRequestBuilderEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"explicit {requestBuilderEntityName}(const std::wstring& requestUrl, IBaseClient& baseClient) noexcept");
                methodCodeBlock.AppendLineShifted(": BaseRequestBuilder{ requestUrl, baseClient }");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates a request builder destructor.
        /// </summary>
        /// <returns>The string contains destructor definition.</returns>
        public string GenerateDestructor()
        {
            string requestBuilderEntityName = GetRequestBuilderEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"~{requestBuilderEntityName}() noexcept override");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the create request method.
        /// </summary>
        /// <returns>The string contains method definition.</returns>
        public string GenerateCreateRequestMethod()
        {
            string requestEntityName = GetRequestEntityName();
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"std::unique_ptr<{requestInterfaceEntityName}> CreateRequest() noexcept override");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    bodyCodeBlock.AppendLine("const std::wstring& baseUrl{ GetBaseUrl() };");
                    bodyCodeBlock.AppendLine("IBaseClient& baseClient{ GetBaseClient() };");
                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine($"return std::make_unique<{requestEntityName}>(baseUrl, baseClient);");
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Gets a list of base entities for the request builder.
        /// </summary>
        /// <returns>The string contains base entities.</returns>
        private string GetBaseClassesList()
        {
            return $"public {GetRequestBuilderInterfaceEntityName()}, public BaseRequestBuilder";
        }
    }
}
