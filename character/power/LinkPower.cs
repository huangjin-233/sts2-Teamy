#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.power
{
    [RegisterPower]
    public class LinkPower : ModPowerTemplate
    {
        // 类型，Buff或Debuff
        public override PowerType Type => PowerType.Buff;
        // 叠加类型，Counter表示可叠加，Single表示不可叠加
        public override PowerStackType StackType => PowerStackType.Counter;

        // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
        public override PowerAssetProfile AssetProfile => new(
            IconPath: "res://images/powers/LinkPower.png",
            BigIconPath: "res://images/powers/LinkPower.png"
        );
        public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
        {
            foreach (Creature item in CombatState.GetTeammatesOf(Owner))
            {
                if (item.IsAlive && item.IsPlayer && item != Owner)
                {
                    await PowerCmd.Apply<Link2>(new ThrowingPlayerChoiceContext(),item, Amount, Owner, null);
                }
            }
        }
        // 抽牌后给予玩家力量
        public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (target != Owner)
            {
                return 1m;
            }
            if (!props.IsPoweredAttack())
            {
                return 1m;
            }
            return 0m;
        }
        public override async Task AfterEnergyReset(Player player)
        {
            if (player == Owner.Player)
            {
                await PowerCmd.Remove(this);
            }
        }
    }
}
