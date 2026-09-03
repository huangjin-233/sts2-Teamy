using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.cards.uncommon;

 [RegisterCard(typeof(TeamyCardPool))]
    public class IcuCapmion : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 1;
        // 卡牌类型
        private const CardType type = CardType.Skill;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Uncommon;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.AnyEnemy;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Bc];
        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
        );

        // 卡牌基础数值
        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DamageVar(7m,ValueProp.Move)
        ];

        public IcuCapmion() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var conut = Owner.Creature.Powers.Where(p => p.Type == PowerType.Buff && p.Amount > 0).Count();
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(conut).FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }
        
        
        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        ];
    }