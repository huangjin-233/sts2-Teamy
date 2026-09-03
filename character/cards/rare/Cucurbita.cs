using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.cards.rare;

[RegisterCard(typeof(TeamyCardPool))]
public class Cucurbita : ModCardTemplate
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
    public override IEnumerable<CardKeyword> CanonicalKeywords=> [TeamyKeyword.Xty];
    protected override List<DynamicVar> CanonicalVars => [
        new HpLossVar(5m),
        new DamageVar(0m,ValueProp.Move)
    ];
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/attack/{GetType().Name}.png"
    );

    // 卡牌基础数值

    public Cucurbita() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue,  ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,this);
        DynamicVars.Damage.BaseValue = Owner.Creature.MaxHp - Owner.Creature.CurrentHp;
        NCreature? ownerNode = NCombatRoom.Instance?.GetCreatureNode(cardPlay.Target);
        if (ownerNode != null)
        {
            Vector2 spawnPos = ownerNode.VfxSpawnPosition;
            TeamyVfx.PlaySimple("res://scenes/vfx/wogua.tscn",spawnPos,1.2f);
            await Task.Delay(800);
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .Execute(choiceContext);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        
    ];
}