#nullable enable
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.cards.rare;

namespace Teamy.character.power;

[RegisterPower]
public class GreenKillerPower: ModPowerTemplate
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;
    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;

    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://images/powers/GreenKillerPower.png",
        BigIconPath: "res://images/powers/GreenKillerPower.png"
    );

    public static bool kill=true;
    private Vector2 spawnPos;
    public override async Task AfterEnergyReset(Player player)
    {
        if (player == Owner.Player)
        {
            var ownerCombatState = Owner.CombatState;
            if (ownerCombatState != null)
            {
                Creature? creature = Owner.Player.RunState.Rng.CombatTargets.NextItem(ownerCombatState.HittableEnemies);
                if (creature != null)
                {
                    NCreature? ownerNode = NCombatRoom.Instance?.GetCreatureNode(Owner);
                    if (Fps.fps != null && !kill)
                    {
                        Fps.fps.QueueFree();
                        kill = true;
                    }
                    Fps.fps = null;
                    if (ownerNode != null)
                    {
                        spawnPos = ownerNode.VfxSpawnPosition;
                    }
                    Fps.fps = TeamyVfx.PlaySimple("res://scenes/vfx/kill.tscn", spawnPos,1.5f);
                    await Task.Delay(1200);
                    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), creature, Amount, ValueProp.Unpowered, Owner);
                    if (Owner.Player.Creature.HasPower<FpsPower>())
                    {
                        Fps.fps = TeamyVfx.Play("res://scenes/vfx/fps.tscn", spawnPos);
                        kill = false;
                    }
                }
            }
        }
    }
}