using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TuxModLoader
{
    public class ModSetup
    {
        private string modsDirectory;
        private StartupPanel startupPanel;
        private int successfulMods;
        private int failedMods;

        public ModSetup(StartupPanel panel)
        {
            modsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Addons");
            startupPanel = panel;
            successfulMods = 0;
            failedMods = 0;
        }

        public void LoadMods()
        {
            List<Addon> loadedMods = new List<Addon>();

            if (Directory.Exists(modsDirectory))
            {
                string[] modDirs = Directory.GetDirectories(modsDirectory);

                foreach (var modDir in modDirs)
                {
                    string modCodePath = Path.Combine(modDir, "Code");
                    string addonPath = Path.Combine(modDir, "info.json");

                    if (Directory.Exists(modCodePath) && File.Exists(addonPath))
                    {
                        var addon = LoadMod(addonPath, modCodePath);
                        if (addon != null)
                        {
                            loadedMods.Add(addon);
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("Addons directory does not exist.");
            }

            startupPanel.ShowStartupPanel(successfulMods, failedMods);
        }

        private Addon LoadMod(string addonPath, string modCodePath)
        {
            var addon = ReadModInfo(addonPath);
            if (addon != null)
            {
                Debug.Log($"Loading Mod: {addon.AddonName} - {addon.Description} (v{addon.Version})");

                SpriteLoader spriteLoader = new SpriteLoader();
                spriteLoader.LoadSprites(Path.GetDirectoryName(addonPath));

                var modFiles = Directory.GetFiles(modCodePath, "*.cs", SearchOption.AllDirectories);
                foreach (var file in modFiles)
                {
                    bool success = LoadAndCompileMod(file);
                    if (success)
                    {
                        successfulMods++;
                    }
                    else
                    {
                        failedMods++;
                    }
                }
            }
            return addon;
        }

        private Addon ReadModInfo(string addonPath)
        {
            try
            {
                string json = File.ReadAllText(addonPath);
                return JsonConvert.DeserializeObject<Addon>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error reading info.json: {ex.Message}");
                return null;
            }
        }

        private bool LoadAndCompileMod(string modFile)
        {
            string code = File.ReadAllText(modFile);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .Select(assembly => assembly.Location)
                .ToList();

            references.Add(typeof(UnityEngine.Object).Assembly.Location);

            var compilation = CSharpCompilation.Create(
                Path.GetFileNameWithoutExtension(modFile),
                syntaxTrees: new[] { syntaxTree },
                references: references.Select(r => MetadataReference.CreateFromFile(r)),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);
                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = System.Reflection.Assembly.Load(ms.ToArray());
                    InvokeModMethods(assembly);
                    return true;
                }
                else
                {
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Debug.LogError(diagnostic.ToString());
                    }
                    return false;
                }
            }
        }

        private void InvokeModMethods(System.Reflection.Assembly assembly)
        {
            var modType = assembly.GetTypes().FirstOrDefault(type => type.GetMethod("Init") != null);
            if (modType != null)
            {
                var methodInfo = modType.GetMethod("Init");
                var instance = Activator.CreateInstance(modType);
                methodInfo.Invoke(instance, null);
            }
            else
            {
                Debug.LogError("Mod does not contain an Init method.");
            }
        }
    }
}