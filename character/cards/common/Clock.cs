using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.MonsterMoves;
using Teamy.character.power;

namespace Teamy.character.cards.common
{
    [RegisterCard(typeof(TeamyCardPool))]
    public class Clock : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = -1;
        // 卡牌类型
        private const CardType type = CardType.Power;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Common;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.Self;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        public override IEnumerable<CardKeyword> CanonicalKeywords => [];
        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/power/{GetType().Name}.png"
        );

        // 卡牌基础数值
        protected override IEnumerable<DynamicVar> CanonicalVars => [
        ];

        protected override bool IsPlayable => false;
        public Clock() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }
        public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
        {
            return true;
        }
        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ShouldPlay(cardPlay.Card, default);
        }

        public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
        {
            if (!card.ShouldPlay(this, default))
                await CardCmd.Exhaust(choiceContext,card);
            ShouldPlay(this, default);
        }

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature applier,
            CardModel cardSource)
        {
            if (!power.ShouldPlay(this,default) && power.Target == Owner.Creature)
                await PowerCmd.Remove(power);
            ShouldPlay(this, default);
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        ];
    }
}
