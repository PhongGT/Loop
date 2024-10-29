using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BrewedInk.WFC;

/// <summary>
/// A type of WFCConfigObject that lets you generate sprite modules with socket connective constraints.
/// </summary>
[CreateAssetMenu(fileName="New Sprite Socket Config", menuName="BrewedInk WFC/Sprite Socket Config")]
public class SpriteConfig : WCFConfigObject<SpriteConfigModuleObject, SpriteConfigModule>
{
    public int Width;
    public int Height;
    
    protected override GenerationSpace CreateSpace()
    {
        // create the constraints on the modules...
        var constraintGenerator = new SocketConstraintGenerator<SpriteConfigModule>(existing => new SpriteConfigModule(existing));
        var constrainedModules = GetModules().ProduceConstraints(constraintGenerator);
        return GenerationSpace.From2DGrid(Width, Height, constrainedModules, useSeed ? seed : default);
    }
    
    public override bool TryGetSprite(Module module, out Sprite sprite)
    {
        sprite = null;
        if (module is SpriteConfigModule typedModule)
        {
            sprite = typedModule.sprite;
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class SpriteConfigModule : Module, IHasSockets
{
    public Sprite sprite;
    public int leftSocket, rightSocket, topSocket, lowSocket;
    public int LeftSocket => leftSocket;
    public int RightSocket => rightSocket;
    public int TopSocket => topSocket;
    public int LowSocket => lowSocket;

    public SpriteConfigModule()
    {
        
    }

    public SpriteConfigModule(SpriteConfigModule existing)
    {
        sprite = existing.sprite;
        leftSocket = existing.leftSocket;
        rightSocket = existing.rightSocket;
        topSocket = existing.topSocket;
        lowSocket = existing.lowSocket;
        Display = existing.Display;
        Weight = existing.Weight;
    }
    
    public override int GetHashCode()
    {
        return Display.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is SpriteConfigModule other)
        {
            return other.Display.Equals(Display);
        }

        return false;
    }
}

[System.Serializable]
public class SpriteConfigModuleObject : ModuleObject<SpriteConfigModule>
{
}

public interface IHasSockets
{
    int LeftSocket { get; }
    int RightSocket { get; }
    int TopSocket { get; }
    int LowSocket { get; }
}

public class SocketConstraintGenerator<TModule> : ConstraintGenerator<TModule> where TModule : Module, IHasSockets
{
    private readonly Func<TModule, TModule> _copier;

    public SocketConstraintGenerator(Func<TModule, TModule> copier)
    {
        _copier = copier;
    }
    
    public override TModule Copy(TModule module, List<ModuleConstraint> constraints)
    {
        var clone = _copier(module);
        
        var groups = constraints.Cast<AdjacencyConstraint>().GroupBy(c => c.Delta);

                
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

    public override List<ModuleConstraint> CreateConstraints(TModule source, TModule target)
    {
        var sourceConstraints = new List<ModuleConstraint>();

        if (source.LeftSocket == target.RightSocket)
        {
            sourceConstraints.Add(new AdjacencyConstraint
            {
                Delta = new Vector3Int(-1, 0, 0),
                NeedsOneOf = new ModuleSet(target)
            });
        }

        if (source.RightSocket == target.LeftSocket)
        {
            sourceConstraints.Add(new AdjacencyConstraint
            {
                Delta = new Vector3Int(1, 0, 0),
                NeedsOneOf = new ModuleSet(target)
            });
        }
        
        if (source.TopSocket == target.LowSocket)
        {
            sourceConstraints.Add(new AdjacencyConstraint
            {
                Delta = new Vector3Int(0, 1, 0),
                NeedsOneOf = new ModuleSet(target)
            });
        }
        
        if (source.LowSocket == target.TopSocket)
        {
            sourceConstraints.Add(new AdjacencyConstraint
            {
                Delta = new Vector3Int(0, -1, 0),
                NeedsOneOf = new ModuleSet(target)
            });
        }

        return sourceConstraints;
    }
}
