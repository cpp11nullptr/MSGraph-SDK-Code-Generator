// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers
{
    /// <summary>
    /// Defines C++ includes.
    /// </summary>
    internal static class IncludeFile
    {
        public const string CppRestSdkHttpClient = "cpprest/http_client.h";

        public const string CppRestSdkHttpMsg = "cpprest/http_msg.h";

        public const string CppRestSdkJson = "cpprest/json.h";

        public const string GraphSdkBaseClientInterface = "IBaseClient.h";

        public const string GraphSdkBaseRequest = "BaseRequest.h";

        public const string GraphSdkBaseRequestBuilder = "BaseRequestBuilder.h";

        public const string GraphSdkStringUtils = "StringUtils.h";

        public const string PplxCancellationToken = "pplx/pplxcancellation_token.h";

        public const string StdIntegral = "cstdint";

        public const string StdAny = "any";

        public const string StdFuture = "future";

        public const string StdMap = "map";

        public const string StdMemory = "memory";

        public const string StdSstream = "sstream";

        public const string StdString = "string";

        public const string StdVector = "vector";
    }
}
