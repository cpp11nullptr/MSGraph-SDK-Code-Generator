// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A collection request entity.
    /// </summary>
    public sealed class CollectionRequestEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="CollectionRequestEntity"/> class
        /// based on passed ODCM property.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property.</param>
        public CollectionRequestEntity(OdcmProperty odcmProperty)
            : base(odcmProperty)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        public override string GenerateEntityHeader()
        {
            string entityName = GetEntityName();
            string collectionBaseEntityName = GetCollectionBaseEntityName();
            string collectionEntityName = GetCollectionEntityName();
            string collectionRequestBuildEntityName = $"{collectionEntityName}Request";

            using (CodeBlock headerBlock = new CodeBlock(1))
            {
                headerBlock.AppendLine($"/*");
                headerBlock.AppendLine($" * A request for {entityName} collection for {collectionBaseEntityName} entity.");
                headerBlock.AppendLine($" */");
                headerBlock.AppendLine($"class {collectionRequestBuildEntityName} final", newLine: false);

                return headerBlock.ToString();
            }
        }
    }
}
