using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Audio;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.rare;

[RegisterCard(typeof(TeamyCardPool))]
public class WhereIsHead : ModCardTemplate
{
    public override bool GainsBlock => true;
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
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
    );
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Xty];
    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HpLossVar(0),
        new BlockVar(0,ValueProp.Move)
    ];

    public WhereIsHead() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this)
        {
            return Task.CompletedTask;
        }
        DynamicVars.HpLoss.BaseValue = Owner.Creature.MaxHp * 2/10;
        DynamicVars.Block.BaseValue=DynamicVars.HpLoss.BaseValue;
        return Task.CompletedTask;
    }
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != base.Owner)
        {
            return Task.CompletedTask;
        }
        DynamicVars.HpLoss.BaseValue = Owner.Creature.MaxHp * 2/10;
        DynamicVars.Block.BaseValue=DynamicVars.HpLoss.BaseValue;
        return Task.CompletedTask;
    }
    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (DynamicVars.HpLoss.BaseValue >= Owner.Creature.CurrentHp)
        {
        }
        else
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue,
                ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
            await PowerCmd.Apply<Head>(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, Owner.Creature, this);
            FmodStudioStreamingFiles.TryPlaySoundFile("res://audio/Head.wav");
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay); //防御
        }
    }
    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        
    ];
}