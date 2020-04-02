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
    /// A request builder interface.
    /// </summary>
    public sealed class RequestBuilderInterfaceEntity : BaseRequestEntity
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="RequestBuilderInterfaceEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        /// <param name="odcmClass">The ODCM class.</param>
        public RequestBuilderInterfaceEntity(OdcmClass odcmClass)
            : base(odcmClass)
        {
        }

        /// <inheritdoc/>
        public override string GenerateIncludeStatements()
        {
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();

            IncludeBlock includesBlock = new IncludeBlock();

            includesBlock.AppendFile(IncludeFile.StdMemory, isSystem: true);
            includesBlock.AppendLine();

            includesBlock.AppendFile($"{requestInterfaceEntityName}.h", isSystem: false);
            includesBlock.AppendLine();

            IEnumerable<string> navigationRequestBuilderInterfaceIncludeStatements =
                GenerateNavigationRequestBuilderIncludeStatements(isInterface: true);

            includesBlock.AppendFiles(navigationRequestBuilderInterfaceIncludeStatements, isSystem: false);
            includesBlock.AppendLine();

            return includesBlock.ToString();
        }

        /// <inheritdoc/>
        public override string GenerateEntityHeader()
        {
            string entityName = GetEntityName();
            string requestBuilderInterfaceEntityName = GetRequestBuilderInterfaceEntityName();

            using (CodeBlock codeBlock = new CodeBlock(1))
            {
                codeBlock.AppendLine($"/*");
                codeBlock.AppendLine($" * An interface of a builder to create a request for {entityName} entity.");
                codeBlock.AppendLine($" */");
                codeBlock.AppendLine($"struct {requestBuilderInterfaceEntityName}", newLine: false);

                return codeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the request builder interface destructor declaration.
        /// </summary>
        /// <returns>The string contains destructor declaration.</returns>
        public string GenerateDestructor()
        {
            string requestBuilderInterfaceEntityName = GetRequestBuilderInterfaceEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual ~{requestBuilderInterfaceEntityName}() noexcept = default;");

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates the create request method declaration.
        /// </summary>
        /// <returns>The string contains method declaration.</returns>
        public string GenerateRequestMethodPrototype()
        {
            string requestInterfaceEntityName = GetRequestInterfaceEntityName();

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual std::unique_ptr<{requestInterfaceEntityName}> Request() noexcept = 0;");

                return methodCodeBlock.ToString();
            }
        }

        /// <summary>
        /// Generates request builder method declarations to navigate into linked entities.
        /// </summary>
        /// <returns>The string contains request builder methods declarations.</returns>
        public string GenerateNavigationRequestBuilderMethodPrototypes()
        {
            OdcmClass odcmClass = GetOdcmTypeAsClass();
            IEnumerable<OdcmProperty> navigationProperties = odcmClass.NavigationProperties();
            int navigationPropertiesCount = navigationProperties.Count();

            if (navigationPropertiesCount == 0)
            {
                return string.Empty;
            }

            IList<string> requestBuilderMethods = new List<string>(navigationPropertiesCount);

            foreach (OdcmProperty navigationProperty in navigationProperties)
            {
                string requestBuilderMethod = GenerateNavigationRequestBuilderMethodPrototype(navigationProperty);

                requestBuilderMethods.Add(requestBuilderMethod);
            }

            string requestBuilderMethodDefinitions = string.Join("\n", requestBuilderMethods);

            return requestBuilderMethodDefinitions;
        }

        /// <summary>
        /// Generates the request builder method declaration to navigate into linked entity.
        /// </summary>
        /// <param name="navigationProperty">The ODCM property contains navigation details.</param>
        /// <returns>The string contains request builder method declaration.</returns>
        private string GenerateNavigationRequestBuilderMethodPrototype(OdcmProperty navigationProperty)
        {
            string navigationEntityBaseName = NameConverter.CapitalizeName(navigationProperty.Name);

            string navigationEntityName = navigationProperty.IsCollection ?
                $"{GetEntityName()}{navigationEntityBaseName}Collection" :
                navigationProperty.Name;

            string navigationRequestBuilderInterfaceEntityName =
                GetRequestBuilderInterfaceEntityName(navigationEntityName);

            using (CodeBlock methodCodeBlock = new CodeBlock(2))
            {
                methodCodeBlock.AppendLine($"virtual std::unique_ptr<{navigationRequestBuilderInterfaceEntityName}> {navigationEntityBaseName}() noexcept = 0;");

                return methodCodeBlock.ToString();
            }
        }
    }
}
