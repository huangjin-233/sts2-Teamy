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
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.key;

namespace Teamy.character.cards.rare;

[RegisterCard(typeof(TeamyCardPool))]
public class Teacher : ModCardTemplate
{
    // 基础耗能
    private const int energyCost = 4;
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
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,CardKeyword.Ethereal];
    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1)
    ];
    
    public Teacher() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
        
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> card = new List<CardModel>();
        List<CardModel> card1 = PileType.Deck.GetPile(Owner).Cards.Where(c => c.IsUpgradable).ToList();
       
        card.AddRange(card1);
        CardModel cards = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: choiceContext, player: Owner, filter: delegate(CardModel c)
        {
            return !c.Keywords.Contains(TeamyKeyword.Xty) ? true : false;
        }, source: this)).FirstOrDefault();
            if (cards != null)
            {
                CardCmd.Upgrade(cards);
                if (cards.Keywords.Contains(TeamyKeyword.Xty))
                { 
                    CardCmd.ApplyKeyword(cards, TeamyKeyword.Xty);
                    cards.GetOrCreateCapability<Xtykey>(); // 挂载组件
                }
                CardModel deckCard = card1.FirstOrDefault(c => c.Id == cards.Id);
                if (deckCard != null) CardCmd.Upgrade(deckCard);
            }
            
    }
    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TeamyKeyword.Xty)
    ];
}