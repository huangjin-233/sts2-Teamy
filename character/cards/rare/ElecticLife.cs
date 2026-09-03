#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.GameInfo.Objects;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.power;

namespace Teamy.character.cards.rare
{
    [RegisterCard(typeof(TeamyCardPool))]
    public class ElecticLife : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 2;
        // 卡牌类型
        private const CardType type = CardType.Power;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Rare;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.Self;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        // 卡图资源
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal,TeamyKeyword.Swk];


        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/power/{GetType().Name}.png"
        );
        // 卡牌基础数值
        public static Node2D? _vfxNode;  // 保存特效引用
        public void SetVfxNode(Node2D vfxNode)
        {
            _vfxNode = vfxNode;
        }
        public ElecticLife() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            if (!Owner.Creature.HasPower<Electic>())
            {
                NCreature? ownerNode = NCombatRoom.Instance?.GetCreatureNode(Owner.Creature);
                if (ownerNode != null)
                {
                    Vector2 spawnPos = ownerNode.VfxSpawnPosition;
                    Node2D? vfxNode = TeamyVfx.Play("res://scenes/vfx/Elect.tscn", spawnPos);
                    if (vfxNode != null)
                    {
                        SetVfxNode(vfxNode);
                        Electic.electlife = false;
                    }
                }
            }
            await PowerCmd.Apply<Electic>(choiceContext,Owner.Creature, 1, Owner.Creature, null);
        }

        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
           RemoveKeyword(CardKeyword.Ethereal);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
              
             HoverTipFactory.FromPower<IntangiblePower>(),
             HoverTipFactory.FromKeyword(CardKeyword.Ethereal),
    ];
    }
}
