#if UNITY_5_6
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class ExportSubstanceTextures
{
    private const string SourcePath = "Assets/Substance_Database/Substance_Database_1.2.1_Sbsar/Textures/Metal/Wall_Steel_Sheet.sbsar";
    private const string OutputFolder = "Assets/ExtractedSubstance";

    [MenuItem("Tools/Export Wall Steel Sheet Textures")]
    public static void ExportWallSteelSheetMenu()
    {
        ExportWallSteelSheetInternal(false);
    }

    public static void ExportWallSteelSheet()
    {
        ExportWallSteelSheetInternal(true);
    }

    private static void ExportWallSteelSheetInternal(bool quitWhenDone)
    {
        Directory.CreateDirectory(OutputFolder);

        var assets = AssetDatabase.LoadAllAssetsAtPath(SourcePath);
        if (assets == null || assets.Length == 0)
        {
            Debug.LogError("No assets found at " + SourcePath);
            if (quitWhenDone)
            {
                EditorApplication.Exit(1);
            }
            return;
        }

        var exportedCount = 0;
        foreach (var asset in assets)
        {
            if (asset == null)
            {
                continue;
            }

            var proceduralTexture = asset as ProceduralTexture;
            if (proceduralTexture == null)
            {
                continue;
            }

            var outputPath = Path.Combine(OutputFolder, proceduralTexture.name + ".png").Replace("\\", "/");
            var readableTexture = TryBuildReadableTexture(proceduralTexture);
            if (readableTexture == null)
            {
                Debug.LogError("Failed to export " + proceduralTexture.name);
                continue;
            }

            File.WriteAllBytes(outputPath, readableTexture.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(readableTexture);
            Debug.Log("Exported " + proceduralTexture.name + " -> " + outputPath);
            exportedCount++;
        }

        AssetDatabase.Refresh();
        Debug.Log("Exported " + exportedCount + " procedural textures from " + SourcePath);
        if (quitWhenDone)
        {
            EditorApplication.Exit(exportedCount > 0 ? 0 : 2);
        }
    }

    private static Texture2D TryBuildReadableTexture(ProceduralTexture proceduralTexture)
    {
        try
        {
            var readableTexture = new Texture2D(proceduralTexture.width, proceduralTexture.height, TextureFormat.ARGB32, false);
            readableTexture.SetPixels32(GetProceduralPixels32(proceduralTexture));
            readableTexture.Apply(false, false);

            if (!IsMostlyBlack(readableTexture))
            {
                return readableTexture;
            }

            UnityEngine.Object.DestroyImmediate(readableTexture);
        }
        catch (Exception exception)
        {
            Debug.LogWarning("Pixel export failed for " + proceduralTexture.name + ": " + exception.Message);
        }

        var previewTexture = WaitForPreviewTexture(proceduralTexture);
        if (previewTexture == null)
        {
            return null;
        }

        var previewCopy = new Texture2D(previewTexture.width, previewTexture.height, TextureFormat.ARGB32, false);
        previewCopy.SetPixels(previewTexture.GetPixels());
        previewCopy.Apply(false, false);
        return previewCopy;
    }

    private static Color32[] GetProceduralPixels32(ProceduralTexture proceduralTexture)
    {
        var blockPixels32 = typeof(ProceduralTexture).GetMethod(
            "GetPixels32",
            BindingFlags.Public | BindingFlags.Instance,
            null,
            new[] { typeof(int), typeof(int), typeof(int), typeof(int) },
            null);
        if (blockPixels32 != null)
        {
            return (Color32[])blockPixels32.Invoke(
                proceduralTexture,
                new object[] { 0, 0, proceduralTexture.width, proceduralTexture.height });
        }

        throw new MissingMethodException("ProceduralTexture pixel read API not found.");
    }

    private static Texture2D WaitForPreviewTexture(UnityEngine.Object asset)
    {
        for (var i = 0; i < 50; i++)
        {
            var preview = AssetPreview.GetAssetPreview(asset);
            if (preview != null)
            {
                return preview;
            }

            preview = AssetPreview.GetMiniThumbnail(asset);
            if (preview != null && preview.width > 32 && preview.height > 32)
            {
                return preview;
            }

            AssetPreview.SetPreviewTextureCacheSize(64);
        }

        return null;
    }

    private static bool IsMostlyBlack(Texture2D texture)
    {
        var pixels = texture.GetPixels32();
        var nonBlackPixels = 0;
        for (var i = 0; i < pixels.Length; i++)
        {
            var pixel = pixels[i];
            if (pixel.r > 5 || pixel.g > 5 || pixel.b > 5)
            {
                nonBlackPixels++;
                if (nonBlackPixels > pixels.Length / 100)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
#endif
