/*
 * Startup.cs
 *
 *   Created: 2022-12-03-02:37:05
 *   Modified: 2022-12-03-02:37:05
 *
 *   Author: Justin Chase <justin@justinwritescode.com>
 *
 *   Copyright © 2022-2023 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

 #pragma warning disable

using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Build.Construction;
using Microsoft.Build.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace JustinWritesCode.AzureFunctions;

public class Startup : FunctionsStartup
{
    protected virtual ProjectInstance Project => new ProjectInstance(ProjectRootElement.Create(new System.Xml.XmlTextReader(Assembly.GetEntryAssembly().GetManifestResourceStream("project.csproj"))));

    public override void Configure(IFunctionsHostBuilder builder)
    {
        FunctionsAssemblyResolver.RedirectAssembly();

        builder.Services.AddLogging();

        var title = ThisAssembly.Project.Title; //Project.GetPropertyValue("Title");
        var description = ThisAssembly.Project.Description;//Project.GetPropertyValue("Description");
        var version = ThisAssembly.Project.PackageVersion ?? ThisAssembly.Project.Version;//Project.GetPropertyValue("PackageVersion") ?? Project.GetPropertyValue("Version");
        var repositoryUrl = ThisAssembly.Project.RepositoryUrl;
        var license = ThisAssembly.Project.LicenseExpression;
        var tosUrl = ThisAssembly.Project.TermsOfServiceUrl;
        var authors = ThisAssembly.Project.Authors;
        var owners = ThisAssembly.Project.Owners;
        var contactName = !string.IsNullOrEmpty(ThisAssembly.Project.ContactName) ? ThisAssembly.Project.ContactName : !string.IsNullOrEmpty(owners) ? owners : authors;
        var contactEmail = ThisAssembly.Project.ContactEmail;

        builder.Services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
			new OpenApiConfigurationOptions()
			{
				Info = new OpenApiInfo()
				{
					Version = version,
					Title = title,
					Description = description,
					TermsOfService = !string.IsNullOrEmpty(tosUrl) ? new Uri(tosUrl) : null,
					Contact = new OpenApiContact()
					{
						Name = contactName,
						Email = contactEmail,
						Url = new (repositoryUrl),
					},
					License = new OpenApiLicense()
					{
						Name = license,
						Url = new ($"http://opensource.org/licenses/{license}"),
					}
                },
				Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
				OpenApiVersion = OpenApiVersionType.V2,
				IncludeRequestingHostName = true,
				ForceHttps = false,
				ForceHttp = false
            });
    }

    public class FunctionsAssemblyResolver
    {
        public static void RedirectAssembly()
        {
            var list = AppDomain.CurrentDomain.GetAssemblies().OrderByDescending(a => a.FullName).Select(a => a.FullName).ToList();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var requestedAssembly = new AssemblyName(args.Name);
            Assembly assembly = null;
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            try
            {
                assembly = Assembly.Load(requestedAssembly.Name);
            }
            catch (Exception ex)
            {
            }
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            return assembly;
        }
    }
}
