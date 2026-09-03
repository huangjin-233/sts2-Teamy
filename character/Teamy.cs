#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Godot;
using Teamy.character.cards.basic;
using Teamy.character.relic;

namespace Teamy.character
{
    [RegisterCharacter]
    public class Teamy : ModCharacterTemplate<TeamyCardPool, TeamyRelicPool, TeamyPotionPool>
    {
        // 角色名称颜色
        public override Color NameColor => new(0.5f, 0.6f, 1f);
        // 能量图标轮廓颜色
        public override Color EnergyLabelOutlineColor => new(0.5f, 0.6f, 1f);

        // 人物性别（男女中立）
        public override CharacterGender Gender => CharacterGender.Masculine;

        // 初始血量和金币
        public override int StartingHp => 80;
        public override int StartingGold => 99;

        public override CharacterAssetProfile AssetProfile => CharacterAssetProfiles.Merge(
            CharacterAssetProfiles.Ironclad(),
            new(
                Scenes: new( 
                    // 人物模型tscn路径。
                    VisualsPath: "res://scenes/creature_visuals/Teamy.tscn",
                    // 能量表盘tscn路径。
                    EnergyCounterPath: "res://scenes/energy/Teamy_energy.tscn",
                    // 商店人物场景。
                    MerchantAnimPath: "res://scenes/merchant/Teamy_merchant.tscn",
                    // 篝火休息场景。
                    RestSiteAnimPath: "res://scenes/rest_site/characters/Teamy_rest_site.tscn"
                ),
                Ui: new(
                    // 人物头像路径。
                    IconTexturePath: "res://images/character/headimage.png",
                    // 人物头像2号。
                    IconPath: "res://scenes/ui/character_icons/Teamy_icon.tscn",
                    // 人物选择背景。
                    CharacterSelectBgPath: "res://scenes/screens/char_select/char_select_bg_teamy.tscn",
                // 人物选择图标。
                CharacterSelectIconPath: "res://images/character/character_select.png",
                // 人物选择图标-锁定状态。
                //CharacterSelectLockedIconPath: "res://Test/images/char_select_test_locked.png"
                // 人物选择过渡动画。
                // CharacterSelectTransitionPath: "res://materials/transitions/ironclad_transition_mat.tres",
                // 地图上的角色标记图标、表情轮盘上的角色头像
                 MapMarkerPath: "res://images/character/headimage.png"
                ),
                Vfx: new(
                // 卡牌拖尾场景。
                // TrailPath: "res://scenes/vfx/card_trail_ironclad.tscn"
                ),
                Audio: new(
                // 攻击音效
                // AttackSfx: null,
                // 施法音效
                // CastSfx: null,
                // 死亡音效
                // DeathSfx: null,
                // 角色选择音效
                // CharacterSelectSfx: null,
                // 过渡音效
                // CharacterTransitionSfx: "event:/sfx/ui/wipe_ironclad"
                ),
                Multiplayer: new(
                // 多人模式-手指。
                ArmPointingTexturePath: "res://images/character/Hand/multiplayer_hand_teamy_point.png",
                // 多人模式剪刀石头布-石头。
                ArmRockTexturePath: "res://images/character/Hand/multiplayer_hand_teamy_rock.png",
                // 多人模式剪刀石头布-布。
                ArmPaperTexturePath: "res://images/character/Hand/multiplayer_hand_teamy_paper.png",
                // 多人模式剪刀石头布-剪刀。
                ArmScissorsTexturePath: "res://images/character/Hand/multiplayer_hand_teamy_scissors.png"
                )));

        // 攻击和施法动画延迟，以对齐动画
        public override float AttackAnimDelay => 0f;
        public override float CastAnimDelay => 0f;

        // 自动转换人物场景，让你不需要手动挂脚本。复制即可。
        protected override NCreatureVisuals? TryCreateCreatureVisuals() => RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.Scenes!.VisualsPath!);

        // 攻击建筑师的攻击特效列表
        public override List<string> GetArchitectAttackVfx() => [
            "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
        ];
    }
}
