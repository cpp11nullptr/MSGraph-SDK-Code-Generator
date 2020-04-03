// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A base collection request entity.
    /// </summary>
    public abstract class BaseCollectionRequestEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="BaseCollectionRequestEntity"/> class
        /// based on passed ODCM property.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property.</param>
        /// <param name="isAbstract">Whether the entity is an abstract type.</param>
        public BaseCollectionRequestEntity(OdcmProperty odcmProperty, bool isAbstract)
            : base(odcmProperty, isAbstract)
        {
        }

        /// <summary>
        /// Constructs a suffix used for full entity name.
        /// </summary>
        /// <returns>The string contains the suffix.</returns>
        protected abstract string GetFullEntityNameSuffix();

        /// <inheritdoc/>
        protected override string GetFullEntityName()
        {
            OdcmProperty odcmProperty = GetOdcmTypeAsProperty();

            string superClassEntityName = GetSuperClassEntityName();
            string collectionEntityName = NameConverter.CapitalizeName(odcmProperty.Name);
            string namePrefix = isAbstract ? "I" : string.Empty;
            string nameSuffix = GetFullEntityNameSuffix();

            return $"{namePrefix}{superClassEntityName}{collectionEntityName}Collection{nameSuffix}";
        }

        /// <summary>
        /// Constructs entity name of super class contains the collection.
        /// </summary>
        /// <returns>The constructed entity name.</returns>
        protected string GetSuperClassEntityName() => NameConverter.CapitalizeName(GetOdcmTypeAsProperty().Class.Name);
    }
}
