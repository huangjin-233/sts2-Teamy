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
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.rare;

[RegisterCard(typeof(TeamyCardPool))]
public class SixCat : ModCardTemplate
{
    // 基础耗能
    private const int energyCost = 0;
    // 卡牌类型
    private const CardType type = CardType.Skill;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Rare;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.Self;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;
    // 卡图资源
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Replace,CardKeyword.Exhaust];


    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
    );

    // 卡牌基础数值
    protected override List<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];

    public SixCat() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> distinctForCombat= CardFactory.GetDistinctForCombat(
            Owner, 
            from c in Owner.Creature.Player.Character.CardPool.GetUnlockedCards(
                Owner.Creature.Player.UnlockState, 
                Owner.Creature.CombatState.RunState.CardMultiplayerConstraint)
            select c,
            DynamicVars.Cards.IntValue,
            Owner.Creature.CombatState.RunState.Rng.CombatCardGeneration).ToList();
        foreach(CardModel card in distinctForCombat)
        {
            card.SetToFreeThisCombat();
            card.AddKeyword(CardKeyword.Exhaust);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card);
            }
        }
        await CardPileCmd.AddGeneratedCardsToCombat(distinctForCombat, PileType.Hand, Owner);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
    ];
}