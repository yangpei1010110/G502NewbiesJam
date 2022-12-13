using MoreMountains.Tools;

public class TestScript : MMTilemapGenerator
{
    private void Awake()
    {
        // 逐渐生成地图
        this.SlowRender          = true;
        this.SlowRenderTweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);
        this.Generate();
    }
}