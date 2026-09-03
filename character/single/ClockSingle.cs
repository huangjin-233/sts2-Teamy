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
public class ClockSingle : HookedSingletonModel
{
    // 填入HookType，可选Combat、Run或者None。Combat涉及战斗接口，Run涉及全局接口，具体看Hook类中的定义。
    public ClockSingle() : base(HookType.Combat)
    {
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card is Clock)
        {
            foreach (var power in card.Owner.Creature.Powers )
            {
                if (!power.ShouldPlay(card,default))
                    await PowerCmd.Remove(power);
            }

            foreach (var cards in PileType.Hand.GetPile(card.Owner).Cards )
            {
                if (!cards.ShouldPlay(card, default))
                    await CardCmd.Exhaust(choiceContext,cards);
            }
            ShouldPlay(card, default);
        }
    }
}