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
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.cards.uncommon
{
    [RegisterCard(typeof(TeamyCardPool))]
    public class BadPizza : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 0;
        // 卡牌类型
        private const CardType type = CardType.Skill;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Uncommon;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.Self;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        // 卡图资源
        public override IEnumerable<CardKeyword> CanonicalKeywords=> [TeamyKeyword.Zzl,TeamyKeyword.Six];
        protected override List<DynamicVar> CanonicalVars => [
            new EnergyVar(3),
            new PowerVar<PoisonPower>(3),
            new PowerVar<WeakPower>(1),
            new PowerVar<VulnerablePower>(1)
            ];// 动态变量

        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
        );

        // 卡牌基础数值

        public BadPizza() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);//能量
            await PowerCmd.Apply<PoisonPower>(choiceContext,Owner.Creature,DynamicVars.Poison.BaseValue , Owner.Creature, this);
            var weak = await PowerCmd.Apply<WeakPower>(choiceContext,Owner.Creature, DynamicVars.Weak.BaseValue, Owner.Creature, this);
            var vulner = await PowerCmd.Apply<VulnerablePower>(choiceContext,Owner.Creature, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
            weak.SkipNextDurationTick = false;
            vulner.SkipNextDurationTick = false;
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.Poison.UpgradeValueBy(-1);
            DynamicVars.Weak.UpgradeValueBy(-1);
            DynamicVars.Vulnerable.UpgradeValueBy(-1);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
            HoverTipFactory.FromPower<PoisonPower>(),
            HoverTipFactory.FromPower<WeakPower>(),
            HoverTipFactory.FromPower<VulnerablePower>(),
    ];
    }
}
