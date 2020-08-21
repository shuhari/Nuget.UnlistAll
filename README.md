# Nuget.UnlistAll

![Main screen](https://github.com/shuhari/Nuget.UnlistAll/raw/master/screenshots/main-app.jpg)

## What's this?

Sometimes you need to unlist packages from nuget registry, 
in case the code is obsolute or moved (nuget would not allow
you to actually delete one package). But the package may 
have many history versions, and it's inefficient to unlist
the versions one-by-one in the control panel. So I write 
this tool to unlist all package versions, in batch mode.

## Install

If you prefer pre-built binary, then a nuget package is already 
here. Run 
<pre>install-package Nuget.UnlistAll</pre>
from nuget package console to get the binary.

Or you can get the code and build it yourself.


## How to use?

* Run application (Nuget.UnlistAll.exe)
* Input the package you want to delete along with your nuget api key
* Click 'Get Versions' to search for history versions
* (Optional) You can use 'Select' to select which version(s) to delete
* When you are sure, click 'Unlist!' to execute.

## Release History

* 2017-6-15 v0.1.1 
  * References shuhari.framework.common
  * Add global error log
  * Better about dialog
  * Confirm before unlist 
  * Internal code refactor
  
* 2017-6-14 v0.1.0 

  * Initial version

For full history list, see [HISTORY.md](https://github.com/shuhari/Nuget.UnlistAll/raw/master/HISTORY.md).


## Reference

* [Unlist a package from Nuget with all it's history versions](https://stackoverflow.com/questions/9853884/unlist-a-package-from-nuget-with-all-its-history-versions)
* [How to remove a package from nuget.org](http://blog.gauffin.org/2016/09/how-to-remove-a-package-from-nuget-org/)
* [Introduction blog (Chinese)](https://shuhari.dev/blog/2017/06/nuget-batch-unlist-tool)
 
