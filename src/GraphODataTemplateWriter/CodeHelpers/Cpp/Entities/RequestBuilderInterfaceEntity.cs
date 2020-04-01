// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A request builder interface.
    /// </summary>
    public sealed class RequestBuilderInterfaceEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="RequestBuilderInterfaceEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        /// <param name="odcmClass">The ODCM class.</param>
        public RequestBuilderInterfaceEntity(OdcmClass odcmClass)
            : base(odcmClass)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();

            IncludeBlock includesBlock = new IncludeBlock();

            includesBlock.AppendFile(IncludeFile.StdMemory, isSystem: true);
            includesBlock.AppendLine();

            includesBlock.AppendFile($"{requestInterfaceEntityName}.h", isSystem: false);

            return includesBlock.ToString();
        }

        /// <inheritdoc/>
        public override string GenerateEntityHeader()
        {
            string entityName = GetEntityName();
            string requestBuilderInterfaceEntityName = GetRequestBuilderInterfaceEntityName();

            using (CodeBlock codeBlock = new CodeBlock(1))
            {
                codeBlock.AppendLine($"/// <summary>");
                codeBlock.AppendLine($"/// An interface of a builder to create a request for {entityName} entity.");
                codeBlock.AppendLine($"/// </summary>");
                codeBlock.AppendLine($"struct {requestBuilderInterfaceEntityName}", newLine: false);

                return codeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the request builder interface destructor declaration.
        /// </summary>
        /// <returns>The string contains destructor declaration.</returns>
        public string GenerateDestructor()
        {
            string requestBuilderInterfaceEntityName = GetRequestBuilderInterfaceEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual ~{requestBuilderInterfaceEntityName}() noexcept = default;");

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the create request method declaration.
        /// </summary>
        /// <returns>The string contains method declaration.</returns>
        public string GenerateCreateRequestMethodPrototype()
        {
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual std::unique_ptr<{requestInterfaceEntityName}> CreateRequest() noexcept = 0;");

                return methodCodeBlock.ToString();
            }
        }
    }
}
