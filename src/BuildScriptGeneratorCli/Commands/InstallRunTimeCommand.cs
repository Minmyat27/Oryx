﻿// --------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Oryx.BuildScriptGenerator;
using Microsoft.Oryx.BuildScriptGenerator.Common;

namespace Microsoft.Oryx.BuildScriptGeneratorCli
{
    [Command(Name, Description = "Install the required runtime components for a platform.")]
    internal class InstallRunTimeCommand : CommandBase
    {
        public const string Name = "install-runtime";

        [Option(
            OptionTemplates.Platform,
            CommandOptionType.SingleValue,
            Description = "The name of the platform for which the runtime components should be installed.")]
        public string Platform { get; set; }

        [Option(
            OptionTemplates.PlatformVersion,
            CommandOptionType.SingleValue,
            Description = "The version of the platform for which the runtime components should be installed.")]
        public string PlatformVersion { get; set; }

        internal override int Execute(IServiceProvider serviceProvider, IConsole console)
        {
            var scriptGenerator = serviceProvider.GetRequiredService<IRunTimeInstallationScriptGenerator>();

            var options = new RunTimeInstallationScriptGeneratorOptions { PlatformVersion = PlatformVersion };

            var script = scriptGenerator.GenerateBashScript(Platform, options);
            if (string.IsNullOrEmpty(script))
            {
                console.WriteErrorLine("Couldn't generate startup script.");
                return ProcessConstants.ExitFailure;
            }

            console.WriteLine(script);

            return ProcessConstants.ExitSuccess;
        }

        internal override bool IsValidInput(IServiceProvider serviceProvider, IConsole console)
        {
            // TODO: validate?
            return true;
        }
    }
}
