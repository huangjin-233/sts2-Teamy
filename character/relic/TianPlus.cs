using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.relic
{

    [RegisterRelic(typeof(TeamyRelicPool))]
    
    public class TianPlus : ModRelicTemplate
    {
        // 稀有度
        public override RelicRarity Rarity => RelicRarity.Starter;

        // 遗物的数值。这里会替换本地化中的{Cards}。
        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new BlockVar(1, ValueProp.Unpowered),
            ModCardVars.Int("bc", 5),
            ModCardVars.Int("swk",3 ),
            ModCardVars.Int("wsd",3 ),
            ModCardVars.Int("BcNum", 0),
            ModCardVars.Int("SwkNum",0 ),
            ModCardVars.Int("WsdNum",0 ),
        ];

        public override RelicAssetProfile AssetProfile => new(
            // 小图标（原版85x85）
            IconPath: $"res://images/relics/{GetType().Name}.png",
            // 轮廓图标（原版85x85）
            IconOutlinePath: $"res://images/relics/{GetType().Name}.png",
            // 大图标（原版256x256）
            BigIconPath: $"res://images/relics/{GetType().Name}.png"
        );

        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner == Owner)
            {
                //六谷：敏捷
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Six) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    Flash();
                    await PowerCmd.Apply<SixPower>(context, Owner.Creature, 1, Owner.Creature, null);
                    await PowerCmd.Apply<SixPower>(context, Owner.Creature, 1, Owner.Creature, null);
                }

                //滑溜
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Bc) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    DynamicVars["BcNum"].UpgradeValueBy(1);
                    DynamicVars["BcNum"].UpgradeValueBy(1);
                    if (DynamicVars["BcNum"].BaseValue >= DynamicVars["bc"].BaseValue)
                    {
                        Flash();
                        DynamicVars["BcNum"].BaseValue = 0;
                        await PowerCmd.Apply<SlipperyPower>(context, Owner.Creature, 1, Owner.Creature, null);
                    }
                }

                //赵子龙：防御
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Zzl) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    Flash();
                    await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
                    await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
                }

                //WSD：无实体
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Wsd) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    DynamicVars["WsdNum"].UpgradeValueBy(1);
                    if (DynamicVars["WsdNum"].BaseValue == DynamicVars["wsd"].BaseValue)
                    {
                        Flash();
                        await PowerCmd.Apply<IntangiblePower>(context, Owner.Creature, 1, Owner.Creature, null);
                    }
                }

                //孙悟空：荆棘
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Swk) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    DynamicVars["SwkNum"].UpgradeValueBy(1);
                    DynamicVars["SwkNum"].UpgradeValueBy(1);
                    if (DynamicVars["SwkNum"].BaseValue >= DynamicVars["swk"].BaseValue)
                    {
                        Flash();
                        DynamicVars["SwkNum"].BaseValue = 0;
                        await PowerCmd.Apply<ThornsPower>(context, Owner.Creature, 2, Owner.Creature, null);
                    }
                }

                //夏天Y：其他
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Xty) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    for (int i = 0;i < 2; i++)
                    {
                        Flash();
                        int xty = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, 5);
                        switch (xty)
                        {
                            case 0:
                                await PowerCmd.Apply<SixPower>(context, Owner.Creature, 1, Owner.Creature, null);
                                //六谷
                                break;
                            case 1:
                                DynamicVars["BcNum"].UpgradeValueBy(1);
                                if (DynamicVars["BcNum"].BaseValue >= DynamicVars["bc"].BaseValue)
                                {
                                    DynamicVars["BcNum"].BaseValue = 0;
                                    await PowerCmd.Apply<SlipperyPower>(context, Owner.Creature, 1, Owner.Creature, null);
                                }

                                //半城
                                break;
                            case 2:
                                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
                                //赵子龙
                                break;
                            case 3:
                                DynamicVars["WsdNum"].UpgradeValueBy(1);
                                if (DynamicVars["WsdNum"].BaseValue == DynamicVars["wsd"].BaseValue)
                                {
                                    Flash();
                                    await PowerCmd.Apply<IntangiblePower>(context, Owner.Creature, 1, Owner.Creature, null);
                                }

                                //王少栋
                                break;
                            case 4:
                                DynamicVars["SwkNum"].UpgradeValueBy(1);
                                if (DynamicVars["SwkNum"].BaseValue >= DynamicVars["swk"].BaseValue)
                                {
                                    DynamicVars["SwkNum"].BaseValue = 0;
                                    await PowerCmd.Apply<ThornsPower>(context, Owner.Creature, 2, Owner.Creature, null);
                                }

                                //孙悟空
                                break;
                            case 5:
                                await PowerCmd.Apply<GouPower>(context, Owner.Creature, 1, Owner.Creature, null);
                                //狗哥
                                break;
                        }
                    }
                }

                //狗哥：临时力量
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Gou) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    await PowerCmd.Apply<GouPower>(context, Owner.Creature, 1, Owner.Creature, null);
                    await PowerCmd.Apply<GouPower>(context, Owner.Creature, 1, Owner.Creature, null);
                }
                //CP禁止
                if ((cardPlay.Card.Keywords.Contains(TeamyKeyword.Six) ||
                     cardPlay.Card.Keywords.Contains(TeamyKeyword.Bc) ||
                     cardPlay.Card.Keywords.Contains(TeamyKeyword.Zzl) ||
                     cardPlay.Card.Keywords.Contains(TeamyKeyword.Wsd) ||
                     cardPlay.Card.Keywords.Contains(TeamyKeyword.Swk) ||
                     cardPlay.Card.Keywords.Contains(TeamyKeyword.Gou) ||
                     cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity)) && Owner.Creature.HasPower<CpCant>())
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int xty = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, 5);
                        switch (xty)
                        {
                            case 0:
                                await PowerCmd.Apply<SixPower>(context, Owner.Creature, 1, Owner.Creature, null);
                                //六谷
                                break;
                            case 1:
                                DynamicVars["BcNum"].UpgradeValueBy(1);
                                if (DynamicVars["BcNum"].BaseValue >= DynamicVars["bc"].BaseValue)
                                {
                                    DynamicVars["BcNum"].BaseValue = 0;
                                    await PowerCmd.Apply<SlipperyPower>(context, Owner.Creature, 1, Owner.Creature,
                                        null);
                                }

                                //半城
                                break;
                            case 2:
                                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
                                //赵子龙
                                break;
                            case 3:
                                DynamicVars["WsdNum"].UpgradeValueBy(1);
                                if (DynamicVars["WsdNum"].BaseValue== DynamicVars["wsd"].BaseValue)
                                {
                                    Flash();
                                    await PowerCmd.Apply<IntangiblePower>(context, Owner.Creature, 1, Owner.Creature, null);
                                }

                                //王少栋
                                break;
                            case 4:
                                DynamicVars["SwkNum"].UpgradeValueBy(1);
                                if (DynamicVars["SwkNum"].BaseValue >= DynamicVars["swk"].BaseValue)
                                {
                                    DynamicVars["SwkNum"].BaseValue = 0;
                                    await PowerCmd.Apply<ThornsPower>(context, Owner.Creature, 2, Owner.Creature, null);
                                }

                                //孙悟空
                                break;
                            case 5:
                                await PowerCmd.Apply<GouPower>(context, Owner.Creature, 1, Owner.Creature, null);
                                //狗哥
                                break;
                        }
                    }
                }
            }
        }

        public override Task AfterCombatEnd(CombatRoom room)
        {
            DynamicVars["BcNum"].BaseValue = 0;
            DynamicVars["WsdNum"].BaseValue = 0;
            DynamicVars["SwkNum"].BaseValue = 0;
            return base.AfterCombatEnd(room);
        }

        protected override IEnumerable<IHoverTip> AdditionalHoverTips  => [
            HoverTipFactory.FromKeyword(TeamyKeyword.Six),
            HoverTipFactory.FromKeyword(TeamyKeyword.Bc),
            HoverTipFactory.FromKeyword(TeamyKeyword.Xty),
            HoverTipFactory.FromKeyword(TeamyKeyword.Wsd),
            HoverTipFactory.FromKeyword(TeamyKeyword.Zzl),
            HoverTipFactory.FromKeyword(TeamyKeyword.Gou),
            HoverTipFactory.FromKeyword(TeamyKeyword.Swk),
            ];
    }
}
