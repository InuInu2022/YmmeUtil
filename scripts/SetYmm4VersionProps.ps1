$basePath = $env:YMM4_PATH
if ([string]::IsNullOrWhiteSpace($basePath)) {
    [Console]::Error.WriteLine("warning MSB9999: 環境変数 YMM4_PATH が設定されていません。DefineConstantsは変更しません。")
    exit 0
}
$dllPath = Join-Path $basePath "YukkuriMovieMaker.dll"
if (!(Test-Path $dllPath)) {
    [Console]::Error.WriteLine("warning MSB9999: YMM4 DLL が見つかりません: $dllPath。DefineConstantsは変更しません。")
    exit 0
}

$ver = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($dllPath).FileVersion
$major, $minor, $build, $revision = $ver.Split('.')
$define = ""
if ($major -eq 4 -and $minor -eq 45 -and $build -ge 1) {
    $define = "FOCUSHELPER_DEFAULTFOCUS_REMOVED"
}

$propsPath = Join-Path $PSScriptRoot "..\src\Directory.Build.props"
$propsPath = [System.IO.Path]::GetFullPath($propsPath)

[xml]$props = Get-Content $propsPath -Raw
$pg = $props.Project.PropertyGroup | Where-Object { $_.DefineConstants }
if ($pg) {
    # DefineConstantsノードを直接取得
    foreach ($node in $pg[0].ChildNodes) {
        if ($node.Name -eq "DefineConstants") {
            $node.InnerText = $define
        }
    }
}
else {
    $newGroup = $props.CreateElement("PropertyGroup")
    $newConst = $props.CreateElement("DefineConstants")
    $newConst.InnerText = $define
    $newGroup.AppendChild($newConst)
    $props.Project.AppendChild($newGroup)
}
$props.Save($propsPath)