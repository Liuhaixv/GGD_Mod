using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(GGD_Hack.BuildInfo.Description)]
[assembly: AssemblyDescription(GGD_Hack.BuildInfo.Description)]
[assembly: AssemblyCompany(GGD_Hack.BuildInfo.Company)]
[assembly: AssemblyProduct(GGD_Hack.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + GGD_Hack.BuildInfo.Author)]
[assembly: AssemblyTrademark(GGD_Hack.BuildInfo.Company)]
[assembly: AssemblyVersion(GGD_Hack.BuildInfo.Version)]
[assembly: AssemblyFileVersion(GGD_Hack.BuildInfo.Version)]
[assembly: MelonInfo(typeof(GGD_Hack.TestMod), GGD_Hack.BuildInfo.Name, GGD_Hack.BuildInfo.Version, GGD_Hack.BuildInfo.Author, GGD_Hack.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]