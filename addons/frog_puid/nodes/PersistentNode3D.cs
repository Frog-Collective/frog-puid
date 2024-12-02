using System.Diagnostics;
using Godot;

namespace Frog;

[Tool]
public partial class PersistentNode3D : Node3D
{
    [Export]
    public ulong Puid
    {
        get;
        private set;
    }

#if TOOLS
    public override void _EnterTree()
    {
        if (Engine.IsEditorHint())
        {
            if (PuidGenerator.TryGeneratePuid(this, PersistentNode3D.PropertyName.Puid))
            {
                Debug.WriteLine($"Puid assigned: {this.Puid}");
            }
        }
    }

    public override string[]? _GetConfigurationWarnings()
    {
        if (string.IsNullOrEmpty(this.SceneFilePath))
        {
            return ["PersistentNode can only be a scene root."];
        }
        
        return default;
    }
#endif

    public override void _Ready()
    {
        Debug.Assert(Engine.IsEditorHint() || this.Puid != 0);
    }
}
