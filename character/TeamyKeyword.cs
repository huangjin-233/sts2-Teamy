using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Teamy.character
{
	[RegisterOwnedCardKeyword(nameof(Six))]
	[RegisterOwnedCardKeyword(nameof(Bc))]
	[RegisterOwnedCardKeyword(nameof(Zzl))]
	[RegisterOwnedCardKeyword(nameof(Wsd))]
	[RegisterOwnedCardKeyword(nameof(Swk))]
	[RegisterOwnedCardKeyword(nameof(Xty))]
	[RegisterOwnedCardKeyword(nameof(Gou))]
	[RegisterOwnedCardKeyword(nameof(Unity))]
	[RegisterOwnedCardKeyword(nameof(Replace), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
	internal class TeamyKeyword
	{
		public static readonly CardKeyword  Six = ModContentRegistry.GetQualifiedKeywordId(Modinit.ModId, nameof(Six)).GetModCardKeyword();
		public static readonly CardKeyword  Bc = ModContentRegistry.GetQualifiedKeywordId(Modinit.ModId, nameof(Bc)).GetModCardKeyword();
		public static readonly CardKeyword  Zzl = ModContentRegistry.GetQualifiedKeywordId(Modinit.ModId, nameof(Zzl)).GetModCardKeyword();
		public static readonly CardKeyword  Wsd = ModContentRegistry.GetQualifiedKeywordId(Modinit.ModId, nameof(Wsd)).GetModCardKeyword();
		public static readonly CardKeyword  Swk = ModContentRegistry.GetQualifiedKeywordId(Modinit.ModId, nameof(Swk)).GetModCardKeyword();
		public static readonly CardKeyword  Xty = ModContentRegistry.GetQualifiedKeywordId(Modinit.ModId, nameof(Xty)).GetModCardKeyword();
		public static readonly CardKeyword  Gou = ModContentRegistry.GetQualifiedKeywordId(Modinit.ModId, nameof(Gou)).GetModCardKeyword();
		public static readonly CardKeyword  Unity = ModContentRegistry.GetQualifiedKeywordId(Modinit.ModId, nameof(Unity)).GetModCardKeyword();
		public static readonly CardKeyword  Replace = ModContentRegistry.GetQualifiedKeywordId(Modinit.ModId, nameof(Replace)).GetModCardKeyword();
	}
}
