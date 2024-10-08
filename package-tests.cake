// Tests run for all runner packages except NETCORE runner
var StandardRunnerTests = new List<PackageTest>();

// Tests run for the NETCORE runner package
var NetCoreRunnerTests = new List<PackageTest>();

// Method for adding to both lists
void AddToBothLists(PackageTest test)
{
    StandardRunnerTests.Add(test);
    NetCoreRunnerTests.Add(test);
}

//////////////////////////////////////////////////////////////////////
// RUN MOCK-ASSEMBLY UNDER EACH RUNTIME
//////////////////////////////////////////////////////////////////////

class MockAssemblyExpectedResult : ExpectedResult
{
    public MockAssemblyExpectedResult(params string[] runtimes) : base("Failed")
    {
        int nCopies = runtimes.Length;
        Total = 37 * nCopies;
        Passed = 23 * nCopies;
        Failed = 5 * nCopies;
        Warnings = 1 * nCopies;
        Inconclusive = 1 * nCopies;
        Skipped = 7 * nCopies;
        Assemblies = new ExpectedAssemblyResult[nCopies];
        for (int i = 0; i < nCopies; i++)
            Assemblies[i] = new ExpectedAssemblyResult("mock-assembly.dll", runtimes[i]);
    }
}

StandardRunnerTests.Add(new PackageTest(
    1, "Net462Test",
    "Run mock-assembly.dll under .NET 4.6.2",
    "net462/mock-assembly.dll",
    new MockAssemblyExpectedResult("net-4.6.2")));

AddToBothLists(new PackageTest(
    1, "Net80Test",
    "Run mock-assembly.dll under .NET 8.0",
    "net8.0/mock-assembly.dll",
    new MockAssemblyExpectedResult("netcore-8.0")));

AddToBothLists(new PackageTest(
    1, "Net70Test",
    "Run mock-assembly.dll under .NET 7.0",
    "net7.0/mock-assembly.dll",
    new MockAssemblyExpectedResult("netcore-7.0")));

AddToBothLists(new PackageTest(
    1, "Net60Test",
    "Run mock-assembly.dll under .NET 6.0",
    "net6.0/mock-assembly.dll",
    new MockAssemblyExpectedResult("netcore-6.0")));

AddToBothLists(new PackageTest(
    1, "NetCore31Test",
    "Run mock-assembly.dll under .NET Core 3.1",
    "netcoreapp3.1/mock-assembly.dll",
    new MockAssemblyExpectedResult("netcore-3.1")));

//////////////////////////////////////////////////////////////////////
// RUN MOCK-ASSEMBLY-X86 UNDER EACH RUNTIME
//////////////////////////////////////////////////////////////////////

const string DOTNET_EXE_X86 = @"C:\Program Files (x86)\dotnet\dotnet.exe";
// TODO: Remove the limitation to Windows
bool dotnetX86Available = IsRunningOnWindows() && System.IO.File.Exists(DOTNET_EXE_X86);

class MockAssemblyX86ExpectedResult : MockAssemblyExpectedResult
{
    public MockAssemblyX86ExpectedResult(params string[] runtimes) : base(runtimes)
    {
        for (int i = 0; i < runtimes.Length; i++)
            Assemblies[i] = new ExpectedAssemblyResult("mock-assembly-x86.dll", runtimes[i]);
    }
}

// X86 is always available for .NET Framework
StandardRunnerTests.Add(new PackageTest(
    1, "Net462X86Test",
    "Run mock-assembly-x86.dll under .NET 4.6.2",
    "net462/mock-assembly-x86.dll",
    new MockAssemblyX86ExpectedResult("net-4.6.2")));

