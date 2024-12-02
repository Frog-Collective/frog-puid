using System.Diagnostics;
using System.Text.RegularExpressions;
using Godot;

namespace Frog;

public static class PuidGenerator
{
    private const string nextPuidKey = "frog_collective/next_puid";
    private static readonly Regex nodeNameRegex = new(@"^(?<name>[^#]*)(#.*)?$");

    public static bool TryGeneratePuid(Node node, string puidPropertyName)
    {
        Debug.Assert(Engine.IsEditorHint(), "This tool can only be called in editor.");

        if (!node.IsPartOfEditedScene())
        {
            // This code cannot run for nodes instantiated during runtime.
            return false;
        }

        if (string.IsNullOrEmpty(node.SceneFilePath))
        {
            return false;
        }

        string editedSceneRoot = EditorInterface.Singleton.GetEditedSceneRoot().SceneFilePath;
        Debug.WriteLine($"EditedScene: {editedSceneRoot} nodeScene: {node.SceneFilePath}");

        // A scene asset cannot contains a PUID since it would then be duplicated when instanced.
        if (node.SceneFilePath != editedSceneRoot)
        {
            // Check if node as an invalid Puid...
            ulong currentPuid = node.Get(puidPropertyName).As<ulong>();
            if (currentPuid == 0 || !node.Name.ToString().EndsWith($"#{currentPuid}"))
            {
                // Parse node name.
                Match match = PuidGenerator.nodeNameRegex.Match(node.Name);
                Debug.Assert(match.Success);
                string nameWithoutPuid = match.Groups["name"].Value;
                
                // Assign new puid.
                ulong nextPuid = ProjectSettings.GetSetting(nextPuidKey, 1u).As<ulong>();
                node.Name = $"{nameWithoutPuid}#{nextPuid}";
                node.Set(puidPropertyName, nextPuid);

                // Write next available puid in project settings.
                nextPuid++;
                ProjectSettings.SetSetting(nextPuidKey, nextPuid);
                ProjectSettings.Save();
                
                return true;
            }
        }
        
        return false;
    }
}
