using System.Diagnostics;
using System.Security.Principal;
using System.Text.RegularExpressions;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

Console.WriteLine("Patch made by Polar.");

#pragma warning disable CA1416
if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
#pragma warning restore CA1416
{
    Console.WriteLine("Please run as Administrator!");
    Console.WriteLine();
    Console.ReadKey();
    return;
}

var vrcx = Process.GetProcessesByName("VRCX");

if (vrcx.Length == 0)
{
    Console.WriteLine("please launch VRCX before attempting to patch it!");
    Console.ReadKey();
    return;
}

vrcx[0].Kill();

var dir = Path.GetDirectoryName(vrcx[0].MainModule?.FileName) + "\\html\\app.js";

var method = @"e\.methods\.isLocalUserVrcplusSupporter=function\(\)\{\s*return\s*g\.currentUser\.\$isVRCPlus\s*\}";

var enstr = @"Local Favorites \(Requires VRC\+\)";

var patch = "e.methods.isLocalUserVrcplusSupporter=function(){return true}";

var enpatch = "Local Favorites";

var code = File.ReadAllText(dir);

var methodReg = new Regex(method);
var enstrReg = new Regex(enstr);

if (methodReg.IsMatch(code) & enstrReg.IsMatch(code))
{
    code = methodReg.Replace(code, patch);
    code = enstrReg.Replace(code, enpatch);
    
    File.WriteAllText(dir, code);
    Console.WriteLine("Successfully patched VRCX!");
}
else
{
    Console.WriteLine("Unable to patch VRCX!");
}

Console.ReadKey();
