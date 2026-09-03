using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.uncommon;

 [RegisterCard(typeof(TeamyCardPool))]
    public class SmallEye : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 2;
        // 卡牌类型
        private const CardType type = CardType.Power;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Uncommon;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.Self;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Six];
        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/power/{GetType().Name}.png"
        );
        protected override List<DynamicVar> CanonicalVars => [
            new PowerVar<StrengthPower>(5)
        ];
        // 卡牌基础数值

        public SmallEye() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext,Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<SmallEyePower>(choiceContext,Owner.Creature, 1, Owner.Creature, null);
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
           EnergyCost.UpgradeBy(-1);
           DynamicVars.Strength.UpgradeValueBy(1);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
            HoverTipFactory.FromPower<StrengthPower>()
        ];
    }