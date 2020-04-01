// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A base class for request entity.
    /// </summary>
    public abstract class BaseRequestEntity : BaseEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="BaseRequestEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        /// <param name="odcmClass">The ODCM class.</param>
        public BaseRequestEntity(OdcmClass odcmClass)
            : base(odcmClass)
        {
        }

        /// <summary>
        /// Constructs the request entity name.
        /// </summary>
        /// <returns>The request entity name.</returns>
        protected string GetRequestEntityName()
        {
            string entityName = GetEntityName();

            return $"{entityName}Request";
        }

        /// <summary>
        /// Constructs the request interface entity name.
        /// </summary>
        /// <returns>The request interface entity name.</returns>
        protected string GetRequestInterfaceEntityName()
        {
            string requestEntityName = GetRequestEntityName();

            return $"I{requestEntityName}";
        }

        /// <summary>
        /// Constructs the request builder entity name.
        /// </summary>
        /// <returns>The request builder entity name.</returns>
        protected string GetRequestBuilderEntityName()
        {
            string entityName = GetEntityName();

            return $"{entityName}RequestBuilder";
        }

        /// <summary>
        /// Constructs the request builder interface entity name.
        /// </summary>
        /// <returns>The request builder interface entity name.</returns>
        protected string GetRequestBuilderInterfaceEntityName()
        {
            string requestBuilderEntityName = GetRequestBuilderEntityName();

            return $"I{requestBuilderEntityName}";
        }
    }
}
