param($installPath, $toolsPath, $package, $project)

function Get-ProjectProperty($name)
{
    $property = $project.Properties.Item($name)

    if (!$property)
    {
        return $null
    }

    return $property.Value
}

function Merge-FileTokens($inputPath, $outputPath, $tokens)
{
    Get-Content $inputPath | %{
        [regex]::Replace(
            $_,
            '\$(?<tokenName>\w+)\$',
            {
                param($match)

                $tokenName = $match.Groups['tokenName'].Value

                return $tokens[$tokenName]
            })
    } | Set-Content $outputPath
}

function Copy-ConfigSection($srcXml, $dstXml, $name)
{
    $node = $srcXml.DocumentElement[$name]

    if (!$node)
    {
        return;
    }

    $nodeCopy = $dstXml.ImportNode($node, $true)

    $dstXml.DocumentElement.AppendChild($nodeCopy) | Out-Null
}

$outputType = Get-ProjectProperty OutputType

# If this is a class library...
if ($outputType -eq 2)
{
    $projectPath = Get-ProjectProperty FullPath
    $webConfigPath = Join-Path $projectPath Web.config

    if (!(Test-Path $webConfigPath))
    {
        $webConfigSrcPath = Join-Path $toolsPath Web.config
        $entityFramework = $project.Object.References | ?{ $_.Name -eq 'EntityFramework' }

        Merge-FileTokens $webConfigSrcPath $webConfigPath @{
                Version = $entityFramework.Version
            }

        $appConfigPath = Join-Path $projectPath App.config

        if (Test-Path $appConfigPath)
        {
            $appConfigXml = [xml](Get-Content $appConfigPath)
            $webConfigXml = [xml](Get-Content $webConfigPath)

            Copy-ConfigSection $appConfigXml $webConfigXml connectionStrings

            $settings = New-Object System.Xml.XmlWriterSettings -Property @{
                    Indent = $true
                }

            $writer = [System.Xml.XmlWriter]::Create($webConfigPath, $settings)
            try
            {
                # NOTE: Using an XmlWriter here because XmlDocument.Save(string) results in
                #       inconsistent line breaks
                $webConfigXml.Save($writer)
            }
            finally
            {
                $writer.Dispose()
            }
        }

        $project.ProjectItems.AddFromFile($webConfigPath) | Out-Null
    }
}
