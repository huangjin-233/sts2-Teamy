using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.uncommon;

[RegisterCard(typeof(TeamyCardPool))]
public class BadAndBad : ModCardTemplate
{
    // 基础耗能
    private const int energyCost = 2;
    // 卡牌类型
    private const CardType type = CardType.Skill;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Uncommon;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.Self;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Unity];
    // 卡图资源
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
    );

    // 卡牌基础数值

    public BadAndBad () : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override List<DynamicVar> CanonicalVars =>
    [
        new MaxHpVar(3),
        ModCardVars.Int("BAD", 1),
    ];
    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (DynamicVars["BAD"].IntValue == 1)
        {
            await PowerCmd.Apply<WeakPower>(choiceContext,Owner.Creature, 99, Owner.Creature, null);
            await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.IntValue);
        }
        if (DynamicVars["BAD"].IntValue == 2)
        {
            await PowerCmd.Apply<FrailPower>(choiceContext,Owner.Creature, 99, Owner.Creature, null);
            await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.IntValue);
        }
        if (DynamicVars["BAD"].IntValue == 3)
        {
            await PowerCmd.Apply<VulnerablePower>(choiceContext,Owner.Creature, 99, Owner.Creature, null);
            await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.IntValue);
            await CardCmd.Exhaust(choiceContext, this);//消耗
            DynamicVars["BAD"].BaseValue = 1;
        }
        DynamicVars["BAD"].UpgradeValueBy(1);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.MaxHp.UpgradeValueBy(1);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<FrailPower>()
    ];
}