using Content.Shared.Actions;
using Content.Shared.Chat;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;

namespace Content.Shared._Europa.Soulbreakers;

public abstract class SharedIslamicTrancerSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audioSystem = default!;
    [Dependency] private readonly SharedChatSystem _chatSystem = default!;
    [Dependency] private readonly INetManager _netManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IslamicTrancerComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<IslamicTrancerComponent, ComponentRemove>(OnComponentRemoved);


        SubscribeLocalEvent<IslamicTrancerComponent, PraiseAllahEvent>(OnPraiseAllah);
    }

    private void OnComponentInit(EntityUid uid, IslamicTrancerComponent component, ComponentInit args)
    {
        _actionsSystem.AddAction(uid, ref component.AllahActionEntity, component.AllahAction);
    }

    private void OnComponentRemoved(EntityUid uid, IslamicTrancerComponent component, ComponentRemove args)
    {
        _actionsSystem.RemoveAction(uid, component.AllahActionEntity);
    }

    private void OnPraiseAllah(EntityUid uid, IslamicTrancerComponent component, PraiseAllahEvent args)
    {
        if (args.Handled)
            return;

        if (_netManager.IsServer)
        {
            _audioSystem.PlayPvs(component.PraiseAllahSound, args.Performer);
        }

        _chatSystem.TrySendInGameICMessage(uid, "praises Allah!", InGameICChatType.Emote, false);

        args.Handled = true;
    }
}
