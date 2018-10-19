﻿using ApiDoctor.Publishing.CSDL;
using ApiDoctor.Validation;
using ApiDoctor.Validation.Error;
using ApiDoctor.Validation.OData;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typewriter
{
    internal class DocAnnotationWriter : ApiDoctor.Publishing.CSDL.CsdlWriter// or this a helper, will name this later
    {
        private static Logger Logger => LogManager.GetLogger("DocAnnotationWriter");
        private string csdl;

        internal IssueLogger IssueLogger { get; set; }
        internal DocSet DocSet { get; set; }

        private readonly CsdlWriterOptions options;

        internal DocAnnotationWriter(DocSet docSet, CsdlWriterOptions options, string csdl) : base(docSet, options)
        {
            this.csdl = csdl;
            this.DocSet = docSet;
            this.options = options; // Can change the base access modifier so we could use it. 
        }

        private async Task<string> PublishToStringAsync(IssueLogger issues)
        {
            // Step 1: Generate an EntityFramework OM from the documentation and/or template file
            EntityFramework framework = CreateEntityFrameworkFromDocs(issues);
            if (null == framework)
                return string.Empty;

            if (!string.IsNullOrEmpty(options.MergeWithMetadataPath))
            {
                EntityFramework secondFramework = CreateEntityFrameworkFromDocs(issues, options.MergeWithMetadataPath, generateFromDocs: false);
                framework = framework.MergeWith(secondFramework);
                outputFilenameSuffix += "-merged";
            }

            // Step 1a: Apply an transformations that may be defined in the documentation
            if (!string.IsNullOrEmpty(options.TransformOutput))
            {
                PublishSchemaChangesConfigFile transformations = DocSet.TryLoadConfigurationFiles<PublishSchemaChangesConfigFile>(options.DocumentationSetPath).Where(x => x.SchemaChanges.TransformationName == options.TransformOutput).FirstOrDefault();
                if (null == transformations)
                {
                    throw new KeyNotFoundException($"Unable to locate a transformation set named {options.TransformOutput}. Aborting.");
                }

                string[] versionsToPublish = options.Version?.Split(new char[] { ',', ' ' });
                framework.ApplyTransformation(transformations.SchemaChanges, versionsToPublish);
                if (!string.IsNullOrEmpty(options.Version))
                {
                    outputFilenameSuffix += $"-{options.Version}";
                }
            }

            if (options.Sort)
            {
                // Sorts the objects in collections, so that we have consistent output regardless of input
                framework.SortObjectGraph();
            }

            if (options.ValidateSchema)
            {
                framework.ValidateSchemaTypes();
            }

            // Step 2: Generate XML representation of EDMX
            string xmlData = null;
            if (options.Formats.HasFlag(MetadataFormat.EdmxOutput))
            {
                xmlData = ODataParser.Serialize<EntityFramework>(framework, options.AttributesOnNewLines);
            }
            else if (options.Formats.HasFlag(MetadataFormat.SchemaOutput))
            {
                xmlData = ODataParser.Serialize<Schema>(framework.DataServices.Schemas.First(), options.AttributesOnNewLines);
            }

            return xmlData;
        }
    }

    internal static class AnnotationHelper
    {
        private static Logger Logger => LogManager.GetLogger("AnnotationHelper");

        internal static string ApplyAnnotationsToCsdl(string csdl, Options options)
        {
            // Get DocSet
            DocSet docs = GetDocSet(options, new IssueLogger());
            // Create CsdlWriterOptions
            var csdlWriterOptions = new CsdlWriterOptions() { DocumentationSetPath = options.DocsRoot + "\\api-reference\\v1.0\\" };

            // Create DocAnnotationWriter
            DocAnnotationWriter docWriter = new DocAnnotationWriter(docs, csdlWriterOptions, csdl);

            throw new NotImplementedException();
        }

        private static DocSet GetDocSet(Options options, IssueLogger issues)
        {
            Logger.Info("Opening documentation from {0}", options.DocsRoot);
            DocSet docSet = null;

            try
            {
                docSet = new DocSet(options.DocsRoot + "\\api-reference\\v1.0\\");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Logger.Error(ex.Message);
                return null; // Hmmmmm. TODO
            }

            Logger.Info("Scanning documentation files");
            var stopwatch = new Stopwatch();
            docSet.ScanDocumentation(string.Empty, issues);
            stopwatch.Stop();
            Logger.Info($"Took {stopwatch.Elapsed} to parse {docSet.Files.Length} source files.");

            return docSet;
        }
    }
}