if (dotnetX86Available)
{
    // TODO: Make tests run on all build platforms
    bool onAppVeyor = BuildSystem.IsRunningOnAppVeyor;
    bool onGitHubActions = BuildSystem.IsRunningOnGitHubActions;

    if (!onAppVeyor)
        StandardRunnerTests.Add(new PackageTest(
            1, "Net80X86Test",
            "Run mock-assembly-x86.dll under .NET 8.0",
            "net8.0/mock-assembly-x86.dll",
            new MockAssemblyX86ExpectedResult("netcore-8.0")));

    if (!onAppVeyor && !onGitHubActions)
        StandardRunnerTests.Add(new PackageTest(
            1, "Net70X86Test",
            "Run mock-assembly-x86.dll under .NET 7.0",
            "net7.0/mock-assembly-x86.dll",
            new MockAssemblyX86ExpectedResult("netcore-7.0")));

    StandardRunnerTests.Add(new PackageTest(
        1, "Net60X86Test",
        "Run mock-assembly-x86.dll under .NET 6.0",
        "net6.0/mock-assembly-x86.dll",
        new MockAssemblyX86ExpectedResult("netcore-6.0")));

    if (!onAppVeyor && !onGitHubActions)
        StandardRunnerTests.Add(new PackageTest(
            1, "NetCore31X86Test",
            "Run mock-assembly-x86.dll under .NET Core 3.1",
            "netcoreapp3.1/mock-assembly-x86.dll",
            new MockAssemblyX86ExpectedResult("netcore-3.1")));
}

//////////////////////////////////////////////////////////////////////
// RUN MULTIPLE COPIES OF MOCK-ASSEMBLY
//////////////////////////////////////////////////////////////////////

StandardRunnerTests.Add(new PackageTest(
    1, "Net462PlusNet462Test",
    "Run two copies of mock-assembly together",
    "net462/mock-assembly.dll net462/mock-assembly.dll",
    new MockAssemblyExpectedResult("net-4.6.2", "net-4.6.2")));

StandardRunnerTests.Add(new PackageTest(
    1, "Net60PlusNet80Test",
    "Run mock-assembly under .NET6.0 and 8.0 together",
    "net6.0/mock-assembly.dll net8.0/mock-assembly.dll",
    new MockAssemblyExpectedResult("netcore-6.0", "netcore-8.0")));

StandardRunnerTests.Add(new PackageTest(
    1, "Net462PlusNet60Test",
    "Run mock-assembly under .Net Framework 4.6.2 and .Net 6.0 together",
    "net462/mock-assembly.dll net6.0/mock-assembly.dll",
    new MockAssemblyExpectedResult("net-4.6.2", "netcore-6.0")));

//////////////////////////////////////////////////////////////////////
// ASP.NETCORE TESTS
//////////////////////////////////////////////////////////////////////

StandardRunnerTests.Add(new PackageTest(
    1, "Net60AspNetCoreTest", "Run test using AspNetCore targeting .NET 6.0",
    "net6.0/aspnetcore-test.dll", new ExpectedResult("Passed")
    {
        Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
        Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("aspnetcore-test.dll", "netcore-6.0") }
    }));

StandardRunnerTests.Add(new PackageTest(
    1, "Net80AspNetCoreTest", "Run test using AspNetCore targeting .NET 8.0",
    "net8.0/aspnetcore-test.dll", new ExpectedResult("Passed")
    {
        Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
        Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("aspnetcore-test.dll", "netcore-8.0") }
    }));

//////////////////////////////////////////////////////////////////////
// WINDOWS FORMS TESTS
//////////////////////////////////////////////////////////////////////

StandardRunnerTests.Add(new PackageTest(
    1, "Net60WindowsFormsTest", "Run test using windows forms under .NET 6.0",
    "net6.0-windows/windows-forms-test.dll", new ExpectedResult("Passed")
    {
        Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
        Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("windows-forms-test.dll", "netcore-6.0") }
    }));

StandardRunnerTests.Add(new PackageTest(
    1, "Net80WindowsFormsTest", "Run test using windows forms under .NET 8.0",
    "net8.0-windows/windows-forms-test.dll", new ExpectedResult("Passed")
    {
        Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
        Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("windows-forms-test.dll", "netcore-8.0") }
    }));

