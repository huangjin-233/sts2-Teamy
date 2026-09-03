using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.uncommon;

[RegisterCard(typeof(TeamyCardPool))]
public class Run : ModCardTemplate
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
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Xty,CardKeyword.Exhaust];
    protected override List<DynamicVar> CanonicalVars => [
        new CardsVar(1)
    ];// 动态变量

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
    );
    // 卡牌基础数值
    public Run() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int cards = Owner.RunState.Rng.CombatCardGeneration.NextInt(DynamicVars.Cards.IntValue, 3);
        await CardPileCmd.Draw(choiceContext, cards, Owner);//抽卡
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
         
    ];
}