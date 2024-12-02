#if TOOLS
using Godot;

namespace Frog;

[Tool]
public partial class PuidPlugin : EditorPlugin
{
    public override void _EnterTree()
    {
        Script persistentNode2DScript = GD.Load<Script>("res://addons/frog_puid/nodes/PersistentNode2D.cs");
        Texture2D persistentNode2DIcon = GD.Load<Texture2D>("res://addons/frog_puid/icons/persistent_node_2D.svg");
        this.AddCustomType(nameof(PersistentNode2D), nameof(Node2D), persistentNode2DScript, persistentNode2DIcon);

        Script persistentNode3DScript = GD.Load<Script>("res://addons/frog_puid/nodes/PersistentNode3D.cs");
        Texture2D persistentNode3DIcon = GD.Load<Texture2D>("res://addons/frog_puid/icons/persistent_node_3d.svg");
        this.AddCustomType(nameof(PersistentNode3D), nameof(Node3D), persistentNode3DScript, persistentNode3DIcon);
    }

    public override void _ExitTree()
    {
        this.RemoveCustomType(nameof(PersistentNode2D));
        this.RemoveCustomType(nameof(PersistentNode3D));
    }
}
#endif
