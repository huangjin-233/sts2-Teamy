#nullable enable
using System.Reflection;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;

namespace Teamy.character.single;

// 全局埋点：上报本模组卡牌的使用情况（平衡性分析用）
[RegisterSingleton]
public class TeamyTelemetrySingle : HookedSingletonModel
{
    // 本模组程序集，用于筛出属于 Teamy 的卡牌
    private static readonly Assembly ModAssembly = typeof(TeamyTelemetrySingle).Assembly;

    public TeamyTelemetrySingle() : base(HookType.Run)
    {
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel card = cardPlay.Card;
        // 只统计本模组的卡牌
        if (card.GetType().Assembly != ModAssembly || !TeamyTelemetry.IsEnabled())
        {
            return Task.CompletedTask;
        }

        IRunState runState = card.Owner.RunState;
        TeamyTelemetry.Capture("card_played", payload: new JsonObject
        {
            ["card"] = card.Id.Entry,
            ["upgraded"] = card.IsUpgraded,
            ["act"] = runState.CurrentActIndex,
            ["floor"] = runState.TotalFloor,
        });
        return Task.CompletedTask;
    }
}
