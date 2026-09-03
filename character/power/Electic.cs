#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.cards.rare;

namespace Teamy.character.power
{
    [RegisterPower]
    public class Electic : ModPowerTemplate
    {
        // 类型，Buff或Debuff
        public override PowerType Type => PowerType.Buff;
        // 叠加类型，Counter表示可叠加，Single表示不可叠加
        public override PowerStackType StackType => PowerStackType.Single;

        // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
        public override PowerAssetProfile AssetProfile => new(
            IconPath: "res://images/powers/Electic.png",
            BigIconPath: "res://images/powers/Electic.png"
        );
        public static bool elect;
        public static bool electics;
        public static bool electlife =true;
        public bool Flag;
        public static Node2D? vfxNode;
        public Node2D? electic;// 保存特效引用
        protected override List<DynamicVar> CanonicalVars => [
            ModCardVars.Int("Electic",0)
        ];// 动态变量
        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner == Owner.Player && (cardPlay.Card.HasModKeyword(TeamyKeyword.Swk) || cardPlay.Card.HasModKeyword(TeamyKeyword.Unity)))
            { 
                DynamicVars["Electic"].UpgradeValueBy(1);
            }
            if (DynamicVars["Electic"].BaseValue >= 7)
            {
                if(ElecticLife._vfxNode != null && !electlife)
                {
                    ElecticLife._vfxNode?.QueueFree();
                    electlife = true;
                }
                ElecticLife._vfxNode = null;
                if (electic != null && !electics)
                {
                    electic?.QueueFree();
                    electics = true;
                }
                electic = null;
                if (!Owner.Player.Creature.HasPower<IntangiblePower>())
                {
                    NCreature? ownerNode = NCombatRoom.Instance?.GetCreatureNode(Owner);
                    Vector2 spawnPos = ownerNode.VfxSpawnPosition;
                    TeamyVfx.PlaySimple("res://scenes/vfx/electic.tscn", spawnPos, 1.8f);
                    await Task.Delay(1800);
                    vfxNode = TeamyVfx.Play("res://scenes/vfx/intangible.tscn", spawnPos);
                }
                await PowerCmd.Apply<IntangiblePower>(choiceContext,Owner, 1, Owner, null);
                Flag = true;
                DynamicVars["Electic"].BaseValue = 0;
            }
        }
        public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
        {
            if (Owner.Player != null && !Owner.Player.Creature.HasPower<IntangiblePower>() && Flag)
            {
                if (vfxNode != null && !elect)
                {
                    vfxNode?.QueueFree();
                    elect = true;
                }
                vfxNode = null;
                NCreature? ownerNode = NCombatRoom.Instance?.GetCreatureNode(Owner);
                if (ownerNode != null)
                {
                    Vector2 spawnPos = ownerNode.VfxSpawnPosition;
                    electic = TeamyVfx.Play("res://scenes/vfx/Elect.tscn", spawnPos);
                }
                Flag = false;
            }
        }
    }
}
