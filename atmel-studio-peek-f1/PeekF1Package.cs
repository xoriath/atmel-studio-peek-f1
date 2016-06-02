﻿using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace Atmel.Studio.Help.Peek
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuidString)]
    public sealed class PeekF1Package : Package
    {
        public const string PackageGuidString = "1b797723-54ad-4029-ac39-fab88b204891";
        public const string PackageCmdSetString = "7593cd3c-ef01-4047-bdcf-b61fab9d1ad8";

        public static readonly Guid PackageCmdSetGuid = new Guid(PackageCmdSetString);

        public const uint PeekHelpCmdId = 0x100;
    }
}
