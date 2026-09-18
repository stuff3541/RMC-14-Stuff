using Content.Shared._RMC14.Weapons.Melee;
using Content.Shared.DoAfter;
using Content.Shared.Tag;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Server._RMC14.Weapons.Melee;

public sealed class RMCBreachingHammerSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly TagSystem _tags = default!;

    private static readonly ProtoId<TagPrototype> WallTag = "Wall";

    public override void Initialize()
    {
        SubscribeLocalEvent<RMCBreachingHammerComponent, MeleeHitEvent>(OnMeleeHit);
        SubscribeLocalEvent<RMCBreachingHammerComponent, RMCBreachingHammerDoAfterEvent>(OnDoAfter);
    }

    private void OnMeleeHit(Entity<RMCBreachingHammerComponent> hammer, ref MeleeHitEvent args)
    {
        if (!args.IsHit)
            return;

        foreach (var target in args.HitEntities)
        {
            if (!_tags.HasTag(target, WallTag))
                continue;

            var doAfter = new DoAfterArgs(
                EntityManager,
                args.User,
                hammer.Comp.DoAfter,
                new RMCBreachingHammerDoAfterEvent(),
                hammer,
                target,
                hammer);

            _doAfter.TryStartDoAfter(doAfter);
        }
    }

    private void OnDoAfter(Entity<RMCBreachingHammerComponent> hammer, ref RMCBreachingHammerDoAfterEvent args)
    {
        if (args.Cancelled || args.Target is not { } target || !_tags.HasTag(target, WallTag))
            return;

        var targetTransform = Transform(target);
        var girder = SpawnAtPosition("CMGirder", targetTransform.Coordinates);
        Transform(girder).LocalRotation = targetTransform.LocalRotation;
        QueueDel(target);
    }
}

