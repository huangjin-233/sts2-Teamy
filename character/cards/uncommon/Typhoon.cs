using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.common;

 [RegisterCard(typeof(TeamyCardPool))]
    public class Typhoon : ModCardTemplate
    {
        public override bool GainsBlock => true;
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
        public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Bc];
        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
        );
        protected override List<DynamicVar> CanonicalVars => [
            new PowerVar<DexterityPower>(4),
            new CardsVar(3)
        ];
        // 卡牌基础数值

        public Typhoon () : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<TyphoonPower>(choiceContext,Owner.Creature, DynamicVars.Dexterity.BaseValue, Owner.Creature, null);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);//抽卡
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
           DynamicVars.Dexterity.UpgradeValueBy(-2);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        ];
    }