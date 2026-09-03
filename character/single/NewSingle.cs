using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;
using Teamy.character.cards.rare;
using Teamy.character.power;

namespace Teamy.character.single;

[RegisterSingleton]
public class NewSingle : HookedSingletonModel
{
    // 填入HookType，可选Combat、Run或者None。Combat涉及战斗接口，Run涉及全局接口，具体看Hook类中的定义。
    public NewSingle() : base(HookType.Run)
    {
    }

    public static bool flag;
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
       if (cardPlay.Card.Owner.Creature.HasPower<NewPower>())
           CardCmd.ApplyKeyword( cardPlay.Card,TeamyKeyword.Replace);
    }
}