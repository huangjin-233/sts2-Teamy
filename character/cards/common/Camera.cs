using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Powers.Mocks;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character.cards.common;

 [RegisterCard(typeof(TeamyCardPool))]
    public class Camera : ModCardTemplate
    {
        // 基础耗能
        private const int energyCost = 1;
        // 卡牌类型
        private const CardType type = CardType.Skill;
        // 卡牌稀有度
        private const CardRarity rarity = CardRarity.Common;
        // 目标类型（AnyEnemy表示任意敌人）
        private const TargetType targetType = TargetType.AnyEnemy;
        // 是否在卡牌图鉴中显示
        private const bool shouldShowInCardLibrary = true;
        public override IEnumerable<CardKeyword> CanonicalKeywords => [];
        // 卡图资源
        public override CardAssetProfile AssetProfile => new(
            PortraitPath: $"res://images/cards/skill/{GetType().Name}.png"
        );

        // 卡牌基础数值
        protected override IEnumerable<DynamicVar> CanonicalVars => [
            ModCardVars.Int("Power",3)
        ];

        public Camera() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
        {
        }

        // 打出时的效果逻辑
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var powers = ModelDb.AllPowers
                .Where(power =>
                    power.Type == PowerType.Debuff
                )
                .Where(power => IsSafeToApplyToEnemy(power, cardPlay.Target))
                .Where(power => !IsFromMod(power))
                .ToList();
            int powernum = Owner.RunState.Rng.CombatCardGeneration.NextInt(0, powers.Count);
            PowerModel power = powers[powernum].ToMutable();
            if (cardPlay.Target != null)
                    await PowerCmd.Apply(choiceContext, power, cardPlay.Target, DynamicVars["Power"].BaseValue, null, cardSource: this);
        }

        private bool IsFromMod(PowerModel power)
        {
            // 方法1：检查能力的类型是否在模组程序集中
            var type = power.GetType();
            var assembly = type.Assembly;
            
            var assemblyName = assembly.GetName().Name;
            
            if (!assemblyName.StartsWith("sts2") && 
                !assemblyName.StartsWith("MegaCrit") &&
                !assemblyName.Equals("mscorlib") &&
                !assemblyName.Equals("System") &&
                !assemblyName.StartsWith("Teamy."))
            {
                return true;
            }
            return false;
        }
        private bool IsSafeToApplyToEnemy(PowerModel power, Creature target)
        {
            if (power is HexPower or
                         RingingPower or 
                         MagicBombPower or
                         DampenPower or
                         MockModifyEnergyCostPower or
                         MockModifyStarCostPower or
                         MockScaleInMultiplayerPower or
                         MockDoNotScaleInMultiplayerPower or
                         MockRemoveDrawnCardsFromCombatPower or
                         KnockdownPower
                         )
                return false;
            // 2. 检查能力是否可以被应用到敌人
            if (power.Target != null && power.Target.IsPlayer)
                return false;
    
            // 3. 检查能力是否可见（通常不可见的能力是内部使用的）
            if (!power.IsVisible)
                return false;
            if (power.StackType == PowerStackType.Single && target.HasPower(power.Id))
            {
                return false;
            }
            return true;
        }
        
        // 升级后的效果逻辑
        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
        protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        ];
    }