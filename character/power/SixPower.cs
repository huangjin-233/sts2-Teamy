using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Combat.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.cards;
using Teamy.character.cards.rare;
using Teamy.character.relic;

namespace Teamy.character.power;
[RegisterPower]
public class SixPower: ModTemporaryAppliedPowerTemplate<Six, DexterityPower>
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://images/powers/SixPower.png",
        BigIconPath: "res://images/powers/SixPower.png"
    );
}