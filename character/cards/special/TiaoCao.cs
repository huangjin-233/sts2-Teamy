using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.cards.special;

[RegisterCard(typeof(TokenCardPool))]
public class TiaoCao : ModCardTemplate
{
    // 基础耗能
    private const int energyCost = 0;

    // 卡牌类型
    private const CardType type = CardType.Skill;

    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Token;

    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.Self;

    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;
    // 卡图资源
    public override IEnumerable<CardKeyword> CanonicalKeywords=> [CardKeyword.Exhaust];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
    );
    protected override List<DynamicVar> CanonicalVars => [
    ];// 动态变量
    // 卡牌基础数值

    public TiaoCao() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        List<CardModel> list = PileType.Hand.GetPile(Owner).Cards.Where(c => c.IsTransformable && c.Type == CardType.Status).ToList();
        foreach (CardModel item in list)
        {
            await CardCmd.Exhaust(choiceContext,item);
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
            await CardPileCmd.Draw(choiceContext,1 ,Owner);
        }
    }
    public static async Task<CardModel> CreateInHand(Player owner, ICombatState combatState)
    {
        return (await CreateInHand(owner, 1, combatState)).FirstOrDefault();
    }
    public static async Task<IEnumerable<CardModel>> CreateInHand(Player owner, int count, ICombatState combatState)
    {
        if (count == 0)
        {
            return Array.Empty<CardModel>();
        }
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return Array.Empty<CardModel>();
        }
        List<CardModel> TiaoCao = new List<CardModel>();
        for (int i = 0; i < count; i++)
        {
            TiaoCao.Add(combatState.CreateCard<TiaoCao>(owner));
        }
        await CardPileCmd.AddGeneratedCardsToCombat(TiaoCao, PileType.Hand, owner);
        return TiaoCao;
    }
    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
    }
}