param($installPath, $toolsPath, $package)
#https://github.com/jonnii/BuildDeploySupport/blob/master/package/tools/Init.ps1

# find out where to put the files, we're going to create a deploy directory
# at the same level as the solution.

$rootDir = (Get-Item $installPath).parent.parent.fullname
$deployTarget = "$rootDir\SrirachaTools\Runners"

# create our deploy support directory if it doesn't exist yet

#$deploySource = join-path $installPath 'tools/deploy'
$deploySource = join-path $installPath 'tools/Runners'

if (!(test-path $deployTarget)) {
	mkdir $deployTarget
}

# copy everything in there

Copy-Item "$deploySource/*" $deployTarget -Recurse -Force
