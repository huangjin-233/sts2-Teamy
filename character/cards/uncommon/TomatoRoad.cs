using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.cards.uncommon

{
    [RegisterCard(typeof(TeamyCardPool))]
    public class TomatoRoad : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 1;

        // 卡牌类型
        private const CardType type = CardType.Skill;

        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Uncommon;

        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.Self;

        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;

        // 卡图资源
        public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Xty, CardKeyword.Exhaust];

        protected override List<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1)
        ]; // 动态变量

        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
        );

        // 卡牌基础数值
        public TomatoRoad() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            IEnumerable<CardModel> card =
                await Tomato.CreateInHand(Owner, DynamicVars.Cards.IntValue, CombatState); //添加手牌
            if (IsUpgraded)
            {
                foreach (CardModel item in card) 
                    CardCmd.Upgrade(item);
            }
        }

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            
            HoverTipFactory.FromCard<Tomato>(IsUpgraded),
        ];
    }
}