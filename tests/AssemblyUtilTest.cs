using FluentAssertions;
using Xunit.Abstractions;
using YmmeUtil;
namespace tests;

public class AssemblyUtilTest(ITestOutputHelper output)
{
	private readonly ITestOutputHelper _output = output;
	[Fact]
	public void GetVersion()
	{
		var v = AssemblyUtil.GetVersion(typeof(AssemblyUtilTest));
		v.Should().NotBeNull();
		v.Should().NotBe(new Version(0,0,0));
		_output.WriteLine(v.ToString());
	}

	[Fact]
	public void GetVersionString()
	{
		var v = AssemblyUtil.GetVersionString(typeof(AssemblyUtilTest));
		v.Should().NotBeNull();
		v.Should().NotBeEmpty();
		v.Should().Be(AssemblyUtil.GetVersion(typeof(AssemblyUtilTest)).ToString());
	}
}
