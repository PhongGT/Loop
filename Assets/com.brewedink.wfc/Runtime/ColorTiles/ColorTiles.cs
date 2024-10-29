using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BrewedInk.WFC;

[Serializable]
public class ColorTileModule : Module
{
    public Sprite Sprite;
    
    public ColorTileModule(){}
    public ColorTileModule(Sprite sprite)
    {
        Sprite = sprite;
        Display = sprite.name;
    }

    public override int GetHashCode()
    {
        return Display.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is ColorTileModule other)
        {
            return other.Display.Equals(Display);
        }

        return false;
    }

}

[System.Serializable]
public struct ColorTileSample
{
    public Vector2Int Position;
    public Color Color;
}

public static class ColorTileSampleExtensions
{
    public static bool Matches(this ColorTileSample[] self, ColorTileSample[] other, float tolerance)
    {
        if (self.Length != other.Length) return false;
        for (var i = 0; i < self.Length; i++)
        {
            var selfColor = self[i].Color;
            var otherColor = other[i].Color;
            var a = new Vector4(selfColor.r, selfColor.g, selfColor.b, selfColor.b); 
            var b = new Vector4(otherColor.r, otherColor.g, otherColor.b, otherColor.b);

            var diff = a - b;
            if (diff.magnitude > tolerance)
            {
                return false;
            }
        }

        return true;
    }
}

public class EdgeColorConstraintGenerator : ConstraintGenerator<ColorTileModule>
{

    public int LeftSamplePad = 1;
    public int RightSamplePad = 1;
    public int TopSamplePad = 1;
    public int LowSamplePad = 1;
    public int SampleCount = 1;
    public float ColorTolerance = .5f;
    public float SampleCompressionRatio = 1;
    public float SampleCompressionOffset = 0;
    
    public override ColorTileModule Copy(ColorTileModule module, List<ModuleConstraint> allConstraints)
    {
        var clone = new ColorTileModule(module.Sprite)
        {
            Display = module.Display,
            Weight = module.Weight
        };
        
        var groups = allConstraints.Cast<AdjacencyConstraint>().GroupBy(c => c.Delta);

                
        foreach (var group in groups)
        {
            var needsOneOf = group.SelectMany(g => g.NeedsOneOf).ToList();
            clone.Constraints.Add(new AdjacencyConstraint
            {
                Delta = group.Key,
                NeedsOneOf = new ModuleSet(needsOneOf)
            });
        }

        return clone;
    }

    float[] GetRatioSamples()
    {
        var samples = new float[SampleCount];
        
        var size = 1f / (SampleCount + 1);
        for (var i = 0; i < SampleCount; i++)
        {
            var r = size * (i + 1);
            samples[i] = r;

            var center = .5f + (SampleCompressionOffset * .5f);
            var diff = (center - r);

            var compression = diff * SampleCompressionRatio;
            samples[i] = Mathf.Clamp01(center + compression);

            // var powDist = dist * 1.2f;
            // samples[i] = .5f + powDist;
        }

        return samples;
    }

    
    public Vector2Int[] GetLeftPositions(Sprite sprite)
    {
        var positions = new Vector2Int[SampleCount];
        var ratios = GetRatioSamples();
        for (var i = 0; i < SampleCount; i++)
        {
            var yCoord = (int) Mathf.Lerp(sprite.textureRect.yMin, sprite.textureRect.yMax, ratios[i]);
            positions[i] = new Vector2Int((int)sprite.textureRect.xMin + LeftSamplePad, yCoord);
        }
        return positions;
    }
    public Vector2Int[] GetRightPositions(Sprite sprite)
    {
        var positions = new Vector2Int[SampleCount];
        var ratios = GetRatioSamples();
        for (var i = 0; i < SampleCount; i++)
        {
            var yCoord = (int) Mathf.Lerp(sprite.textureRect.yMin, sprite.textureRect.yMax, ratios[i]);
            positions[i] = new Vector2Int((int)sprite.textureRect.xMax - RightSamplePad, yCoord);
        }
        return positions;
    }
    
