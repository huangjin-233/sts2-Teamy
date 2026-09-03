using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;
using Teamy.character.power;

namespace Teamy.character.single;

[RegisterSingleton]
public class HeadSingle : HookedSingletonModel
{
    // 填入HookType，可选Combat、Run或者None。Combat涉及战斗接口，Run涉及全局接口，具体看Hook类中的定义。
    public HeadSingle() : base(HookType.Run)
    {
    }

    public override async Task AfterCombatEnd(CombatRoom combatRoom)
    {
        var players = combatRoom.Allies;
        foreach (var creature in players)
        {
            // 检查这个生物是否是玩家（通常 Player 是 Creature 的子类或包装）
                // 检查玩家是否拥有特定能力
                bool hasHealAbility = creature.HasPower<Head>();
                if (hasHealAbility)
                {
                    // 执行回血
                    await CreatureCmd.Heal(creature,creature.GetPowerAmount<Head>());
                }
        }
        await Task.CompletedTask;
    }

    // public async override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    // {
    //     Log.Info($"AfterCardDrawn: {card.Id}");
    // }
}