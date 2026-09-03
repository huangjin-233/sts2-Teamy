using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.key;

namespace Teamy.character.cards.ancient;

[RegisterCard(typeof(TeamyCardPool))]
public class BossPlaus : ModCardTemplate
{
    public override bool GainsBlock => true;
    // 基础耗能
    private const int energyCost = 1;
    // 卡牌类型
    private const CardType type = CardType.Skill;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Ancient;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.Self;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;

    // 卡图资源
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/ancient/{GetType().Name}.png"
    );
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Xty];
    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(13, ValueProp.Move),
        new CardsVar(1)
    ];

    public BossPlaus() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    { 
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);//防御
        foreach (CardModel cards in PileType.Hand.GetPile(Owner).Cards)
            if (cards != null&& !cards.Keywords.Contains(TeamyKeyword.Xty)
                             && !cards.Keywords.Contains(TeamyKeyword.Zzl)
                             && !cards.Keywords.Contains(TeamyKeyword.Six)
                             && !cards.Keywords.Contains(TeamyKeyword.Swk)
                             && !cards.Keywords.Contains(TeamyKeyword.Bc)
                             && !cards.Keywords.Contains(TeamyKeyword.Wsd)
                             && !cards.Keywords.Contains(TeamyKeyword.Gou))
            {
                for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
                {
                    int keyword = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, 7);
                    while (cards.Keywords.Contains(TeamyKeyword.Xty) && keyword == 0
                           || cards.Keywords.Contains(TeamyKeyword.Zzl) && keyword == 1
                           || cards.Keywords.Contains(TeamyKeyword.Six) && keyword == 2
                           || cards.Keywords.Contains(TeamyKeyword.Swk) && keyword == 3
                           || cards.Keywords.Contains(TeamyKeyword.Bc) && keyword == 4
                           || cards.Keywords.Contains(TeamyKeyword.Wsd) && keyword == 5
                           || cards.Keywords.Contains(TeamyKeyword.Gou) && keyword == 6)
                    {
                        keyword = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, 7);
                    }
                    switch (keyword)
                    {
                        case 0:
                            CardCmd.ApplyKeyword(cards, TeamyKeyword.Xty);
                            cards.GetOrCreateCapability<Xtykey>(); // 挂载组件
                            break;
                        case 1:
                            CardCmd.ApplyKeyword(cards, TeamyKeyword.Zzl);
                            cards.GetOrCreateCapability<Zzlkey>(); // 挂载组件
                            break;
                        case 2:
                            CardCmd.ApplyKeyword(cards, TeamyKeyword.Six);
                            cards.GetOrCreateCapability<Sixkey>(); // 挂载组件
                            break;
                        case 3:
                            CardCmd.ApplyKeyword(cards, TeamyKeyword.Swk);
                            cards.GetOrCreateCapability<Swkkey>(); // 挂载组件
                            break;
                        case 4:
                            CardCmd.ApplyKeyword(cards, TeamyKeyword.Bc);
                            cards.GetOrCreateCapability<Bckey>(); // 挂载组件
                            break;
                        case 5:
                            CardCmd.ApplyKeyword(cards, TeamyKeyword.Wsd);
                            cards.GetOrCreateCapability<Wsdkey>(); // 挂载组件
                            break;
                        case 6:
                            CardCmd.ApplyKeyword(cards, TeamyKeyword.Gou);
                            cards.GetOrCreateCapability<Goukey>(); // 挂载组件
                            break;
                    }
                }
            }
    }
    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Block.UpgradeValueBy(5);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        
    ];
}