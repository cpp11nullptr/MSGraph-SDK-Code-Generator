// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Microsoft.Graph.ODataTemplateWriter.Extensions;
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
        /// <param name="odcmTypeName">The name of ODCM type to be used.</param>
        /// <returns>The request builder entity name.</returns>
        protected string GetRequestBuilderEntityName(string odcmTypeName = null)
        {
            string entityName = GetEntityName(odcmTypeName);

            return $"{entityName}RequestBuilder";
        }

        /// <summary>
        /// Constructs the request builder interface entity name.
        /// </summary>
        /// <param name="odcmTypeName">The name of ODCM type to be used.</param>
        /// <returns>The request builder interface entity name.</returns>
        protected string GetRequestBuilderInterfaceEntityName(string odcmTypeName = null)
        {
            string requestBuilderEntityName = GetRequestBuilderEntityName(odcmTypeName);

            return $"I{requestBuilderEntityName}";
        }

        /// <summary>
        /// Generates include statements needed for navigation request builders.
        /// </summary>
        /// <param name="isInterface">
        /// True if include statements should be generated for interfaces or False if
        /// include statements should generated for implemented entities.
        /// </param>
        /// <returns>The list contains include statements.</returns>
        protected IEnumerable<string> GenerateNavigationRequestBuilderIncludeStatements(bool isInterface)
        {
            OdcmClass odcmClass = GetOdcmTypeAsClass();
            IEnumerable<OdcmProperty> navigationProperties = odcmClass.NavigationProperties();
            int navigationPropertiesCount = navigationProperties.Count();

            if (navigationPropertiesCount == 0)
            {
                return Enumerable.Empty<string>();
            }

            IList<string> includeStatements = new List<string>(navigationProperties.Count());

            foreach (OdcmProperty navigationProperty in navigationProperties)
            {
                string navigationEntityBaseName = NameConverter.CapitalizeName(navigationProperty.Name);

                string navigationEntityName = navigationProperty.IsCollection ?
                    $"{GetEntityName()}{navigationEntityBaseName}Collection" :
                    navigationProperty.Name;

                string navigationRequestBuilderEntityName =
                    isInterface ?
                        GetRequestBuilderInterfaceEntityName(navigationEntityName) :
                        GetRequestBuilderEntityName(navigationEntityName);

                includeStatements.Add($"{navigationRequestBuilderEntityName}.h");
            }

            return includeStatements.OrderBy(statement => statement);
        }
    }
}
