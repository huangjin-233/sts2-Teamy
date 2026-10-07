// ===== 遗物模板 =====
// 用法：复制本文件 → 改类名（类名决定遗物 id 与资源路径）→ 取消下面 [RegisterRelic] 的注释 → 补本地化与图标。
// 本地化 key（Teamy/localization/zhs/relics.json），类名按驼峰拆词转大写，如 TianPlus → TEAMY_RELIC_TIAN_PLUS：
//   {ENTRY}.title / {ENTRY}.description / {ENTRY}.flavor
// 图标（可选）：images/relics/{类名}.png，小图标 85x85、大图标 256x256。
// =====
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;


namespace Teamy.character.relic
{
    // 注册进遗物池；不需要这个模板本身生效时保持注释即可
    [RegisterRelic(typeof(TeamyRelicPool))]
    public class MindHealth : ModRelicTemplate
    {
        // 稀有度：Common / Uncommon / Rare / Shop / Ancient / Starter / Event / None
        public override RelicRarity Rarity => RelicRarity.Event;

        // 遗物数值，会替换本地化描述里的 {xxx} 占位符
        protected override IEnumerable<DynamicVar> CanonicalVars => [
        ];

        // 图标路径
        public override RelicAssetProfile AssetProfile => new(
            // 小图标（原版85x85）
            IconPath: $"res://images/relics/{GetType().Name}.png",
            // 轮廓图标（原版85x85）
            IconOutlinePath: $"res://images/relics/{GetType().Name}.png",
            // 大图标（原版256x256）
            BigIconPath: $"res://images/relics/{GetType().Name}.png"
        );

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Keywords.Contains(TeamyKeyword.Wsd))
            {
                
            }
        }
    }
}