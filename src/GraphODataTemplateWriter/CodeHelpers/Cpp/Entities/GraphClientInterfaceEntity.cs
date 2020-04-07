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
    /// A graph client interface entity.
    /// </summary>
    public sealed class GraphClientInterfaceEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="GraphClientInterfaceEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        /// <param name="odcmClass">The ODCM class.</param>
        /// <param name="odcmLinkedEntities">A list of linked entities.</param>
        public GraphClientInterfaceEntity(OdcmClass odcmClass, IEnumerable<OdcmProperty> odcmLinkedEntities)
            : base(odcmClass, isAbstract: true)
        {
            this.odcmLinkedEntities = odcmLinkedEntities;
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            string baseInterfaceEntityName = GetBaseInterfaceEntityName();

            IncludeBlock includesBlock = new IncludeBlock();

            includesBlock.AppendFile(IncludeFile.StdMemory, isSystem: true);
            includesBlock.AppendLine();

            includesBlock.AppendFile($"{baseInterfaceEntityName}.h", isSystem: false);
            includesBlock.AppendLine();

            IEnumerable<string> linkedRequestBuilderInterfaceIncludeStatements =
                GenerateLinkedRequestBuilderIncludeStatements(odcmLinkedEntities, isPrototype: true);

            includesBlock.AppendFiles(linkedRequestBuilderInterfaceIncludeStatements, isSystem: false);
            includesBlock.AppendLine();

            return includesBlock.ToString();
        }

        /// <inheritdoc/>
        protected override string GetFullEntityName() => $"I{GetEntityName()}";

        /// <inheritdoc/>
        protected override string GetBasePrimaryEntityName() => string.Empty;

        /// <inheritdoc/>
        protected override string GetBaseInterfaceEntityName() => $"IBaseClient";

        /// <inheritdoc/>
        protected override string GetEntityHeaderComment() => $"An interface of graph service client";

        /// <summary>
        /// Generates request builder method prototypes to navigate into linked entities.
        /// </summary>
        /// <returns>The string contains request builder method definitions.</returns>
        public string GenerateLinkedRequestBuilderMethodPrototypes()
        {
            return GenerateLinkedRequestBuilderMethods(odcmLinkedEntities, isPrototype: true);
        }

        /// <summary>
        /// A list of linked entities.
        /// </summary>
        private readonly IEnumerable<OdcmProperty> odcmLinkedEntities;
    }
}
