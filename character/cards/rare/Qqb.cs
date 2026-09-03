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
public class Qqb : ModCardTemplate
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
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Xty, CardKeyword.Exhaust];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
    );

    // 卡牌基础数值
    public Qqb() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int strength =Owner.Creature.GetPowerAmount<StrengthPower>();
        int dexterity = Owner.Creature.GetPowerAmount<DexterityPower>();
        await PowerCmd.Apply<StrengthPower>(choiceContext,Owner.Creature, -strength, Owner.Creature, null);
        await PowerCmd.Apply<DexterityPower>(choiceContext,Owner.Creature, -dexterity, Owner.Creature, null);
        await PowerCmd.Apply<StrengthPower>(choiceContext,Owner.Creature,dexterity, Owner.Creature, null);
        await PowerCmd.Apply<DexterityPower>(choiceContext,Owner.Creature, strength, Owner.Creature, null);
        await PowerCmd.Remove<GouPower>(Owner.Creature);
        await PowerCmd.Remove<SixPower>(Owner.Creature);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>()
    ];
}