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
                Debug.Log($"Loading Mod: {addon.Name} - {addon.Description} (v{addon.Version})");
                bool isEnabled = PlayerPrefs.GetInt(addon.Name, 0) == 1;
                if (isEnabled)
                {
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
                else
                {
                    Debug.Log($"Skipping Disabled Mod: {addon.Name}");
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
            Debug.Log($"Loading mod file: {modFile}");
            string code = File.ReadAllText(modFile);
            Debug.Log("Mod file code read successfully.");
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            Debug.Log("Syntax tree created.");
            var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => !assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location))
            .Select(assembly => assembly.Location)
            .ToList();
            string unityEngineLocation = typeof(UnityEngine.Object).Assembly.Location;
            if (!string.IsNullOrEmpty(unityEngineLocation))
            {
                references.Add(unityEngineLocation);
                Debug.Log($"Added UnityEngine reference: {unityEngineLocation}");
            }
            else
            {
                Debug.LogWarning("UnityEngine reference is invalid or empty.");
            }
            Debug.Log($"Total references: {references.Count}");
            var compilation = CSharpCompilation.Create(
            Path.GetFileNameWithoutExtension(modFile),
            syntaxTrees: new[] { syntaxTree },
            references: references.Select(r => MetadataReference.CreateFromFile(r)),
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );
            Debug.Log("Compilation object created.");
            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);
                if (result.Success)
                {
                    Debug.Log("Compilation succeeded!");
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = System.Reflection.Assembly.Load(ms.ToArray());
                    Debug.Log("Assembly loaded successfully.");
                    InvokeModMethods(assembly);
                    return true;
                }
                else
                {
                    Debug.LogError("Compilation failed.");
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Debug.LogError($"Diagnostic: {diagnostic.ToString()}");
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