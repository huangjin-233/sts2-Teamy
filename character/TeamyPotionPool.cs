#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STS2RitsuLib.Scaffolding.Content;

namespace Teamy.character
{
    public class TeamyPotionPool : TypeListPotionPoolModel
    {
        // 描述中使用的能量图标。大小为24x24。
        public override string? TextEnergyIconPath => "res://images/energy/Teamy_energy_small_icon.png";
        // tooltip和卡牌左上角的能量图标。大小为74x74。
        public override string? BigEnergyIconPath => "res://images/energy/Teamy_energy_icon_big.png";

        public override string EnergyColorName => "Teamy";
    }
}
