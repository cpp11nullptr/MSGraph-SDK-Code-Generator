// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp
{
    using System.Globalization;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A helper class to use in C++ entity generation.
    /// </summary>
    public class CodeWriterCpp : CodeWriterBase
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="CodeWriterCpp"/> class.
        /// </summary>
        public CodeWriterCpp() : base()
        {
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="CodeWriterCpp"/> class
        /// based on passed ODCM model.
        /// </summary>
        /// <param name="model">The ODCM model.</param>
        public CodeWriterCpp(OdcmModel model) : base(model)
        {
        }

        /// <inheritdoc/>
        public override string WriteOpeningCommentLine()
        {
            return $"/{_starSequence}{NewLineCharacter}*{NewLineCharacter}";
        }

        /// <inheritdoc/>
        public override string WriteClosingCommentLine()
        {
            return $"*{NewLineCharacter}{_starSequence}/";
        }

        /// <inheritdoc/>
        public override string WriteInlineCommentChar()
        {
            return "* ";
        }

        /// <summary>
        /// Defines a sequence of stars used in block comment.
        /// </summary>
        private static readonly string _starSequence = new string('*', 80);
    }
}
