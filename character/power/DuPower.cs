using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.power
{
    [RegisterPower]
    public class DuPower : ModPowerTemplate
    {
        // 类型，Buff或Debuff
        public override PowerType Type => PowerType.Buff;
        // 叠加类型，Counter表示可叠加，Single表示不可叠加
        public override PowerStackType StackType => PowerStackType.Counter;

        // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
        public override PowerAssetProfile AssetProfile => new(
            IconPath: "res://images/powers/DuPower.png",
            BigIconPath: "res://images/powers/DuPower.png"
        );
        // 抽牌后给予玩家力量
        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int du = Owner.Player.RunState.Rng.CombatCardSelection.NextInt(0, 3);
            switch (du)
            {
                case 1:await PlayerCmd.GainGold(Amount, Owner.Player);
                    break;
                case 2:await PlayerCmd.LoseGold(Amount, Owner.Player);
                    if (Owner.Player.Gold <= Amount)
                    {
                        await CreatureCmd.LoseMaxHp(choiceContext, Owner.Player.Creature, 20, false);
                        await PlayerCmd.GainGold(100, Owner.Player);
                    }
                    break;
                case 3:
                    break;
            }
        }
    }
}
