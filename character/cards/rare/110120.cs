using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.cards.special;
using Teamy.character.power;

namespace Teamy.character.cards.rare;

[RegisterCard(typeof(TeamyCardPool))]
public class _110120 : ModCardTemplate
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
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];


    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
    );

    // 卡牌基础数值

    public _110120() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(
            Owner,
            from c in Owner.Creature.Player.Character.CardPool.GetUnlockedCards(
                Owner.Creature.Player.UnlockState,
                Owner.Creature.CombatState.RunState.CardMultiplayerConstraint)
            where c.Keywords.Contains(TeamyKeyword.Unity)
            select c,
            1,
            Owner.Creature.CombatState.RunState.Rng.CombatCardGeneration).ToList();
        foreach (CardModel card in distinctForCombat)
        {
            card.SetToFreeThisCombat();
            card.AddKeyword(CardKeyword.Exhaust);
            card.AddKeyword(CardKeyword.Ethereal);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card);
            }
        }

        await CardPileCmd.AddGeneratedCardsToCombat(distinctForCombat, PileType.Hand, Owner);
    }
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            IEnumerable<CardModel> card1 =
                await TiaoCao.CreateInHand(Owner, 1, CombatState); //添加手牌
        }
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromCard<TiaoCao>()
    ];
}