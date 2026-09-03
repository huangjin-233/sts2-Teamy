#nullable enable
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.relic;

namespace Teamy.character.power;

[RegisterPower]
public class Bump: ModPowerTemplate
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;
    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;

    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://images/powers/Bump.png",
        BigIconPath: "res://images/powers/Bump.png"
    );
    public override async Task AfterEnergyReset(Player player)
    {
        if (player == Owner.Player)
        {
            NCreature? ownerNode = NCombatRoom.Instance?.GetCreatureNode(Owner);
            if (ownerNode != null)
            {
                Vector2 spawnPos = ownerNode.VfxSpawnPosition;
                TeamyVfx.PlaySimple("res://scenes/vfx/Fist.tscn",spawnPos,1.2f);
            }

            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        }
    }
}