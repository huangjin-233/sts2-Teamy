using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;
using STS2RitsuLib.Models.Capabilities;
using Teamy.character.cards.common;
using Teamy.character.cards.rare;
using Teamy.character.key;
using Teamy.character.power;

namespace Teamy.character.single;

[RegisterSingleton]
public class ReplaceSingle : HookedSingletonModel
{
    // 填入HookType，可选Combat、Run或者None。Combat涉及战斗接口，Run涉及全局接口，具体看Hook类中的定义。
    public ReplaceSingle() : base(HookType.Combat)
    {
    }
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Replace))
        {
            await CardPileCmd.Draw(choiceContext, 1,cardPlay.Card.Owner);
            IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(
                cardPlay.Card.Owner,
                from c in cardPlay.Card.Owner.Creature.Player.Character.CardPool.GetUnlockedCards(
                    cardPlay.Card.Owner.Creature.Player.UnlockState,
                    cardPlay.Card.Owner.Creature.CombatState.RunState.CardMultiplayerConstraint)
                select c,
                1,
                cardPlay.Card.Owner.Creature.CombatState.RunState.Rng.CombatCardGeneration).ToList();
            foreach (CardModel card in distinctForCombat)
            {
                if (card == null) continue;
                if (cardPlay.Card.IsUpgraded)
                {
                    CardCmd.Upgrade(card);
                }
                CardCmd.PreviewCardPileAdd(
                    await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Discard, cardPlay.Card.Owner));
                await Cmd.Wait(0.5f);
                card.AddKeyword(TeamyKeyword.Replace);
                card.EnergyCost.SetThisCombat(1);
                await CardPileCmd.RemoveFromCombat(cardPlay.Card);
            }
        }
    }
}