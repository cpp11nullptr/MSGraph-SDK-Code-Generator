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
    /// A request builder interface entity.
    /// </summary>
    public sealed class RequestBuilderInterfaceEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="RequestBuilderInterfaceEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        /// <param name="odcmClass">The ODCM class.</param>
        public RequestBuilderInterfaceEntity(OdcmClass odcmClass)
            : base(odcmClass, isAbstract: true)
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
            includesBlock.AppendLine();

            OdcmClass odcmClass = GetOdcmTypeAsClass();
            IEnumerable<OdcmProperty> navigationProperties = odcmClass.NavigationProperties();

            IEnumerable<string> navigationRequestBuilderInterfaceIncludeStatements =
                GenerateLinkedRequestBuilderIncludeStatements(navigationProperties, isPrototype: true);

            includesBlock.AppendFiles(navigationRequestBuilderInterfaceIncludeStatements, isSystem: false);
            includesBlock.AppendLine();

            return includesBlock.ToString();
        }

        /// <inheritdoc/>
        protected override string GetFullEntityName() => $"I{GetEntityName()}RequestBuilder";

        /// <inheritdoc/>
        protected override string GetBasePrimaryEntityName() => string.Empty;

        /// <inheritdoc/>
        protected override string GetBaseInterfaceEntityName() => "IBaseRequestBuilder";

        /// <inheritdoc/>
        protected override string GetEntityHeaderComment() => $"An interface of a builder to create a request for {GetEntityName()} entity";

        /// <summary>
        /// Generates the create request method declaration.
        /// </summary>
        /// <returns>The string contains method declaration.</returns>
        public string GenerateRequestMethodPrototype()
        {
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual std::unique_ptr<{requestInterfaceEntityName}> Request() noexcept = 0;");

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates request builder method declarations to navigate into linked entities.
        /// </summary>
        /// <returns>The string contains request builder methods declarations.</returns>
        public string GenerateNavigationRequestBuilderMethodPrototypes()
        {
            OdcmClass odcmClass = GetOdcmTypeAsClass();
            IEnumerable<OdcmProperty> navigationProperties = odcmClass.NavigationProperties();

            return GenerateLinkedRequestBuilderMethods(navigationProperties, isPrototype: true);
        }
    }
}