    public Vector2Int[] GetLowPositions(Sprite sprite)
    {
        var positions = new Vector2Int[SampleCount];
        var ratios = GetRatioSamples();
        for (var i = 0; i < SampleCount; i++)
        {
            var xCoord = (int) Mathf.Lerp(sprite.textureRect.xMin, sprite.textureRect.xMax, ratios[i]);
            positions[i] = new Vector2Int(xCoord,  (int)sprite.textureRect.yMin + TopSamplePad);
        }
        return positions;
    }
    
    public Vector2Int[] GetTopPositions(Sprite sprite)
    {
        var positions = new Vector2Int[SampleCount];
        var ratios = GetRatioSamples();
        for (var i = 0; i < SampleCount; i++)
        {
            var xCoord = (int) Mathf.Lerp(sprite.textureRect.xMin, sprite.textureRect.xMax, ratios[i]);
            positions[i] = new Vector2Int(xCoord,  (int)sprite.textureRect.yMax - LowSamplePad);
        }
        return positions;
    }

    public ColorTileSample[] GetSamples(Sprite sprite, Func<Sprite, Vector2Int[]> positionFunc)
    {
        var positions = positionFunc(sprite);

        var samples = new ColorTileSample[positions.Length];
        for (var i = 0; i < positions.Length; i++)
        {
            var pos = positions[i];
            samples[i] = new ColorTileSample
            {
                Position = pos,
                Color = sprite.texture.GetPixel(pos.x, pos.y)
            };
        }
        return samples;
    }

    public ColorTileSample[] GetLeftSamples(Sprite sprite) => GetSamples(sprite, GetLeftPositions);
    public ColorTileSample[] GetRightSamples(Sprite sprite) => GetSamples(sprite, GetRightPositions);

    public ColorTileSample[] GetTopSamples(Sprite sprite) => GetSamples(sprite, GetLowPositions);
    public ColorTileSample[] GetLowSamples(Sprite sprite) => GetSamples(sprite, GetTopPositions);
    
    public override List<ModuleConstraint> CreateConstraints(ColorTileModule source, ColorTileModule target)
    {
            
        var sourceConstraints = new List<ModuleConstraint>();

        // left edge


        var sourceLeft = GetLeftSamples(source.Sprite);
        var sourceTop = GetTopSamples(source.Sprite);
        var sourceRight = GetRightSamples(source.Sprite);
        var sourceLow = GetLowSamples(source.Sprite);
        
        var targetRight = GetRightSamples(target.Sprite);
        var targetLow = GetLowSamples(target.Sprite);
        var targetLeft = GetLeftSamples(target.Sprite);
        var targetTop = GetTopSamples(target.Sprite);

        
        
        if (sourceLeft.Matches(targetRight, ColorTolerance))
        {
            sourceConstraints.Add(new AdjacencyConstraint
            {
                Delta = new Vector3Int(-1, 0, 0),
                NeedsOneOf = new ModuleSet(target)
            });
        }
        //
        if (sourceTop.Matches(targetLow, ColorTolerance))
        {
           
            sourceConstraints.Add(new AdjacencyConstraint
            {
                Delta = new Vector3Int(0, -1, 0),
                NeedsOneOf = new ModuleSet(target)
            });
        }
        if (sourceRight.Matches(targetLeft, ColorTolerance))
        {
            sourceConstraints.Add(new AdjacencyConstraint
            {
                Delta = new Vector3Int(1, 0, 0),
                NeedsOneOf = new ModuleSet(target)
            });
        }
                
        if (sourceLow.Matches(targetTop, ColorTolerance))
        {
            sourceConstraints.Add(new AdjacencyConstraint
            {
                Delta = new Vector3Int(0, 1, 0),
                NeedsOneOf = new ModuleSet(target)
            });
        }

        return sourceConstraints;
    }
}