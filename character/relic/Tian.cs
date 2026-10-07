#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.cards.common;
using Teamy.character.power;

namespace Teamy.character.relic
{

    [RegisterRelic(typeof(TeamyRelicPool))]
    [RegisterCharacterStarterRelic(typeof(Teamy))]
    [RegisterTouchOfOrobasRefinement(typeof(TianPlus))]
    public class Tian : ModRelicTemplate,IModRightClickableRelic
    {
        // 稀有度
        public override RelicRarity Rarity => RelicRarity.Starter;
        // 遗物的数值。这里会替换本地化中的{Cards}。
        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new BlockVar(1, ValueProp.Unpowered),
            ModCardVars.Int("bc", 5),
            ModCardVars.Int("swk",3 ),
            ModCardVars.Int("wsd",4 ),
            ModCardVars.Int("BcNum", 0),
            ModCardVars.Int("SwkNum",0 ),
            ModCardVars.Int("WsdNum",0 ),
        ];

        private bool mind;

        // 按玩家记录的 wsd 状态（不持久化，仅本次运行内有效）
        private static readonly Dictionary<Player, bool> WsdByPlayer = new();

        // 供其他实例调用（如 MindHealth 等）：Tian.GetWsd(player) / Tian.SetWsd(player, true)
        public static bool GetWsd(Player player) => WsdByPlayer.TryGetValue(player, out var value) && value;

        public static void SetWsd(Player player, bool value) => WsdByPlayer[player] = value;
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
                if (mind && cardPlay.Card.Keywords.Contains(TeamyKeyword.Wsd))
                {
                    //夏天Y：其他
                    if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Xty) ||
                        cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                    {
                        await Xtycmd.xty(context, cardPlay, this,Owner);
                    }
                }
                //六谷：敏捷
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Six) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    await Xtycmd.six(context, this);
                }

                //滑溜
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Bc) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    await Xtycmd.bc(context, this);
                }

                //赵子龙：防御
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Zzl) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    await Xtycmd.zzl(context, cardPlay, this);
                }

                //WSD：无实体
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Wsd) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    await Xtycmd.wsd(context, this,Owner);
                }

                //孙悟空：荆棘
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Swk) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    await Xtycmd.swk(context, this);
                }

                //夏天Y：其他
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Xty) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    await Xtycmd.xty(context, cardPlay, this,Owner);
                }

                //狗哥：临时力量
                if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Gou) ||
                    cardPlay.Card.Keywords.Contains(TeamyKeyword.Unity))
                {
                    await Xtycmd.gou(context, this);
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
                    int xty = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, 5);
                    switch (xty)
                    {
                        case 0:
                             await Xtycmd.six(context, this);
                            //六谷
                            break;
                        case 1:
                            await Xtycmd.bc(context, this);
                            //半城
                            break;
                        case 2:
                            await Xtycmd.zzl(context, cardPlay,this);
                            //赵子龙
                            break;
                        case 3:
                            await Xtycmd.wsd(context, this,Owner);
                            //王少栋
                            break;
                        case 4:
                            await Xtycmd.swk(context, this);
                            //孙悟空
                            break;
                        case 5:
                            await Xtycmd.gou(context, this);
                            //狗哥
                            break;
                    }
                }
            }
            if (cardPlay.Card is SecondBrother)
            {
                int xty = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, 5);
                switch (xty)
                {
                    case 0:
                         await Xtycmd.six(context, this);
                        //六谷
                        break;
                    case 1:
                        await Xtycmd.bc(context, this);
                        //半城
                        break;
                    case 2:
                        await Xtycmd.zzl(context, cardPlay,this);
                        //赵子龙
                        break;
                    case 3:
                        await Xtycmd.wsd(context, this,Owner);
                        //王少栋
                        break;
                    case 4:
                        await Xtycmd.swk(context, this);
                        //孙悟空
                        break;
                    case 5:
                        await Xtycmd.gou(context, this);
                        //狗哥
                        break;
                }
            }
        }
        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState.TurnNumber <= 1)
            {
                SetWsd(Owner,false);
                if (Owner.Relics.Any(r => r is MindHealth))
                {
                    mind = true;
                }
            }
        }
        // 右键执行（多人下会在所有客户端同步执行）
        public async Task OnRightClick(ModRightClickExecutionContext context)
        {
            if (GetWsd(Owner))
            {
                await PowerCmd.Apply<IntangiblePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Owner.Creature,null);
                SetWsd(Owner,false);
            }
        }
       
        // 遥测：遗物获得埋点
        public override Task AfterObtained()
        {
            if (TeamyTelemetry.IsEnabled())
            {
                TeamyTelemetry.Capture("relic_obtained", payload: new JsonObject
                {
                    ["relic"] = Id.Entry,
                });
            }
            return base.AfterObtained();
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
            HoverTipFactory.FromKeyword(TeamyKeyword.Gou),
            HoverTipFactory.FromKeyword(TeamyKeyword.Xty),
            HoverTipFactory.FromKeyword(TeamyKeyword.Wsd),
            HoverTipFactory.FromKeyword(TeamyKeyword.Zzl),
            HoverTipFactory.FromKeyword(TeamyKeyword.Swk),
            ];
    }
}
