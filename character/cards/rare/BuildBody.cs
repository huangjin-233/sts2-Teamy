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
using STS2RitsuLib.Audio;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.rare
{
    [RegisterCard(typeof(TeamyCardPool))]
    public class BuildBody : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 1;
        // 卡牌类型
        private const CardType type = CardType.Skill;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Rare;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.Self;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        // 卡图资源
        public override IEnumerable<CardKeyword> CanonicalKeywords=> [
            TeamyKeyword.Unity,
            ];
        protected override List<DynamicVar> CanonicalVars => [
                new MaxHpVar(3m)
                   ];// 动态变量
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
        );

        // 卡牌基础数值

        public BuildBody() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            FmodStudioStreamingFiles.TryPlaySoundFile("res://audio/Build.wav");
            int h = (int)(Owner.Creature.CurrentHp * 20 / 100m);
            await CreatureCmd.Damage(choiceContext, Owner.Creature, h, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
            await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.IntValue);
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
