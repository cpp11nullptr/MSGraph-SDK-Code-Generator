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
    /// A graph client entity.
    /// </summary>
    public sealed class GraphClientEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="GraphClientEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        /// <param name="odcmClass">The ODCM class.</param>
        /// <param name="odcmLinkedEntities">A list of linked entities.</param>
        public GraphClientEntity(OdcmClass odcmClass, IEnumerable<OdcmProperty> odcmLinkedEntities)
            : base(odcmClass, isAbstract: false)
        {
            this.odcmLinkedEntities = odcmLinkedEntities;
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        protected override string GetFullEntityName() => $"{GetEntityName()}";

        /// <inheritdoc/>
        protected override string GetBasePrimaryEntityName() => "BaseClient";

        /// <inheritdoc/>
        protected override string GetBaseInterfaceEntityName() => $"I{GetFullEntityName()}";

        /// <inheritdoc/>
        protected override string GetEntityHeaderComment() => $"A graph service client";

        /// <summary>
        /// Generates a constructor receives HTTP and authentication providers.
        /// </summary>
        /// <remarks>
        /// A predefined URL will be used for further requests to graph API.
        /// </remarks>
        /// <returns>The string contains constructor definition.</returns>
        public string GenerateConstructorWithAuthenticationProviderAndPredefinedGraphUrl()
        {
            string fullEntityName = GetFullEntityName();
            string basePrimaryEntityName = GetBasePrimaryEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"explicit {fullEntityName}(const std::shared_ptr<IAuthenticationProvider>& authenticationProvider, const std::shared_ptr<IHttpProvider>& httpProvider = nullptr) noexcept");
                methodCodeBlock.AppendLineShifted($": {basePrimaryEntityName}{{ \"{GraphUrl}\", authenticationProvider, httpProvider }}");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates a constructor receives HTTP and authentication providers and
        /// passing a custom URL used in further calls to graph API.
        /// </summary>
        /// <returns>The string contains constructor definition.</returns>
        public string GenerateConstructorWithAuthenticationProviderAndCustomGraphUrl()
        {
            string fullEntityName = GetFullEntityName();
            string basePrimaryEntityName = GetBasePrimaryEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"explicit {fullEntityName}(const std::wstring& baseUrl, const std::shared_ptr<IAuthenticationProvider>& authenticationProvider, const std::shared_ptr<IHttpProvider>& httpProvider = nullptr) noexcept");
                methodCodeBlock.AppendLineShifted($": {basePrimaryEntityName}{{ baseUrl, authenticationProvider, httpProvider }}");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates a constructor receives a custom HTTP client.
        /// </summary>
        /// <remarks>
        /// A predefined URL will be used for further requests to graph API.
        /// </remarks>
        /// <returns>The string contains constructor definition.</returns>
        public string GenerateConstructorWithHttpClient()
        {
            string fullEntityName = GetFullEntityName();
            string basePrimaryEntityName = GetBasePrimaryEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"explicit {fullEntityName}(const std::shared_ptr<HttpClient>& httpClient) noexcept");
                methodCodeBlock.AppendLineShifted($": {basePrimaryEntityName}{{ \"{GraphUrl}\", httpClient }}");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates request builder methods to navigate into linked entities.
        /// </summary>
        /// <returns>The string contains request builder methods definitions.</returns>
        public string GenerateLinkedRequestBuilderMethods()
        {
            return GenerateLinkedRequestBuilderMethods(odcmLinkedEntities);
        }

        /// <summary>
        /// Defines a defaul URL used for calls to graph API.
        /// </summary>
        private const string GraphUrl = "https://graph.microsoft.com/v1.0";

        /// <summary>
        /// A list of linked entities.
        /// </summary>
        private readonly IEnumerable<OdcmProperty> odcmLinkedEntities;
    }
}
