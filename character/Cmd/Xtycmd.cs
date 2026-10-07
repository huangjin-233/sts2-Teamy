using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Teamy.character.cards.common;
using Teamy.character.power;
using Teamy.character.relic;

namespace Teamy.character;

// 夏天Y：其他 —— 随机触发一个词条效果
public static class Xtycmd
{
    public static async Task xty(PlayerChoiceContext context, CardPlay cardPlay, RelicModel relic,Player Owner)
    {
        int xty = relic.Owner.RunState.Rng.CombatCardGeneration.NextInt(0, 5);
        switch (xty)
        {
            case 0:
                await six(context, relic);
                //六谷
                break;
            case 1:
                await bc(context, relic);

                //半城
                break;
            case 2:
                await zzl(context, cardPlay, relic);
                //赵子龙
                break;
            case 3:
                await wsd(context, relic,Owner);

                //王少栋
                break;
            case 4:
                await swk(context, relic);

                //孙悟空
                break;
            case 5:
                await gou(context, relic);
                //狗哥
                break;
        }
    }

    // 六谷：敏捷
    public static async Task six(PlayerChoiceContext context, RelicModel relic, int times = 1)
    {
        relic.Flash();
        for (int i = 0; i < times; i++)
        {
            await PowerCmd.Apply<SixPower>(context, relic.Owner.Creature, 1, relic.Owner.Creature, null);
        }
    }

    // BC：滑溜
    public static async Task bc(PlayerChoiceContext context, RelicModel relic, int times = 1)
    {
        relic.DynamicVars["BcNum"].UpgradeValueBy(times);
        if (relic.DynamicVars["BcNum"].BaseValue >= relic.DynamicVars["bc"].BaseValue)
        {
            relic.Flash();
            relic.DynamicVars["BcNum"].BaseValue = 0;
            await PowerCmd.Apply<SlipperyPower>(context, relic.Owner.Creature, 1, relic.Owner.Creature, null);
        }
    }

    // 赵子龙：防御
    public static async Task zzl(PlayerChoiceContext context, CardPlay cardPlay, RelicModel relic, int times = 1)
    {
        relic.Flash();
        for (int i = 0; i < times; i++)
        {
            await CreatureCmd.GainBlock(relic.Owner.Creature, relic.DynamicVars.Block, cardPlay);
        }
    }

    // WSD：无实体
    public static async Task wsd(PlayerChoiceContext context, RelicModel relic,Player player)
    {
        relic.DynamicVars["WsdNum"].UpgradeValueBy(1);
        if (relic.DynamicVars["WsdNum"].BaseValue == relic.DynamicVars["wsd"].BaseValue)
        {
            relic.Flash();
            Tian.SetWsd(player,true);
            TianPlus.SetWsd(player,true);
        }
    }

    // 孙悟空：荆棘
    public static async Task swk(PlayerChoiceContext context, RelicModel relic, int times = 1)
    {
        relic.DynamicVars["SwkNum"].UpgradeValueBy(times);
        if (relic.DynamicVars["SwkNum"].BaseValue >= relic.DynamicVars["swk"].BaseValue)
        {
            relic.Flash();
            relic.DynamicVars["SwkNum"].BaseValue = 0;
            await PowerCmd.Apply<ThornsPower>(context, relic.Owner.Creature, 2, relic.Owner.Creature, null);
        }
    }

    // 狗哥：临时力量
    public static async Task gou(PlayerChoiceContext context, RelicModel relic, int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            await PowerCmd.Apply<GouPower>(context, relic.Owner.Creature, 1, relic.Owner.Creature, null);
        }
    }
}