using Content.Shared.Damage;

namespace Content.Shared._Europa.Morph;

[RegisterComponent]
public sealed partial class MorphAmbushComponent : Component
{
    /// <summary>
    /// время стана после касания, но не удара
    /// </summary>
    [DataField]
    public int StunTimeInteract = 6;
    /// <summary>
    /// урон при касании
    /// </summary>
    [DataField]
    public DamageSpecifier DamageOnTouch = new()
    {
        DamageDict = new()
        {
            { "Blunt", 20 },
            { "Slash", 20 },
        }
    };
}
