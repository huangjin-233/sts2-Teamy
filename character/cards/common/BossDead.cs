using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Audio;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.key;
using Teamy.character.power;

namespace Teamy.character.cards.common
{
    [RegisterCard(typeof(TeamyCardPool))]
    public class BossDead : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 1;
        // 卡牌类型
        private const CardType type = CardType.Attack;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Common;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.AnyEnemy;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Swk];
        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/attack/{GetType().Name}.png"
        );
        public bool keyword;
        // 卡牌基础数值
        protected override IEnumerable<DynamicVar> CanonicalVars => [
          new DamageVar(9m,ValueProp.Move)
        ];

        public BossDead() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            FmodStudioStreamingFiles.TryPlaySoundFile("res://audio/BossDead.wav");
            await Task.Delay(1000);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target!)
                .Execute(choiceContext);
            CardPile pile = PileType.Hand.GetPile(Owner);
            foreach (var cards in PileType.Hand.GetPile(Owner).Cards)
            {
                if (!cards.Keywords.Contains(TeamyKeyword.Xty))
                    keyword = true;
            }
            if (keyword)
            {
                CardModel card = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
                while (card.Keywords.Contains(TeamyKeyword.Xty))
                {
                    card = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
                }

                CardCmd.ApplyKeyword(card, TeamyKeyword.Xty);
                card.GetOrCreateCapability<Xtykey>(); // 挂载组件}
            }
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
            HoverTipFactory.FromKeyword(TeamyKeyword.Xty)
        ];
    }
}
