using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.key;
using Teamy.character.relic;

namespace Teamy.character.@event
{
    // 注册为共享事件（可出现在任意章节的事件池中）
    [RegisterSharedEvent]
    public class Mind : ModEventTemplate
    {
        // 事件立绘（占位图，直接替换 images/events/none.png 即可换成正式图）
        public override EventAssetProfile AssetProfile => new(
            InitialPortraitPath: "res://images/events/Mind.png"
        );

        // 事件数值
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
        ];
        

        // 三个初始选项
        protected override IReadOnlyList<EventOption> GenerateInitialOptions()
        {
            return
            [
                //END
                new EventOption(this, END, InitialOptionKey("END")),
                //TRUE END
                new EventOption(this, TRUE, InitialOptionKey("TRUE"),HoverTipFactory.FromRelic<MindHealth>()),
            ];
        }

        // 选项一：选5获得关键词
        private async Task END()
        {
            if (Owner != null)
            {
                var cardModels = (await CardSelectCmd.FromDeckGeneric(Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 0,5), c => !c.Keywords.Contains(TeamyKeyword.Xty) || !c.Keywords.Contains(TeamyKeyword.Wsd))).ToList();
                foreach (var cardModel in cardModels)
                {
                    if (cardModel != null)
                    {
                        if (cardModel.Keywords.Contains(TeamyKeyword.Xty))
                        {
                        }
                        else
                        {
                            cardModel.AddKeyword(TeamyKeyword.Xty);
                            cardModel.GetOrCreateCapability<Xtykey>(); // 挂载组件
                        }

                        if (cardModel.Keywords.Contains(TeamyKeyword.Wsd))
                        {
                        }
                        else
                        {
                            cardModel.AddKeyword(TeamyKeyword.Wsd);
                            cardModel.GetOrCreateCapability<Wsdkey>(); // 挂载组件
                        }
                    }
                }
            }
            SetEventFinished(PageDescription("END"));
        }

        // 选项二：降2遗物
        private async Task TRUE()
        {
            if (Owner != null)
            {
                List<CardModel> upgradedCards = Owner.Deck.Cards.Where(c => c.IsUpgraded).ToList();
                for (int i = 0; i < 2; i++)
                {
                    if (upgradedCards.Count <= 0)
                    {
                        break;
                    }

                    CardModel cardModel = Rng.NextItem(upgradedCards);
                    upgradedCards.Remove(cardModel);
                    CardCmd.Downgrade(cardModel);
                    CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.MessyLayout);
                    await Cmd.CustomScaledWait(0.3f, 0.5f);
                }

                await RelicCmd.Obtain<MindHealth>(Owner);
            }

            SetEventFinished(PageDescription("TRUE"));
        }
    }
}
