using Combo.Items;

namespace Combo.Frame.Types {
    public class OrderedFrame : ComboFrame {
        protected override void HandleHit(ComboItem item, float accuracy, int index) {
            if (++hitCount == index) base.HandleHit(item, accuracy, index);
            else OnMissed();
        }
    }
}