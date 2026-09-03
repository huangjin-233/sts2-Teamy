using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.uncommon;

[RegisterCard(typeof(TeamyCardPool))]
public class Seka : ModCardTemplate
{
    // 基础耗能
    private const int energyCost = 0;
    // 卡牌类型
    private const CardType type = CardType.Attack;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Uncommon;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.AnyEnemy;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;

    // 卡图资源
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/attack/{GetType().Name}.png"
    );
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Xty];
    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(16, ValueProp.Move),
        new PowerVar<VulnerablePower>(2)
    ];

    public Seka() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .Execute(choiceContext);
        var cnm = await PowerCmd.Apply<VulnerablePower>(choiceContext,Owner.Creature,DynamicVars.Vulnerable.BaseValue ,Owner.Creature, null);
        cnm.SkipNextDurationTick = false;
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(-1);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        
        HoverTipFactory.FromPower<VulnerablePower>(),
    ];
}
