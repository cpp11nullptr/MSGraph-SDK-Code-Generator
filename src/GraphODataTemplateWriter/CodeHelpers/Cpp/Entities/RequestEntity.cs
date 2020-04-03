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
    /// A request entity.
    /// </summary>
    public sealed class RequestEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="RequestEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        /// <param name="odcmClass">The ODCM class.</param>
        public RequestEntity(OdcmClass odcmClass)
            : base(odcmClass, isAbstract: false)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            string entityName = GetEntityName();
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();

            IncludeBlock includesBlock = new IncludeBlock();

            includesBlock.AppendFile(IncludeFile.CppRestSdkHttpMsg, isSystem: true);
            includesBlock.AppendFile(IncludeFile.PplxCancellationToken, isSystem: true);
            includesBlock.AppendFile(IncludeFile.StdFuture, isSystem: true);
            includesBlock.AppendFile(IncludeFile.StdString, isSystem: true);
            includesBlock.AppendLine();

            includesBlock.AppendFile($"{requestInterfaceEntityName}.h", isSystem: false);
            includesBlock.AppendFile($"{entityName}.h", isSystem: false);
            includesBlock.AppendLine();

            includesBlock.AppendFile(IncludeFile.GraphSdkBaseClientInterface, isSystem: false);
            includesBlock.AppendFile(IncludeFile.GraphSdkBaseRequest, isSystem: false);
            includesBlock.AppendLine();

            return includesBlock.ToString();
        }

        /// <inheritdoc/>
        protected override string GetFullEntityName() => $"{GetEntityName()}Request";

        /// <inheritdoc/>
        protected override string GetBasePrimaryEntityName() => "BaseRequest";

        /// <inheritdoc/>
        protected override string GetBaseInterfaceEntityName() => $"I{GetFullEntityName()}";

        /// <inheritdoc/>
        protected override string GetEntityHeaderComment() => $"A request for {GetEntityName()} entity";

        /// <summary>
        /// Generates the get method.
        /// </summary>
        /// <returns>The string contains method definition.</returns>
        public string GenerateGetMethod()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"std::future<{entityName}> GetAsync(const pplx::cancellation_token& token) noexcept override");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    bodyCodeBlock.AppendLine("constexpr auto method{ web::http::methods::GET };");
                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine($"const {entityName} responseEntity{{ co_await SendAsync<{entityName}>(method, token) }};");
                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine("co_return responseEntity;");
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the create method.
        /// </summary>
        /// <returns>The string contains method definition.</returns>
        public string GenerateCreateMethod()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"std::future<{entityName}> CreateAsync(const {entityName}& entity, const pplx::cancellation_token& token) noexcept override");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    bodyCodeBlock.AppendLine("constexpr auto method{ web::http::methods::POST };");
                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine($"const {entityName} responseEntity{{ co_await SendAsync<{entityName}>(entity, method, token) }};");
                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine("co_return responseEntity;");
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the update method.
        /// </summary>
        /// <returns>The string contains method definition.</returns>
        public string GenerateUpdateMethod()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"std::future<{entityName}> UpdateAsync(const {entityName}& entity, const pplx::cancellation_token& token) noexcept override");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    bodyCodeBlock.AppendLine("constexpr auto method{ web::http::methods::PATCH };");
                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine($"const {entityName} responseEntity{{ co_await SendAsync<{entityName}>(entity, method, token) }};");
                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine("co_return responseEntity;");
                }

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the delete method.
        /// </summary>
        /// <returns>The string contains method definition.</returns>
        public string GenerateDeleteMethod()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"std::future<void> DeleteAsync(const {entityName}& entity, const pplx::cancellation_token& token) noexcept override");

                using (CodeBlock bodyCodeBlock = new CodeBlock(methodCodeBlock))
                {
                    bodyCodeBlock.AppendLine("constexpr auto method{ web::http::methods::DELETE };");
                    bodyCodeBlock.AppendLine();
                    bodyCodeBlock.AppendLine($"co_await SendAsync<{entityName}>(entity, method, token);");
                }

                return methodCodeBlock.ToString();
            }
        }
    }
}
