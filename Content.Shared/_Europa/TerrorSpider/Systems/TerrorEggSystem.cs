using System.Linq;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Damage;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared._Europa.TerrorSpider;

//
// License-Identifier: MIT
//

public sealed class TerrorEggSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;

    private readonly Dictionary<EntityUid, Entity<EggHolderComponent>> _eggs = [];
    private EntProtoId[] _terrorSpiders = [];
    private DamageTypePrototype? _blunt;
    private DamageSpecifier? _damage;

    public override void Initialize()
    {
        base.Initialize();

        _terrorSpiders = _prototype.EnumeratePrototypes<EntityPrototype>()
            .Where(p => p.ID.StartsWith("MobTerrorSpider") && !p.Abstract)
            .Select(p => new EntProtoId(p.ID))
            .ToArray();

        SubscribeLocalEvent<EggHolderComponent, ComponentInit>(AddEgg);
        SubscribeLocalEvent<EggHolderComponent, ComponentShutdown>(RemoveEgg);
    }

    private void AddEgg(Entity<EggHolderComponent> ent, ref ComponentInit args) => _eggs.TryAdd(ent.Owner, ent);
    private void RemoveEgg(Entity<EggHolderComponent> ent, ref ComponentShutdown args) => _eggs.Remove(ent.Owner);

    public float Threshold { get; set; } = 1f;
    public override void Update(float frameTime)
    {
        foreach (var egg in _eggs)
        {
            egg.Value.Comp.Counter++;
            _blunt ??= _prototype.Index<DamageTypePrototype>("Blunt");
            _damage ??= new(_blunt, 1);
            _damageable.TryChangeDamage(egg.Value.Owner, _damage, false);

            if (egg.Value.Comp.Counter >= 300)
            {
                var entity = EntityManager.SpawnEntity(_random.Pick(_terrorSpiders), Transform(egg.Value.Owner).Coordinates);
                RemComp<EggHolderComponent>(egg.Value.Owner);
            }
        }
    }
}
