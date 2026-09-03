using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Audio;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.cards.uncommon;

 [RegisterCard(typeof(TeamyCardPool))]
    public class Cliff : ModCardTemplate
    {
        private const int energyCost = -1;
        // 卡牌类型
        private const CardType type = CardType.Attack;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Uncommon;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.AnyEnemy;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Six];
        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/attack/{GetType().Name}.png"
        );
        protected override bool HasEnergyCostX => true;
        // 卡牌基础数值
        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DamageVar(20, ValueProp.Move),
            ModCardVars.Int("Cliff",2),
            ModCardVars.Damage("Cliffdamge",6m,ValueProp.Move)
        ];
        public Cliff () : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            FmodStudioStreamingFiles.TryPlaySoundFile("res://audio/Cliff.wav");
            await Task.Delay(1000);
            var attack = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, DynamicVars["Cliff"].IntValue);
            if (attack == 0)
               await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .Targeting(cardPlay.Target!)
                    .Execute(choiceContext);
            else
                await DamageCmd.Attack(DynamicVars["Cliffdamge"].BaseValue)
                    .FromCard(this)
                    .Targeting(cardPlay.Target!)
                    .Execute(choiceContext);
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars["Cliffdamge"].UpgradeValueBy(4);
            DynamicVars.Damage.UpgradeValueBy(10);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        ];
    }