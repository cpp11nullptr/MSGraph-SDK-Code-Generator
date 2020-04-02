// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A collection response entity.
    /// </summary>
    public sealed class CollectionResponseEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="CollectionResponseEntity"/> class
        /// based on passed ODCM property.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property.</param>
        public CollectionResponseEntity(OdcmProperty odcmProperty)
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
            string collectionRequestBuildEntityName = $"{collectionEntityName}Response";

            using (CodeBlock headerBlock = new CodeBlock(1))
            {
                headerBlock.AppendLine($"/*");
                headerBlock.AppendLine($" * A response for {entityName} collection for {collectionBaseEntityName} entity.");
                headerBlock.AppendLine($" */");
                headerBlock.AppendLine($"class {collectionRequestBuildEntityName} final", newLine: false);

                return headerBlock.ToString();
            }
        }
    }
}
