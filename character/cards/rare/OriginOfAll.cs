using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.key;
using Teamy.character.power;

namespace Teamy.character.cards.rare;

[RegisterCard(typeof(TeamyCardPool))]
public class OriginOfAll : ModCardTemplate
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
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Gou];
    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(6, ValueProp.Unpowered)
    ];

    public OriginOfAll() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    { 
        await PowerCmd.Apply<GouPower>(choiceContext,Owner.Creature, 3, Owner.Creature, null);
        await PowerCmd.Apply<DexterityPower>(choiceContext,Owner.Creature, 1, Owner.Creature, null);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        foreach (CardModel cards in PileType.Hand.GetPile(base.Owner).Cards)
        {
            if (cards.Keywords.Contains(TeamyKeyword.Unity))
            {
            }
            else
            {
                CardCmd.ApplyKeyword(cards, TeamyKeyword.Unity);
                cards.GetOrCreateCapability<Unitykey>(); // 挂载组件
            }
        }

    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>(),
        HoverTipFactory.FromKeyword(TeamyKeyword.Unity)
    ];
}
