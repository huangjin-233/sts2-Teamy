using System.Collections.Generic;
using MegaCrit.Sts2.Core.Localization;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;

namespace Teamy.character.key;

[RegisterModelCapability]
public class Goukey : CardCapability,
    ICardDescriptionContributor
{

    public IEnumerable<CardDescriptionFragment> GetDescriptionFragments(
        CardDescriptionContext context) =>
    [
        new(
            new LocString("card_keywords", $"{Id.Entry}.exhaustHealDescription"),
            CardDescriptionFragmentPlacement.BeforeBase
        )
    ];
}