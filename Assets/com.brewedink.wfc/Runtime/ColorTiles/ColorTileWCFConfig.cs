using System;
using UnityEngine;
using BrewedInk.WFC;

[CreateAssetMenu(fileName="New SpriteSheet Config", menuName="BrewedInk WFC/SpriteSheet Config")]
public class ColorTileWCFConfig : WCFConfigObject<ColorTileModuleObject, ColorTileModule>
{
    public Vector2Int Size;

    public int LeftSamplePad = 1;
    public int RightSamplePad = 1;
    public int LowSamplePad = 1;
    public int TopSamplePad = 1;

    public int SampleCount = 1;
    public float SampleTolerance = .2f;
    public float SampleCompressionRatio = 1;
    public float SampleCompressionOffset = 0;

    
    public override bool TryGetSprite(Module module, out Sprite sprite)
    {
        sprite = null;
        if (!TryGetObject(module, out var moduleObject))
        {
            return false;
        }

        sprite = moduleObject.Module.Sprite;
        return true;
    }

    protected override GenerationSpace CreateSpace()
    {
        var constraintSolver = new EdgeColorConstraintGenerator
        {
            SampleCount = SampleCount,
            LeftSamplePad = LeftSamplePad,
            RightSamplePad = RightSamplePad,
            LowSamplePad = LowSamplePad,
            TopSamplePad = TopSamplePad,
            ColorTolerance = SampleTolerance
        };
        var space = GenerationSpace.From2DGrid(Size.x, Size.y, GetModules().ProduceConstraints(constraintSolver), useSeed? seed : default);

        return space;
    }
}

[Serializable]
public class ColorTileModuleObject : ModuleObject<ColorTileModule>
{
    
}