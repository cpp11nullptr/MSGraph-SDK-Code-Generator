// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp
{
    using System.Collections.Generic;
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A converter creates a specialized entity on top of ODCM entity.
    /// </summary>
    public static class EntityConverter
    {
        /// <summary>
        /// Creates a model enum entity.
        /// </summary>
        /// <param name="odcmEnum">The ODCM enum instance.</param>
        /// <returns>The model enum entity instance.</returns>
        public static ModelEnumEntity ToModelEnumEntity(this OdcmEnum odcmEnum) =>
            new ModelEnumEntity(odcmEnum);

        /// <summary>
        /// Creates a model type entity.
        /// </summary>
        /// <param name="odcmClass">The ODCM type instance.</param>
        /// <returns>The model type entity instance.</returns>
        public static ModelTypeEntity ToModelTypeEntity(this OdcmClass odcmClass) =>
            new ModelTypeEntity(odcmClass);

        /// <summary>
        /// Creates a collection page entity.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property instance.</param>
        /// <returns>The collection page entity instance.</returns> => 
        public static CollectionPageEntity ToCollectionPageEntity(this OdcmProperty odcmProperty) =>
            new CollectionPageEntity(odcmProperty);

        /// <summary>
        /// Creates a collection page interface entity.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property instance.</param>
        /// <returns>The collection page interface entity instance.</returns> => 
        public static CollectionPageInterfaceEntity ToCollectionPageInterfaceEntity(this OdcmProperty odcmProperty) =>
            new CollectionPageInterfaceEntity(odcmProperty);

        /// <summary>
        /// Creates a collection request builder entity.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property instance.</param>
        /// <returns>The collection request builder entity instance.</returns>
        public static CollectionRequestBuilderEntity ToCollectionRequestBuilderEntity(this OdcmProperty odcmProperty) =>
            new CollectionRequestBuilderEntity(odcmProperty);

        /// <summary>
        /// Creates a collection request builder interface entity.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property instance.</param>
        /// <returns>The collection request builder interface entity instance.</returns>
        public static CollectionRequestBuilderInterfaceEntity ToCollectionRequestBuilderInterfaceEntity(this OdcmProperty odcmProperty) =>
            new CollectionRequestBuilderInterfaceEntity(odcmProperty);

        /// <summary>
        /// Creates a collection request entity.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property instance.</param>
        /// <returns>The collection request entity instance.</returns>
        public static CollectionRequestEntity ToCollectionRequestEntity(this OdcmProperty odcmProperty) =>
           new CollectionRequestEntity(odcmProperty);

        /// <summary>
        /// Creates a collection request interface entity.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property instance.</param>
        /// <returns>The collection request interface entity instance.</returns>
        public static CollectionRequestInterfaceEntity ToCollectionRequestInterfaceEntity(this OdcmProperty odcmProperty) =>
           new CollectionRequestInterfaceEntity(odcmProperty);

        // <summary>
        /// Creates a collection response entity.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property instance.</param>
        /// <returns>The collection response entity instance.</returns>
        public static CollectionResponseEntity ToCollectionResponseEntity(this OdcmProperty odcmProperty) =>
            new CollectionResponseEntity(odcmProperty);

        /// <summary>
        /// Creates a graph client entity.
        /// </summary>
        /// <param name="odcmClass">The ODCM class instance.</param>
        /// <param name="odcmLinkedEntities">A list of linked entities.</param>
        /// <returns>The graph client entity instance.</returns>
        public static GraphClientEntity ToGraphClientEntity(this OdcmClass odcmClass, IEnumerable<OdcmProperty> odcmLinkedEntities) =>
            new GraphClientEntity(odcmClass, odcmLinkedEntities);

        /// <summary>
        /// Creates a request builder entity.
        /// </summary>
        /// <param name="odcmClass">The ODCM class instance.</param>
        /// <returns>The request builder entity instance.</returns>
        public static RequestBuilderEntity ToRequestBuilderEntity(this OdcmClass odcmClass) =>
            new RequestBuilderEntity(odcmClass);

        /// <summary>
        /// Creates a request builder interface entity.
        /// </summary>
        /// <param name="odcmClass">The ODCM class instance.</param>
        /// <returns>The request builder interface entity instance.</returns>
        public static RequestBuilderInterfaceEntity ToRequestBuilderInterfaceEntity(this OdcmClass odcmClass) =>
            new RequestBuilderInterfaceEntity(odcmClass);


        /// <summary>
        /// Creates a request entity.
        /// </summary>
        /// <param name="odcmClass">The ODCM class instance.</param>
        /// <returns>The request entity instance.</returns>
        public static RequestEntity ToRequestEntity(this OdcmClass odcmClass) =>
            new RequestEntity(odcmClass);

        /// <summary>
        /// Creates a request interface entity.
        /// </summary>
        /// <param name="odcmClass">The ODCM class instance.</param>
        /// <returns>The request interface entity instance.</returns>
        public static RequestInterfaceEntity ToRequestInterfaceEntity(this OdcmClass odcmClass) =>
            new RequestInterfaceEntity(odcmClass);
    }
}
