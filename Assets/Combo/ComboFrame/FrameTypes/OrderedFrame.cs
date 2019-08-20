namespace Combo.ComboFrame.FrameTypes {
    public class OrderedFrame : ComboFrame {
        protected override void HandleHit(float accuracy, int index) {
            if (hitCount == index) base.HandleHit(accuracy, index);
            else ItemMissed();
        }
    }
}