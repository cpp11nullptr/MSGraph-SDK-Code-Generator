// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers
{
    using System;
    using System.Text;

    /// <summary>
    /// A helper generates a code block.
    /// </summary>
    internal sealed class CodeBlock : IDisposable
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="CodeBlock"/> class.
        /// </summary>
        /// <param name="tabsCount">The amount of tabs to be placed before each new line.</param>
        public CodeBlock(int tabsCount)
        {
            _builder = new StringBuilder();

            _shouldBeClosingBracketAdded = false;
            _tabsCount = tabsCount;
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="CodeBlock"/> class.
        /// </summary>
        /// <param name="block">The base code block to be linked to with adding a new tab to parent counter.</param>
        public CodeBlock(CodeBlock codeBlock) : this(codeBlock._tabsCount)
        {
            _builder = codeBlock._builder;

            AppendLine("{");

            _shouldBeClosingBracketAdded = true;
            _tabsCount = codeBlock._tabsCount + 1;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_shouldBeClosingBracketAdded)
            {
                _tabsCount -= 1;

                AppendLine("}");
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _builder.ToString();
        }

        /// <summary>
        /// Appends the default line terminator to the end of the current <see cref="CodeBlock"/> object.
        /// </summary>
        /// <returns>The reference to this instance after the append operation has completed.</returns>
        public CodeBlock AppendLine()
        {
            _builder.AppendLine();

            return this;
        }

        /// <summary>
        /// Appends a copy of the specified string prefixed with certain amount of tabs.
        /// </summary>
        /// <param name="str">The string to be append.</param>
        /// <param name="newLine">Whether the string should be ended with a new line terminator.</param>
        /// <returns>The reference to this instance after the append operation has completed.</returns>
        public CodeBlock AppendLine(string str, bool newLine = true)
        {
            AppendPrefix();

            if (newLine)
            {
                _builder.AppendLine(str);
            }
            else
            {
                _builder.Append(str);
            }

            return this;
        }

        /// <summary>
        /// Appends a tab and a copy of the specified string prefixed with certain amount of tabs.
        /// </summary>
        /// <param name="str">The string to be append.</param>
        /// <param name="newLine">Whether the string should be ended with a new line terminator.</param>
        /// <returns>The reference to this instance after the append operation has completed.</returns>
        public CodeBlock AppendLineShifted(string str, bool newLine = true)
        {
            AppendTab();

            return AppendLine(str, newLine);
        }

        /// <summary>
        /// Appends the default tabular prefix to the beginning of the current <see cref="CodeBlock"/> object.
        /// </summary>
        /// <returns>The reference to this instance after the append operation has completed.</returns>
        public CodeBlock AppendPrefix()
        {
            _builder.Append(TabDelimiter, _tabsCount);

            return this;
        }


        /// <summary>
        /// Appends a one tab to the current <see cref="CodeBlock"/> object.
        /// </summary>
        /// <returns>The reference to this instance after the append operation has completed.</returns>
        public CodeBlock AppendTab()
        {
            _builder.Append(TabDelimiter);

            return this;
        }

        /// <summary>
        /// Appends a space tab to the current <see cref="CodeBlock"/> object.
        /// </summary>
        /// <returns>The reference to this instance after the append operation has completed.</returns>
        public CodeBlock AppendSpace()
        {
            _builder.Append(SpaceDelimiter);

            return this;
        }

        /// <summary>
        /// Defines tab delimiter.
        /// </summary>
        public const char TabDelimiter = '\t';

        /// <summary>
        /// Defines space delimiter.
        /// </summary>
        public const char SpaceDelimiter = ' ';

        /// <summary>
        /// A builder contructs the code block.
        /// </summary>
        private StringBuilder _builder { get; }

        /// <summary>
        /// An amount of tabs to be placed before each new line.
        /// </summary>
        private int _tabsCount { get; set; }

        /// <summary>
        /// Whether a closing bracket should be added during object disposing.
        /// </summary>
        private bool _shouldBeClosingBracketAdded { get; }
    }
}
