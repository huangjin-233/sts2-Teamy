using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards;

[RegisterCard(typeof(QuestCardPool))]
public class Six : ModCardTemplate
{
    // 基础耗能
    private const int energyCost = -2;

    // 卡牌类型
    private const CardType type = CardType.Power;

    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Quest;

    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.Self;

    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = false;
    // 卡图资源


    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://images/cards/power/Fps.png"
    );

    // 卡牌基础数值

    public Six() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
}