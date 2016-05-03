# GTAVRemultiplied

A fanciful high-hopes mod for Grand Theft Auto V

More information coming soon!

### Installing From Release

- Download the release .zip file, extract its contents to a folder
- Download the file at http://www.dev-c.com/gtav/scripthookv/ and include the files ScriptHookV.dll and dinput8.dll in your folder created by the first step
- Run InstallGTAVRMP.exe, click "Find It" at the top right, and click your GTA5.exe file, where that is (Wherever you installed GTAV, EG in `C:/Program Files (x86)/Steam/steamapps/common/Grand Theft Auto V/GTA5.exe`)
- Click "Install GTAV RMP"

### Installing From Source (Manual)

- Install http://www.dev-c.com/gtav/scripthookv/ using their resources.
- Install https://github.com/crosire/scripthookvdotnet/releases using their resources or the copy contained in this source.
- Build this project from the source available here, using Visual Studio 2015 (or acquire the dll's from a release copy).
- Copy the resultant dll's (GTAVRemultiplied, YAMLDotNet, FreneticScript) of this project's build into your `GTAV/scripts/` folder.
- In your GTAV folder, create a folder labeled "frenetic", and in that one labeled "client" and one labeled "server", in each, add a server labeled "scripts".
- You are now ready to play!

### Licensing pre-note:

This is an open source project, provided entirely freely, for everyone to use and contribute to.

If you make any changes that could benefit the community as a whole, please contribute upstream.

### The short of the license is:

You can do basically whatever you want, except you may not hold any developer liable for what you do with the software.

### The long version of the license follows:

The MIT License (MIT)

Copyright (c) 2016 Frenetic XYZ

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
