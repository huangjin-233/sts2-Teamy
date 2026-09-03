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

namespace Teamy.character.cards.rare;
    
    [RegisterCard(typeof(TeamyCardPool))]
    public class LKL : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 1;
        // 卡牌类型
        private const CardType type = CardType.Power;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Rare;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.Self;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;

        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/power/{GetType().Name}.png"
        );
        public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Xty];
        // 卡牌基础数值
        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new HpLossVar(5)
        ];

        public LKL() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PowerCmd.Apply<LklPower>(choiceContext,Owner.Creature, 1, Owner.Creature, null);
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.HpLoss.UpgradeValueBy(-2);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
            
            HoverTipFactory.FromPower<WeakPower>(),
            HoverTipFactory.FromPower<VulnerablePower>(),
            HoverTipFactory.FromPower<PoisonPower>()
        ];
    }
