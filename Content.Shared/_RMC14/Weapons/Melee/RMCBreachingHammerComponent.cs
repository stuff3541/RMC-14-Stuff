using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._RMC14.Weapons.Melee;

[RegisterComponent]
public sealed partial class RMCBreachingHammerComponent : Component
{
    [DataField]
    public TimeSpan DoAfter = TimeSpan.FromSeconds(1.5);
}

[Serializable, NetSerializable]
public sealed partial class RMCBreachingHammerDoAfterEvent : SimpleDoAfterEvent
{
}