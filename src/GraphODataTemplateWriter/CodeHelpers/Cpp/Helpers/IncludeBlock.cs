// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A helper generates a block with include statements.
    /// </summary>
    public sealed class IncludeBlock
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="IncludeBlock"/> class.
        /// </summary>
        public IncludeBlock()
        {
            _builder = new StringBuilder();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _builder.ToString();
        }

        /// <summary>
        /// Adds an include statement to the block.
        /// </summary>
        /// <param name="includeFile">The include file.</param>
        /// <param name="isSystem">Whether it is a system defined file.</param>
        /// <returns>The reference to this instance after the append operation has completed.</returns>
        public IncludeBlock AppendFile(string includeFile, bool isSystem)
        {
            string includeStatement = string.Format(isSystem ? SystemIncludeFormat : UserDefinedIncludeFormat, includeFile);

            _builder.AppendLine(includeStatement);

            return this;
        }

        /// <summary>
        /// Adds include statements to the block.
        /// </summary>
        /// <param name="includeFile">The include files.</param>
        /// <param name="isSystem">Whether it is a system defined file.</param>
        /// <returns>The reference to this instance after the append operation has completed.</returns>
        public IncludeBlock AppendFiles(IEnumerable<string> includeFiles, bool isSystem)
        {
            foreach (string includeFile in includeFiles)
            {
                AppendFile(includeFile, isSystem);
            }

            return this;
        }

        /// <summary>
        /// Adds include statements to the block based on passed list of types.
        /// </summary>
        /// <param name="includeFile">The list of types to be added as include statements.</param>
        /// <param name="isSystem">Whether include statements should be filtered only by system types or user defined otherwise.</param>
        /// <returns>The reference to this instance after the append operation has completed.</returns>
        public IncludeBlock AppendFilesFromTypes(IEnumerable<string> types, bool includeSystemOnly)
        {
            HashSet<string> includeFiles = new HashSet<string>();

            foreach (string typeName in types)
            {
                string includeFile;

                bool isSystemType = _systemTypeToIncludeFile.TryGetValue(typeName, out includeFile);

                if (includeSystemOnly && !string.IsNullOrWhiteSpace(includeFile))
                {
                    includeFiles.Add(includeFile);
                }
                else if (!includeSystemOnly && !isSystemType)
                {
                    includeFile = string.Format(UserDefinedHeaderFileFormat, typeName);

                    includeFiles.Add(includeFile);
                }
            }

            IEnumerable<string> sortedIncludeFiles = includeFiles.OrderBy(value => value);

            foreach (string includeFile in sortedIncludeFiles)
            {
                AppendFile(includeFile, includeSystemOnly);
            }

            return this;
        }

        /// <summary>
        /// Adds a new line to the block.
        /// </summary>
        /// <returns>The reference to this instance after the append operation has completed.</returns>
        public IncludeBlock AppendLine()
        {
            _builder.AppendLine();

            return this;
        }

        /// <summary>
        /// Defines an include statement for system include.
        /// </summary>
        private const string SystemIncludeFormat = "#include <{0}>";

        /// <summary>
        /// Defines an include statement for user defined include.
        /// </summary>
        private const string UserDefinedIncludeFormat = "#include \"{0}\"";

        /// <summary>
        /// Defines a header file for a user defined type.
        /// </summary>
        private const string UserDefinedHeaderFileFormat = "{0}.h";

        /// <summary>
        /// A builder contructs the include statements block.
        /// </summary>
        private StringBuilder _builder { get; }

        /// <summary>
        /// Defines mapping system type to include file.
        /// </summary>
        private static readonly IReadOnlyDictionary<string, string> _systemTypeToIncludeFile =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { BasicType.Boolean, string.Empty },
                { BasicType.SignedChar, string.Empty },
                { BasicType.Float, string.Empty },
                { BasicType.Double, string.Empty },
                { BasicType.SignedInt8, IncludeFile.StdIntegral },
                { BasicType.SignedInt16, IncludeFile.StdIntegral },
                { BasicType.SignedInt32, IncludeFile.StdIntegral },
                { BasicType.SignedInt64, IncludeFile.StdIntegral },
                { BasicType.UnsignedInt8, IncludeFile.StdIntegral },
                { BasicType.UnsignedInt16, IncludeFile.StdIntegral },
                { BasicType.UnsignedInt32, IncludeFile.StdIntegral },
                { BasicType.UnsignedInt64, IncludeFile.StdIntegral },
                { BasicType.Any, IncludeFile.StdAny },
                { BasicType.String, IncludeFile.StdString },
                { BasicType.WideString, IncludeFile.StdString },
                { BasicType.Vector, IncludeFile.StdVector },
                { BasicType.Map, IncludeFile.StdMap },
                { BasicType.Dictionary, IncludeFile.StdMap }
            };
    }
}
