#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Teamy.character
{
    public class TeamyCardPool : TypeListCardPoolModel
    {
        // 卡池的ID。必须唯一防撞车。
        public override string Title => "Teamy";
        public override string EnergyColorName => "Teamy";

        // 描述中使用的能量图标。大小为24x24。
        public override string? TextEnergyIconPath => "res://images/energy/Teamy_energy_small_icon.png";
        // tooltip和卡牌左上角的能量图标。大小为74x74。
        public override string? BigEnergyIconPath => "res://images/energy/Teamy_energy_icon_big.png";

        // 卡池的主题色。
        public override Color DeckEntryCardColor => new(0.5f, 0.5f, 1f);
        // 能量表盘文字轮廓颜色
        public override Color EnergyOutlineColor => new(0.5f, 0.5f, 1f);

        // 卡池是否是无色。例如事件、状态等卡池就是无色的。
        public override bool IsColorless => false;
        public override Material? PoolFrameMaterial => MaterialUtils.CreateHsvShaderMaterial(0f, 1f, 1f);
    }
}
