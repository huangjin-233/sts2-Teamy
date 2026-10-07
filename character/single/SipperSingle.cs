#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;
using Teamy.character.power;

namespace Teamy.character.single;

[RegisterSingleton]
public class SipperSingle : HookedSingletonModel
{
    // 填入HookType，可选Combat、Run或者None。Combat涉及战斗接口，Run涉及全局接口，具体看Hook类中的定义。
    public SipperSingle() : base(HookType.Combat)
    {
    }
    //滑溜修正
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target.IsPlayer && target.HasPower<SlipperyPower>() && target.Player.Relics.Any(r => r is TungstenRod) && !target.HasPower<IntangiblePower>() && !target.HasPower<NutPower>() && target.Block == 0)
        {
            await PowerCmd.Apply<SlipperyPower>(choiceContext,target, -1, target, null);
        }
    }
}