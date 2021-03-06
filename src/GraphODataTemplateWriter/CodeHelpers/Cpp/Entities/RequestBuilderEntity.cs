﻿// Copyright (c) Microsoft Corporation.
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
            : base(odcmClass, isAbstract: false)
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

            OdcmClass odcmClass = GetOdcmTypeAsClass();
            IEnumerable<OdcmProperty> navigationProperties = odcmClass.NavigationProperties();

            IEnumerable<string> navigationRequestBuilderIncludeStatements =
                GenerateLinkedRequestBuilderIncludeStatements(navigationProperties, isPrototype: false);

            includesBlock.AppendFiles(navigationRequestBuilderIncludeStatements, isSystem: false);
            includesBlock.AppendLine();

            return includesBlock.ToString();
        }

        /// <inheritdoc/>
        protected override string GetFullEntityName() => $"{GetEntityName()}RequestBuilder";

        /// <inheritdoc/>
        protected override string GetBasePrimaryEntityName() => "BaseRequestBuilder";

        /// <inheritdoc/>
        protected override string GetBaseInterfaceEntityName() => $"I{GetFullEntityName()}";

        /// <inheritdoc/>
        protected override string GetEntityHeaderComment() => $"A builder to create a request for {GetEntityName()} entity";

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

            return GenerateLinkedRequestBuilderMethods(navigationProperties, isPrototype: false);
        }
    }
}
