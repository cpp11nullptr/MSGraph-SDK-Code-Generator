// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A collection request interface entity.
    /// </summary>
    public sealed class CollectionRequestInterfaceEntity : BaseCollectionRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="CollectionRequestInterfaceEntity"/> class
        /// based on passed ODCM property.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property.</param>
        public CollectionRequestInterfaceEntity(OdcmProperty odcmProperty)
            : base(odcmProperty, isAbstract: true)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        protected override string GetFullEntityNameSuffix() => "Request";

        /// <inheritdoc/>
        protected override string GetBasePrimaryEntityName() => string.Empty;

        /// <inheritdoc/>
        protected override string GetBaseInterfaceEntityName() => "IBaseRequest";

        /// <inheritdoc/>
        protected override string GetEntityHeaderComment() => $"An interface of a request for {GetEntityName()} collection for {GetSuperClassEntityName()} entity";
    }
}
