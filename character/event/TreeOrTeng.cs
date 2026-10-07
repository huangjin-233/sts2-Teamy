using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Teamy.character.cards.common;
using Teamy.character.cards.special;

namespace Teamy.character.@event
{
    // 注册为共享事件（可出现在任意章节的事件池中）
    [RegisterSharedEvent]
    public class TreeOrTeng : ModEventTemplate
    {
        // 事件立绘（占位图，直接替换 images/events/none.png 即可换成正式图）
        public override EventAssetProfile AssetProfile => new(
            InitialPortraitPath: "res://images/events/grape.png"
        );

        // 事件数值
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
        ];

        // 只有牌组中拥有可变化的「葡萄兄弟」时，该事件才会进入事件池
        public override bool IsAllowed(IRunState runState)
        {
            return runState.Players.Any(p =>
                p.Deck.Cards.Any(c => c.IsTransformable && c is PrapeBrother));
        }

        // 三个初始选项
        protected override IReadOnlyList<EventOption> GenerateInitialOptions()
        {
            // 牌组中没有可变化的「葡萄兄弟」时，前两个选项锁定
            bool hasPrapeBrother = Owner != null &&
                Owner.Deck.Cards.Any(c => c.IsTransformable && c is PrapeBrother);

            return
            [
                // 选项一：六谷（需要牌组中有葡萄兄弟）
                hasPrapeBrother
                    ? new EventOption(this, Tree, InitialOptionKey("Tree"),HoverTipFactory.FromCardWithCardHoverTips<PrapeBrotherSix>())
                    : new EventOption(this, null, InitialOptionKey("Tree_LOCKED")),
                // 选项二：BC（需要牌组中有葡萄兄弟）
                hasPrapeBrother
                    ? new EventOption(this, Teng, InitialOptionKey("Teng"),HoverTipFactory.FromCardWithCardHoverTips<PrapeBrotherBc>())
                    : new EventOption(this, null, InitialOptionKey("Teng_LOCKED")),
                // 选项三：增加
                new EventOption(this, Unknow, InitialOptionKey("Unknow"),HoverTipFactory.FromCardWithCardHoverTips<PrapeBrother>()),
            ];
        }

        // 选项一：0费2力
        private async Task Tree()
        {
            if (Owner != null)
            {
                CardModel cardModel = (await CardSelectCmd.FromDeckGeneric(Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1), c => c.IsTransformable && c is PrapeBrother)).FirstOrDefault();
                if (cardModel != null)
                {
                    // 保留原牌的升级状态
                    bool upgraded = cardModel.IsUpgraded;
                    var result = await CardCmd.TransformTo<PrapeBrotherSix>(cardModel, CardPreviewStyle.EventLayout);
                    if (upgraded && result is { } added && added.cardAdded != null)
                    {
                        CardCmd.Upgrade(added.cardAdded, CardPreviewStyle.EventLayout);
                    }
                }
            }
            SetEventFinished(PageDescription("Tree"));
        }

        // 选项二：2费5力
        private async Task Teng()
        {
            if (Owner != null)
            {
                CardModel cardModel = (await CardSelectCmd.FromDeckGeneric(Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1), c => c.IsTransformable && c is PrapeBrother)).FirstOrDefault();
                if (cardModel != null)
                {
                    // 保留原牌的升级状态
                    bool upgraded = cardModel.IsUpgraded;
                    var result = await CardCmd.TransformTo<PrapeBrotherBc>(cardModel, CardPreviewStyle.EventLayout);
                    if (upgraded && result is { } added && added.cardAdded != null)
                    {
                        CardCmd.Upgrade(added.cardAdded, CardPreviewStyle.EventLayout);
                    }
                }
            }
            SetEventFinished(PageDescription("Teng"));
        }

        // 选项三：再来一张
        private async Task Unknow()
        {
            if (Owner != null)
            {
                CardModel card = Owner.RunState.CreateCard<PrapeBrother>(base.Owner);
                CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), 2f);
            }
            SetEventFinished(PageDescription("Unknow"));
        }
    }
}
