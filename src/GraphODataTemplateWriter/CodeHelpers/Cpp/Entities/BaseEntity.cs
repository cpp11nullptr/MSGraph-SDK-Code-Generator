// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using System;
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A base class for C++ entity.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Generates include statements for the entity.
        /// </summary>
        /// <returns>The string contains include statements.</returns>
        public abstract string GenerateIncludeStatements();

        /// <summary>
        /// Generates entity header contains entity name and comment block.
        /// </summary>
        /// <returns>The string contains entity header.</returns>
        public abstract string GenerateEntityHeader();

        /// <summary>
        /// Instantiates a new instance of <see cref="BaseEntity"/> class
        /// based on passed ODCM class.
        /// </summary>
        public BaseEntity(OdcmClass odcmClass)
        {
            _odcmClass = odcmClass;
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="BaseEntity"/> class
        /// based on passed ODCM enumeration.
        /// </summary>
        public BaseEntity(OdcmEnum odcmEnum)
        {
            _odcmEnum = odcmEnum;
        }

        /// <summary>
        /// Generates namespace name where the entity belongs to.
        /// </summary>
        /// <returns>The generated namespace name.</returns>
        public string GenerateNamespaceName()
        {
            return GetNamespaceName();
        }

        /// <summary>
        /// Constructs namespace name where the entity is located.
        /// </summary>
        /// <returns>The constructed namespace name.</returns>
        protected string GetNamespaceName()
        {
            string namespaceName = GetOdcmType().Namespace.Name;

            return $"namespace {Inflector.Inflector.Titleize(namespaceName).Replace(".", "::")}";
        }

        /// <summary>
        /// Whether the entity has base entity.
        /// </summary>
        /// <returns>True if the entity has base entity or False in other case.</returns>
        protected bool HasBaseEntity()
        {
            return GetOdcmTypeAsClass().Base != null;
        }

        /// <summary>
        /// Contstructs an entity name.
        /// </summary>
        /// <returns>The constructed entity name.</returns>
        protected string GetEntityName()
        {
            string odcmTypeName = GetOdcmType().Name;

            return NameConverter.CapitalizeName(odcmTypeName);
        }

        /// <summary>
        /// Contstructs an base entity name.
        /// </summary>
        /// <returns>The constructed base entity name.</returns>
        protected string GetBaseEntityName()
        {
            if (!HasBaseEntity())
            {
                return string.Empty;
            }

            string odcmBaseEntityName = GetOdcmTypeAsClass().Base.Name;

            return NameConverter.CapitalizeName(odcmBaseEntityName);
        }

        /// <summary>
        /// Gets ODCM class which the entity based on.
        /// </summary>
        /// <remarks>
        /// If entity is not based on ODCM class the exception will be thrown.
        /// </remarks>
        /// <returns>The ODCM class.</returns>
        protected OdcmClass GetOdcmTypeAsClass()
        {
            if (_odcmClass == null)
            {
                throw new Exception("An ODCM type is not class type.");
            }

            return _odcmClass;
        }

        /// <summary>
        /// Gets ODCM enumeration which the entity based on.
        /// </summary>
        /// <remarks>
        /// If entity is not based on ODCM enumeration the exception will be thrown.
        /// </remarks>
        /// <returns>The ODCM enumeration.</returns>
        protected OdcmEnum GetOdcmTypeAsEnum()
        {
            if (_odcmEnum == null)
            {
                throw new Exception("An ODCM type is not enum type.");
            }

            return _odcmEnum;
        }

        /// <summary>
        /// Gets type of ODCM object which the entity is based on.
        /// </summary>
        /// <returns>The ODCM type.</returns>
        private OdcmType GetOdcmType()
        {
            if (_odcmClass != null)
            {
                return _odcmClass;
            }
            else if (_odcmEnum != null)
            {
                return _odcmEnum;
            }

            throw new Exception("An ODCM type is undefined.");
        }

        /// <summary>
        /// A ODCM class instance which the entity is based on.
        /// </summary>
        private readonly OdcmClass _odcmClass;

        /// <summary>
        /// A ODCM enumeration instance which the entity is based on.
        /// </summary>
        private readonly OdcmEnum _odcmEnum;
    }
}
