using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.key;
using Teamy.character.power;

namespace Teamy.character.cards.common
{
    [RegisterCard(typeof(TeamyCardPool))]
    public class EggFen : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 0;
        // 卡牌类型
        private const CardType type = CardType.Skill;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Common;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.Self;
        public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Gou];
        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
        );

        // 卡牌基础数值
        protected override IEnumerable<DynamicVar> CanonicalVars => [
          new PowerVar<StrengthPower>(2)
        ];

        public EggFen() : base(energyCost, type, rarity, targetType)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<GouPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.Strength.UpgradeValueBy(3);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        ];
    }
}
