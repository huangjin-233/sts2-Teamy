using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.power;

[RegisterPower]
public class Tiger : ModPowerTemplate
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;
    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;

    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://images/powers/Tiger.png",
        BigIconPath: "res://images/powers/Tiger.png"
    );

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            IReadOnlyList<CardModel> cards = CardFactory.GetDistinctForCombat(
                Owner.Player,
                from c in Owner.Player.Character.CardPool.GetUnlockedCards(
                    Owner.Player.UnlockState,
                    Owner.CombatState.RunState.CardMultiplayerConstraint)
                where c.Rarity == CardRarity.Rare
                select c,
                26,
                Owner.CombatState.RunState.Rng.CombatCardGeneration).ToList();
            foreach (CardModel tiger in await CardSelectCmd.FromSimpleGrid(choiceContext, cards,
                         Owner.Player, new CardSelectorPrefs(RelicModel.L10NLookup("从稀有牌中选择一张牌加入手卡"), 1)))
            {
                tiger.SetToFreeThisCombat();
                tiger.AddKeyword(CardKeyword.Exhaust);
                await CardPileCmd.Add(tiger, PileType.Hand);
            }

            await PowerCmd.Remove(this);
        }
    }
}