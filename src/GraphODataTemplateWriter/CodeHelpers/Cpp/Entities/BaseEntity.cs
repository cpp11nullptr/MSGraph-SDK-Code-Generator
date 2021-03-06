﻿// Copyright (c) Microsoft Corporation.
// All rights reserved.
// Licensed under the MIT license.
// See license in the project root for license information.

namespace Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Graph.ODataTemplateWriter.CodeHelpers.Cpp.Helpers;
    using Vipr.Core.CodeModel;

    /// <summary>
    /// A base entity.
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
        /// <param name="odcmClass">The ODCM class instance.</param>
        public BaseEntity(OdcmClass odcmClass)
        {
            _odcmClass = odcmClass;
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="BaseEntity"/> class
        /// based on passed ODCM enumeration.
        /// </summary>
        /// <param name="odcmEnum">The ODCM enumeration instance.</param>
        public BaseEntity(OdcmEnum odcmEnum)
        {
            _odcmEnum = odcmEnum;
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="BaseEntity"/> class
        /// based on passed ODCM property.
        /// </summary>
        /// <param name="odcmProperty">The ODCM property instance.</param>
        public BaseEntity(OdcmProperty odcmProperty)
        {
            _odcmProperty = odcmProperty;
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
        /// Constructs full name of entity.
        /// </summary>
        /// <remarks>
        /// For compound entities it can be different from the entity name, e.g. the full name for
        /// ODCM property will be constructed based on its underlying ODCM class name.
        /// </remarks>
        /// <returns>The constructed full name of entity.</returns>
        protected virtual string GetFullEntityName()
        {
            return GetEntityName();
        }

        /// <summary>
        /// Constructs a base primary entity name.
        /// </summary>
        /// <remarks>
        /// It is marked as a virtual function for cases when a base entity does not match
        /// defined in ODCM structure.
        /// </remarks>
        /// <returns>The constructed base primary entity name.</returns>
        protected virtual string GetBasePrimaryEntityName()
        {
            if (!HasBaseEntity())
            {
                return string.Empty;
            }

            string odcmBaseEntityName = GetOdcmTypeAsClass().Base.Name;

            return NameConverter.CapitalizeName(odcmBaseEntityName);
        }

        /// <summary>
        /// Constructs a base interface entity name.
        /// </summary>
        /// <returns>The constructed base interface entity name.</returns>
        protected virtual string GetBaseInterfaceEntityName()
        {
            string fullEntityName = GetFullEntityName();

            return $"I{fullEntityName}";
        }

        /// <summary>
        /// Gets a list of base entity names.
        /// </summary>
        /// <returns>The list contains base entity names.</returns>
        protected IEnumerable<string> GetBaseEntityNames()
        {
            string basePrimaryEntityName = GetBasePrimaryEntityName();
            string baseInterfaceEntityName = GetBaseInterfaceEntityName();

            if (string.IsNullOrWhiteSpace(baseInterfaceEntityName) &&
                string.IsNullOrWhiteSpace(baseInterfaceEntityName))
            {
                return Enumerable.Empty<string>();
            }
            else if (string.IsNullOrWhiteSpace(baseInterfaceEntityName))
            {
                return new[] { basePrimaryEntityName };
            }
            else if (string.IsNullOrWhiteSpace(basePrimaryEntityName))
            {
                return new[] { baseInterfaceEntityName };
            }

            return new[] { baseInterfaceEntityName, basePrimaryEntityName };
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
        /// Constructs an entity name.
        /// </summary>
        /// <param name="odcmTypeName">The name of ODCM type to be used.</param>
        /// <returns>The constructed entity name.</returns>
        protected string GetEntityName(string odcmTypeName = null)
        {
            string odcmTypeNameToUse = odcmTypeName != null ? odcmTypeName : GetOdcmType().Name;

            return NameConverter.CapitalizeName(odcmTypeNameToUse);
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
                throw new Exception("An ODCM type is not enumeration type.");
            }

            return _odcmEnum;
        }

        /// <summary>
        /// Gets ODCM property which the entity based on.
        /// </summary>
        /// <remarks>
        /// If entity is not based on ODCM enumeration the exception will be thrown.
        /// </remarks>
        /// <returns>The ODCM property.</returns>
        protected OdcmProperty GetOdcmTypeAsProperty()
        {
            if (_odcmProperty == null)
            {
                throw new Exception("An ODCM type is not property type.");
            }

            return _odcmProperty;
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
            else if (_odcmProperty != null)
            {
                return _odcmProperty.Type;
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

        /// <summary>
        /// A ODCM property instance which the entity is based on.
        /// </summary>
        private readonly OdcmProperty _odcmProperty;
    }
}
