// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A collection page entity.
    /// </summary>
    public sealed class CollectionPageEntity : BaseCollectionRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="CollectionPageEntity"/> class
        /// based on passed ODCM property.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property.</param>
        public CollectionPageEntity(OdcmProperty odcmProperty)
            : base(odcmProperty, isAbstract: false)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        protected override string GetFullEntityNameSuffix() => "Page";

        /// <inheritdoc/>
        protected override string GetBasePrimaryEntityName() => $"CollectionPage<{GetEntityName()}>";

        /// <inheritdoc/>
        protected override string GetBaseInterfaceEntityName() => $"I{GetFullEntityName()}";

        /// <inheritdoc/>
        protected override string GetEntityHeaderComment() => $"A page of {GetEntityName()} collection for {GetSuperClassEntityName()} entity";
    }
}
