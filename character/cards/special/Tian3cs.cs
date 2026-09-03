using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.cards.rare;

namespace Teamy.character.cards.special;

[RegisterCard(typeof(TokenCardPool))]
public class Tian3 : ModCardTemplate
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
    // 卡图资源
    public override IEnumerable<CardKeyword> CanonicalKeywords=> [TeamyKeyword.Xty];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/attack/{GetType().Name}.png"
    );
    protected override List<DynamicVar> CanonicalVars => [
        new DamageVar(30m,ValueProp.Move),
        new PowerVar<VulnerablePower>(3),
       new PowerVar<WeakPower>(3)
    ];// 动态变量
    // 卡牌基础数值

    public Tian3() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .Execute(choiceContext);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars.Weak.BaseValue, Owner.Creature, this);
        CardModel card = CombatState.CreateCard<Tian1>(Owner);
        if (IsUpgraded)
        {
            CardCmd.Upgrade(card);
        }
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Discard, Owner));
        await Cmd.Wait(0.5f);
        await CardPileCmd.RemoveFromCombat(this);
    }
    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(9);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        
        HoverTipFactory.FromCard<Tian1>(IsUpgraded)
    ];
}