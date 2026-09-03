using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.relic;
using Teamy.character.single;

namespace Teamy.character.power;

[RegisterPower]
public class LongXia: ModPowerTemplate
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;
    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;

    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://images/powers/LongXia.png",
        BigIconPath: "res://images/powers/LongXia.png"
    );

    private bool xty;
    private bool zzl;
    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Xty))
            xty = true;
        if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Zzl))
            zzl = true;
        if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
        {
            xty = true;
            zzl = true;
        }
        if (xty && zzl)
        {
            await CreatureCmd.Heal(Owner.Player.Creature, Amount);
            xty=false;
            zzl=false;
        }
    }
}