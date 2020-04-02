// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Microsoft.Graph.ODataTemplateWriter.Extensions;
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
            string requestEntityName = GetRequestEntityName();
            string requestBuilderInterfaceEntityName = GetRequestBuilderInterfaceEntityName();

            IncludeBlock includesBlock = new IncludeBlock();

            includesBlock.AppendFile(IncludeFile.StdMemory, isSystem: true);
            includesBlock.AppendFile(IncludeFile.StdString, isSystem: true);
            includesBlock.AppendLine();

            includesBlock.AppendFile(IncludeFile.GraphSdkBaseClientInterface, isSystem: false);
            includesBlock.AppendFile(IncludeFile.GraphSdkBaseRequestBuilder, isSystem: false);
            includesBlock.AppendLine();

            includesBlock.AppendFile($"{requestBuilderInterfaceEntityName}.h", isSystem: false);
            includesBlock.AppendFile($"{requestEntityName}.h", isSystem: false);
            includesBlock.AppendLine();

            IEnumerable<string> navigationRequestBuilderIncludeStatements =
                GenerateNavigationRequestBuilderIncludeStatements(isInterface: false);

            includesBlock.AppendFiles(navigationRequestBuilderIncludeStatements, isSystem: false);
            includesBlock.AppendLine();

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
                codeBlock.AppendLine($"/*");
                codeBlock.AppendLine($" * A builder to create a request for {entityName} entity.");
                codeBlock.AppendLine($" */");
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
        public string GenerateRequestMethod()
        {
            string requestEntityName = GetRequestEntityName();
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"std::unique_ptr<{requestInterfaceEntityName}> Request() noexcept override");

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
        /// Generates request builder methods to navigate into linked entities.
        /// </summary>
        /// <returns>The string contains request builder methods definitions.</returns>
        public string GenerateNavigationRequestBuilderMethods()
        {
            OdcmClass odcmClass = GetOdcmTypeAsClass();
            IEnumerable<OdcmProperty> navigationProperties = odcmClass.NavigationProperties();
            int navigationPropertiesCount = navigationProperties.Count();

            if (navigationPropertiesCount == 0)
            {
                return string.Empty;
            }

            IList<string> requestBuilderMethods = new List<string>(navigationPropertiesCount);

            foreach (OdcmProperty navigationProperty in navigationProperties)
            {
                string requestBuilderMethod = GenerateNavigationRequestBuilderMethod(navigationProperty);

                requestBuilderMethods.Add(requestBuilderMethod);
            }

            string requestBuilderMethodDefinitions = string.Join("\n", requestBuilderMethods);

            return requestBuilderMethodDefinitions;
        }

        /// <summary>
        /// Gets a list of base entities for the request builder.
        /// </summary>
        /// <returns>The string contains base entities.</returns>
        private string GetBaseClassesList()
        {
            return $"public {GetRequestBuilderInterfaceEntityName()}, public BaseRequestBuilder";
        }

        /// <summary>
        /// Generates the request builder method to navigate into linked entity.
        /// </summary>
        /// <param name="navigationProperty">The ODCM property contains navigation details.</param>
        /// <returns>The string contains request builder method definitions.</returns>
        private string GenerateNavigationRequestBuilderMethod(OdcmProperty navigationProperty)
        {
            string navigationEntityBaseName = NameConverter.CapitalizeName(navigationProperty.Name);
            string navigationPath = navigationProperty.Name;

            string navigationEntityName = navigationProperty.IsCollection ?
                $"{GetEntityName()}{navigationEntityBaseName}Collection" :
                navigationProperty.Name;

            string navigationRequestBuilderEntityName =
                GetRequestBuilderEntityName(navigationEntityName);

            string navigationRequestBuilderInterfaceEntityName =
                GetRequestBuilderInterfaceEntityName(navigationEntityName);

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"std::unique_ptr<{navigationRequestBuilderInterfaceEntityName}> {navigationEntityBaseName}() noexcept override");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    bodyCodeBlock.AppendLine($"const std::wstring& navigationUrl{{ ExtendBaseUrl(\"{navigationPath}\") }};");
                    bodyCodeBlock.AppendLine("IBaseClient& baseClient{ GetBaseClient() };");
                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine($"return std::make_unique<{navigationRequestBuilderEntityName}>(navigationUrl, baseClient);");
                }

                return methodCodeBlock.ToString();
            }
        }
    }
}
