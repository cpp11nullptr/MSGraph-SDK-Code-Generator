// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.PathWriters
{
    using System.IO;
    using Microsoft.Graph.ODataTemplateWriter.TemplateProcessor;

    /// <summary>
    /// A path writer needed to resolve paths for C++ entities.
    /// </summary>
    public class CppPathWriter : PathWriterBase
    {
        /// <summary>
        /// Resolves a path for C++ entity.
        /// </summary>
        /// <param name="template">The template details.</param>
        /// <param name="entityTypeName">The entity type name.</param>
        /// <returns>The resolved path.</returns>
        public override string WritePath(ITemplateInfo template, string entityTypeName)
        {
            string coreFileName = TransformFileName(template, entityTypeName);

            return Path.Combine(template.OutputParentDirectory, coreFileName);
        }
    }
}
