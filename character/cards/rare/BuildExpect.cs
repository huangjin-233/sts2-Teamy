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
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.rare
{
    [RegisterCard(typeof(TeamyCardPool))]
    public class BuildExpect : ModCardTemplate
    {
        public override bool GainsBlock => true;
        // 基础耗能
        private const int energyCost = 2;
        // 卡牌类型
        private const CardType type = CardType.Skill;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Rare;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.Self;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        // 卡图资源
        public override IEnumerable<CardKeyword> CanonicalKeywords=> [TeamyKeyword.Zzl,CardKeyword.Exhaust];
        protected override List<DynamicVar> CanonicalVars => [
            new BlockVar(16m, ValueProp.Move),
            ModCardVars.Int("Plating", 8)
        ];
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
        );

        // 卡牌基础数值

        public BuildExpect() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);//防御
            await PowerCmd.Apply<PlatingPower>(choiceContext,Owner.Creature, DynamicVars["Plating"].BaseValue, Owner.Creature, this);
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(6);
            DynamicVars["Plating"].UpgradeValueBy(3);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
            
            HoverTipFactory.FromPower<PlatingPower>()
            ];
    }
}
