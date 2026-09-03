using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.rare;

[RegisterCard(typeof(TeamyCardPool))]
public class LifeAndDeath : ModCardTemplate
{
    // 基础耗能
    private const int energyCost = 1;
    // 卡牌类型
    private const CardType type = CardType.Attack;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Rare;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.AnyEnemy;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;
    // 卡图资源
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Xty,CardKeyword.Exhaust];


    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/attack/{GetType().Name}.png"
    );

    // 卡牌基础数值
    protected override List<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10m,ValueProp.Move)
    ];

    public LifeAndDeath() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await CreatureCmd.Heal(Owner.Creature, attackCommand.Results.SelectMany(r => r).Sum(r => r.TotalDamage + r.OverkillDamage));
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
       DynamicVars.Damage.UpgradeValueBy(4);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        
    ];
}