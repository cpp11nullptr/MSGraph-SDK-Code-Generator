// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A collection response entity.
    /// </summary>
    public sealed class CollectionResponseEntity : BaseCollectionRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="CollectionResponseEntity"/> class
        /// based on passed ODCM property.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property.</param>
        public CollectionResponseEntity(OdcmProperty odcmProperty)
            : base(odcmProperty, isAbstract: false)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        protected override string GetFullEntityNameSuffix() => "Response";

        /// <inheritdoc/>
        protected override string GetBasePrimaryEntityName() => string.Empty;

        /// <inheritdoc/>
        protected override string GetBaseInterfaceEntityName() => string.Empty;

        /// <inheritdoc/>
        protected override string GetEntityHeaderComment() => $"A response for {GetEntityName()} collection for {GetSuperClassEntityName()} entity";
    }
}
