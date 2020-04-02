// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A C++ request interface.
    /// </summary>
    public sealed class RequestInterfaceEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="RequestInterfaceEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        /// <param name="odcmClass">The ODCM class.</param>
        public RequestInterfaceEntity(OdcmClass odcmClass)
            : base(odcmClass)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            string entityName = GetEntityName();

            IncludeBlock includesBlock = new IncludeBlock();

            includesBlock.AppendFile(IncludeFile.StdFuture, isSystem: true);
            includesBlock.AppendLine();

            includesBlock.AppendFile($"{entityName}.h", isSystem: false);
            includesBlock.AppendLine();

            return includesBlock.ToString();
        }

        /// <inheritdoc/>
        public override string GenerateEntityHeader()
        {
            string entityName = GetEntityName();
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();

            using (CodeBlock headerBlock = new CodeBlock(1))
            {
                headerBlock.AppendLine($"/*");
                headerBlock.AppendLine($" * An interface of a request for {entityName} entity.");
                headerBlock.AppendLine($" */");
                headerBlock.AppendLine($"struct {requestInterfaceEntityName}", newLine: false);

                return headerBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the request interface destructor declaration.
        /// </summary>
        /// <returns>The string contains destructor declaration.</returns>
        public string GenerateDestructor()
        {
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual ~{requestInterfaceEntityName}() noexcept = default;");

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the get method declaration.
        /// </summary>
        /// <returns>The string contains method declaration.</returns>
        public string GenerateGetMethodPrototype()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual std::future<{entityName}> GetAsync(const pplx::cancellation_token& token) noexcept = 0;");

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the create method declaration.
        /// </summary>
        /// <returns>The string contains method declaration.</returns>
        public string GenerateCreateMethodPrototype()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual std::future<{entityName}> CreateAsync(const {entityName}& entity, const pplx::cancellation_token& token) noexcept = 0;");

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the update method declaration.
        /// </summary>
        /// <returns>The string contains method declaration.</returns>
        public string GenerateUpdateMethodPrototype()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual std::future<{entityName}> UpdateAsync(const {entityName}& entity, const pplx::cancellation_token& token) noexcept = 0;");

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the delete method declaration.
        /// </summary>
        /// <returns>The string contains method declaration.</returns>
        public string GenerateDeleteMethodPrototype()
        {
            string entityName = GetEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual std::future<void> DeleteAsync(const {entityName}& entity, const pplx::cancellation_token& token) noexcept = 0;");

                return methodCodeBlock.ToString();
            }
        }
    }
}
