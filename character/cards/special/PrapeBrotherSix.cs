using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Audio;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.cards.special;

 [RegisterCard(typeof(TokenCardPool))]
    public class PrapeBrotherSix : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 0;
        // 卡牌类型
        private const CardType type = CardType.Attack;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Token;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.AnyEnemy;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Six,TeamyKeyword.Bc];
        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/attack/PrapeBrother.png"
        );

        // 卡牌基础数值
        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DamageVar(8, ValueProp.Move),
            new PowerVar<StrengthPower>(2)
        ];

        public PrapeBrotherSix() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            FmodStudioStreamingFiles.TryPlaySoundFile("res://audio/Grape.wav");
            await Task.Delay(1000);
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.IntValue, Owner.Creature, this);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target!)
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