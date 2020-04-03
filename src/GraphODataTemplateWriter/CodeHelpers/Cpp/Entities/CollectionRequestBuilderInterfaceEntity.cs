// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A collection request builder interface entity.
    /// </summary>
    public sealed class CollectionRequestBuilderInterfaceEntity : BaseCollectionRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="CollectionRequestBuilderEntity"/> class
        /// based on passed ODCM property.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property.</param>
        public CollectionRequestBuilderInterfaceEntity(OdcmProperty odcmProperty)
            : base(odcmProperty, isAbstract: true)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        protected override string GetFullEntityNameSuffix() => $"RequestBuilder";

        /// <inheritdoc/>
        protected override string GetBasePrimaryEntityName() => string.Empty;

        /// <inheritdoc/>
        protected override string GetBaseInterfaceEntityName() => "IBaseRequestBuilder";

        /// <inheritdoc/>
        protected override string GetEntityHeaderComment() => $"An interface of a request builder for {GetEntityName()} collection for {GetSuperClassEntityName()} entity";
    }
}
