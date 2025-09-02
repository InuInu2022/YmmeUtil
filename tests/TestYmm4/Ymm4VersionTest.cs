using System;

using Xunit;

using YmmeUtil.Ymm4;

namespace TestYmm4;

public class Ymm4VersionTest
{
	// Helper to set the Current version via reflection (since it's a static property)
	private void SetCurrentVersion(Version version)
	{
		Ymm4Version.Current = version;
	}

	[Theory]
	[InlineData(4, 45, 0, true)] // >= 4.45.0
	[InlineData(4, 44, 9, false)]
	public void HasDockedReturnsExpected(int major, int minor, int build, bool expected)
	{
		SetCurrentVersion(new Version(major, minor, build));
		Assert.Equal(expected, Ymm4Version.HasDocked);
	}

	[Theory]
	[InlineData(4, 35, 0, true)] // >= 4.35.0
	[InlineData(4, 34, 9, false)]
	public void HasRequiredNet90ReturnsExpected(int major, int minor, int build, bool expected)
	{
		SetCurrentVersion(new Version(major, minor, build));
		Assert.Equal(expected, Ymm4Version.HasRequiredNet90);
	}

	[Theory]
	[InlineData(4, 23, 0, true)] // >= 4.23.0 and < 4.35.0
	[InlineData(4, 34, 9, true)]
	[InlineData(4, 35, 0, false)]
	[InlineData(4, 22, 9, false)]
	public void HasRequiredNet80ReturnsExpected(int major, int minor, int build, bool expected)
	{
		SetCurrentVersion(new Version(major, minor, build));
		Assert.Equal(expected, Ymm4Version.HasRequiredNet80);
	}

	[Theory]
	[InlineData(4, 33, 0, true)] // >= 4.33.0
	[InlineData(4, 32, 9, false)]
	public void HasDeveloperModeReturnsExpected(int major, int minor, int build, bool expected)
	{
		SetCurrentVersion(new Version(major, minor, build));
		Assert.Equal(expected, Ymm4Version.HasDeveloperMode);
	}

	[Theory]
	[InlineData(4, 29, 0, true)] // >= 4.29.0
	[InlineData(4, 28, 9, false)]
	public void HasPluginInstallerReturnsExpected(int major, int minor, int build, bool expected)
	{
		SetCurrentVersion(new Version(major, minor, build));
		Assert.Equal(expected, Ymm4Version.HasPluginInstaller);
	}

	[Theory]
	[InlineData(4, 20, 0, true)] // >= 4.20.0
	[InlineData(4, 19, 9, false)]
	public void IsSupportPluginReturnsExpected(int major, int minor, int build, bool expected)
	{
		SetCurrentVersion(new Version(major, minor, build));
		Assert.Equal(expected, Ymm4Version.IsSupportPlugin);
	}
}