using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Combat.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.cards;
using Teamy.character.cards.common;
using Teamy.character.relic;

namespace Teamy.character.power;
[RegisterPower]
public class RabitPower: ModTemporaryAppliedPowerTemplate<Rabits,StrengthPower>
{
    public override PowerType Type => PowerType.Debuff;
    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override bool IsPositive => false;
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://images/powers/RabitPower.png",
        BigIconPath: "res://images/powers/RabitPower.png"
    );
}