//////////////////////////////////////////////////////////////////////
// WPF TESTS
//////////////////////////////////////////////////////////////////////

StandardRunnerTests.Add(new PackageTest(
    1, "Net60WPFTest", "Run test using WPF under .NET 6.0",
    "net6.0-windows/WpfTest.dll --trace=Debug", 
    new ExpectedResult("Passed") { Assemblies = new[] { new ExpectedAssemblyResult("WpfTest.dll", "netcore-6.0") } }));

StandardRunnerTests.Add(new PackageTest(
    1, "Net80WPFTest", "Run test using WPF under .NET 8.0",
    "net8.0-windows/WpfTest.dll --trace=Debug",
    new ExpectedResult("Passed") { Assemblies = new[] { new ExpectedAssemblyResult("WpfTest.dll", "netcore-8.0") } }));

//////////////////////////////////////////////////////////////////////
// RUN TESTS USING EACH OF OUR EXTENSIONS
//////////////////////////////////////////////////////////////////////

StandardRunnerTests.Add(new PackageTest(
    1, "NUnitProjectTest",
    "Run project with both copies of mock-assembly",
    "../../NetFXTests.nunit --config=Release --trace=Debug",
    new MockAssemblyExpectedResult("net-4.6.2", "netcore-6.0"),
    KnownExtensions.NUnitProjectLoader.SetVersion("3.8.0")));

StandardRunnerTests.Add(new PackageTest(
    1, "V2ResultWriterTest",
    "Run mock-assembly under .NET 6.0 and produce V2 output",
    "net6.0/mock-assembly.dll --result=TestResult.xml --result=NUnit2TestResult.xml;format=nunit2",
    new MockAssemblyExpectedResult("netcore-6.0"),
    KnownExtensions.NUnitV2ResultWriter.SetVersion("3.8.0")));

StandardRunnerTests.Add(new PackageTest(
    1, "VSProjectLoaderTest_Project",
    "Run mock-assembly using the .csproj file",
    "../../src/TestData/mock-assembly/mock-assembly.csproj --config=Release",
    new MockAssemblyExpectedResult("net462", "netcore-3.1", "netcore-6.0", "netcore-7.0", "netcore-8.0"),
    KnownExtensions.VSProjectLoader.SetVersion("3.9.0")));

static ExpectedResult MockAssemblySolutionResult = new ExpectedResult("Failed")
{
    Total = 37 * 5,
    Passed = 23 * 5,
    Failed = 5 * 5,
    Warnings = 1 * 5,
    Inconclusive = 1 * 5,
    Skipped = 7 * 5,
    Assemblies = new ExpectedAssemblyResult[]
    {
        new ExpectedAssemblyResult("mock-assembly.dll", "net-4.6.2"),
        new ExpectedAssemblyResult("mock-assembly.dll", "netcore-3.1"),
        new ExpectedAssemblyResult("mock-assembly.dll", "netcore-6.0"),
        new ExpectedAssemblyResult("mock-assembly.dll", "netcore-7.0"),
        new ExpectedAssemblyResult("mock-assembly.dll", "netcore-8.0"),
        new ExpectedAssemblyResult("notest-assembly.dll", "net-4.6.2"),
        new ExpectedAssemblyResult("notest-assembly.dll", "netcore-3.1"),
        new ExpectedAssemblyResult("notest-assembly.dll", "netstandard-2.0"),
        new ExpectedAssemblyResult("WpfApp.exe")
    }
};

StandardRunnerTests.Add(new PackageTest(
    1, "VSProjectLoaderTest_Solution",
    "Run mock-assembly using the .sln file",
    "../../src/TestData/TestData.sln --config=Release --trace=Debug",
    MockAssemblySolutionResult,
    KnownExtensions.VSProjectLoader.SetVersion("3.9.0")));
