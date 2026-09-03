using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.MonsterMoves;

namespace Teamy.character.cards.common
{
    [RegisterCard(typeof(TeamyCardPool))]
    public class CNM : ModCardTemplate
    {
        public override bool GainsBlock => true;
        // 基础耗能
        private const int energyCost = 1;
        // 卡牌类型
        private const CardType type = CardType.Skill;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Common;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.AnyEnemy;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Xty,CardKeyword.Exhaust];
        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
        );

        // 卡牌基础数值
        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new BlockVar(5, ValueProp.Move),
            new DamageVar(15,ValueProp.Move)
        ];

        public CNM() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);//防御
            string original = cardPlay.Target.Monster.MoveStateMachine.StateLog.Last<MonsterState>().Id;
            var cnmIntent = new SingleAttackIntent(9);
            if (!cardPlay.Target.Monster.IntendsToAttack)
            {
                var cnmMove = new MoveState(
                    "HA_ATTACK",
                    async target =>
                    {
                        var players = Owner.Creature.CombatState?.Players.Select(p => p.Creature).ToList();
                        if (players != null && players.Any())
                            await CreatureCmd.Damage(choiceContext, players, 9, ValueProp.Move, cardPlay.Target, null);
                    },
                    cnmIntent
                )
                {
                    FollowUpStateId = original
                };
                    if (cardPlay.Target.IsAlive && cardPlay.Target.Monster !=null)
                        cardPlay.Target.Monster.SetMoveImmediate(cnmMove,true);
            }
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(2);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        ];
    }
}
