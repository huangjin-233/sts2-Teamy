using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.key;
using Teamy.character.power;

namespace Teamy.character.cards.basic;

[RegisterCard(typeof(TeamyCardPool))]
[RegisterCharacterStarterCard(typeof(Teamy))]
public class Boss : ModCardTemplate
{
    public override bool GainsBlock => true;
    // 基础耗能
    private const int energyCost = 1;
    // 卡牌类型
    private const CardType type = CardType.Skill;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Basic;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.Self;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;

    // 卡图资源
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/basic/{GetType().Name}.png"
    );
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TeamyKeyword.Xty];
    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(8, ValueProp.Move),
        new CardsVar(1)
    ];

    public Boss() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    { 
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);//防御
        foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Cards.IntValue), null, this)) 
            if (card != null && (!card.Keywords.Contains(TeamyKeyword.Xty)
                                || !card.Keywords.Contains(TeamyKeyword.Zzl)
                                || !card.Keywords.Contains(TeamyKeyword.Six)
                                || !card.Keywords.Contains(TeamyKeyword.Swk)
                                || !card.Keywords.Contains(TeamyKeyword.Bc)
                                || !card.Keywords.Contains(TeamyKeyword.Wsd)
                                || !card.Keywords.Contains(TeamyKeyword.Gou)))
            {
                int keyword = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, 7);
                while (card.Keywords.Contains(TeamyKeyword.Xty) && keyword == 0
                    || card.Keywords.Contains(TeamyKeyword.Zzl) && keyword == 1
                    || card.Keywords.Contains(TeamyKeyword.Six) && keyword == 2
                    || card.Keywords.Contains(TeamyKeyword.Swk) && keyword == 3
                    || card.Keywords.Contains(TeamyKeyword.Bc) && keyword == 4
                    || card.Keywords.Contains(TeamyKeyword.Wsd) && keyword == 5
                    || card.Keywords.Contains(TeamyKeyword.Gou) && keyword == 6)
                {
                    keyword = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, 7);
                }
                switch (keyword)
                {
                    case 0:
                        CardCmd.ApplyKeyword(card, TeamyKeyword.Xty);
                        card.GetOrCreateCapability<Xtykey>(); // 挂载组件
                        break;
                    case 1:
                        CardCmd.ApplyKeyword(card, TeamyKeyword.Zzl);
                        card.GetOrCreateCapability<Zzlkey>(); // 挂载组件
                        break;
                    case 2:
                        CardCmd.ApplyKeyword(card, TeamyKeyword.Six);
                        card.GetOrCreateCapability<Sixkey>(); // 挂载组件
                        break;
                    case 3:
                        CardCmd.ApplyKeyword(card, TeamyKeyword.Swk);
                        card.GetOrCreateCapability<Swkkey>(); // 挂载组件
                        break;
                    case 4:
                        CardCmd.ApplyKeyword(card, TeamyKeyword.Bc);
                        card.GetOrCreateCapability<Bckey>(); // 挂载组件
                        break;
                    case 5:
                        CardCmd.ApplyKeyword(card, TeamyKeyword.Wsd);
                        card.GetOrCreateCapability<Wsdkey>(); // 挂载组件
                        break;
                    case 6:
                        CardCmd.ApplyKeyword(card, TeamyKeyword.Gou);
                        card.GetOrCreateCapability<Goukey>(); // 挂载组件
                        break;
                }
            }
    }
    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Block.UpgradeValueBy(3);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TeamyKeyword.Unity)
    ];
}