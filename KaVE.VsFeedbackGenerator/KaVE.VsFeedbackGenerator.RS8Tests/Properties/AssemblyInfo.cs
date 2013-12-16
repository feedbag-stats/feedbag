using System.Collections.Generic;
using System.Reflection;
using JetBrains.Application;
using JetBrains.Threading;
using KaVE.VsFeedbackGenerator;
using NUnit.Framework;

/// <summary>
/// Test environment. Must be in the global namespace.
/// </summary>
[SetUpFixture]
// ReSharper disable once CheckNamespace
// ReSharper disable once InconsistentNaming
public class KaVE_VsFeedbackGenerator_Rs8TestsAssembly : ReSharperTestEnvironmentAssembly
{
    /// <summary>
    /// Gets the assemblies to load into test environment.
    /// Should include all assemblies which contain components.
    /// </summary>
    private static IEnumerable<Assembly> GetAssembliesToLoad()
    {
        // Test assembly
        yield return Assembly.GetExecutingAssembly();

        yield return typeof (AboutAction).Assembly;
    }

    public override void SetUp()
    {
        base.SetUp();
        ReentrancyGuard.Current.Execute(
            "LoadAssemblies",
            () => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
                GetType().Name,
                GetAssembliesToLoad()));
    }

    public override void TearDown()
    {
        ReentrancyGuard.Current.Execute(
            "UnloadAssemblies",
            () => Shell.Instance.GetComponent<AssemblyManager>().UnloadAssemblies(
                GetType().Name,
                GetAssembliesToLoad()));
        base.TearDown();
    }
}