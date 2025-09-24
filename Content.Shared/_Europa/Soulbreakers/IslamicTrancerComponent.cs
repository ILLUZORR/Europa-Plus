using Content.Shared.Actions;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._Europa.Soulbreakers;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class IslamicTrancerComponent : Component
{
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string? AllahAction = "ActionPraiseAllah";

    [DataField, AutoNetworkedField]
    public EntityUid? AllahActionEntity;

    private static readonly ProtoId<SoundCollectionPrototype> PraiseAllahSounds = new("PraiseAllah");
    public SoundSpecifier PraiseAllahSound = new SoundCollectionSpecifier(PraiseAllahSounds, AudioParams.Default.WithVariation(0.1f));
}

public sealed partial class PraiseAllahEvent : InstantActionEvent
{
}
