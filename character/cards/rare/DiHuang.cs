using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.single;

namespace Teamy.character.cards.rare;

[RegisterCard(typeof(TeamyCardPool))]
public class DiHuang : ModCardTemplate
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
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
    );
    public static bool flag = true;
    // 卡牌基础数值

    public DiHuang() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    [SavedProperty]
    public bool CombatsSeen
    {
        get
        {
            return flag;
        }
        set
        {
            AssertMutable();
            flag = value;
        }
    }
    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int dihuang = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, 4);
        if (dihuang != 0)
        {
            IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(Owner).Cards.ToList();
            await CardCmd.Discard(choiceContext, enumerable);
            var powers = Owner.Creature.Powers.Where(p => p.Type == PowerType.Buff && p.Amount > 0).ToList();
            foreach (var powered in powers)
                await PowerCmd.Remove(powered);
        }
        else
        {
            IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(Owner).Cards.ToList();
            await CardCmd.Discard(choiceContext, enumerable);
            await PowerCmd.Apply<BufferPower>(choiceContext, Owner.Creature, 999, Owner.Creature, this);
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, 999, Owner.Creature, this);
            await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, 999, Owner.Creature, this);
        }
        await CardPileCmd.RemoveFromCombat(this);
        flag = false;

    }
    public override async Task AfterCombatEnd(CombatRoom _)
    {
            if (!CombatsSeen && Pile != null && Pile.Type == PileType.Deck)
            {
                await CardPileCmd.RemoveFromDeck(this);
            }
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
    ];
}