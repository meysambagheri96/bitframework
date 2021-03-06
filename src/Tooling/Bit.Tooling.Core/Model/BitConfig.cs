﻿using Microsoft.CodeAnalysis;
using System;
using System.IO;

namespace Bit.Tooling.Core.Model
{
    public class BitConfig
    {
        public virtual string TargetFramework { get; set; } = "netcoreapp3.1";

        public virtual BitCodeGeneratorConfig BitCodeGeneratorConfigs { get; set; } = default!;
    }

    public class BitCodeGeneratorConfig
    {
        public virtual BitCodeGeneratorMapping[] BitCodeGeneratorMappings { get; set; } = default!;
    }

    public enum GenerationType
    {
        CSharp, TypeScript
    }

    public class BitCodeGeneratorMapping
    {
        public virtual string Route { get; set; } = default!;

        [Obsolete("Bit 1.3.80 and above are using default odata namespace. There is no need to customize this anymore.")]
        public virtual string Namespace { get; set; } = default!;

        public virtual ProjectInfo[] SourceProjects { get; set; } = default!;

        public virtual ProjectInfo DestinationProject { get; set; } = default!;

        public virtual NamespaceAlias[] NamespaceAliases { get; set; } = Array.Empty<NamespaceAlias>();

        public virtual string DestinationFileName { get; set; } = default!;

        public virtual string TypingsPath { get; set; } = default!;

        public virtual GenerationType GenerationType => string.IsNullOrEmpty(TypingsPath) ? GenerationType.CSharp : GenerationType.TypeScript;
    }

    public class ProjectInfo
    {
        public virtual string Name { get; set; } = default!;

        public bool IsThisProject(Project p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            return p.Name == Name || Path.GetFileNameWithoutExtension(p.FilePath) == Name;
        }
    }

    public class NamespaceAlias
    {
        public virtual string Namespace { get; set; } = default!;

        public virtual string Alias { get; set; } = default!;
    }
}
