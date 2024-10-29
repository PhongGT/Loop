using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace BrewedInk.WFC.Editor
{
    
    public class AsciiRoadTests
    {

        public AsciiRoadModule allGrass, horizontal1, vertical1, cornertl, cornertr, cornerll, cornerlr;
        
        public class AsciiRoadModule : Module
        {
            public string Visual;
            public AsciiRoadModule(string display, string visual)
            {
                Display = display;
                Visual = visual;
            }

            public AsciiRoadModule(AsciiRoadModule other)
            {
                Visual = other.Visual;
                Display = other.Display;
            }

            public override int GetHashCode()
            {
                return Display.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is AsciiRoadModule other)
                {
                    return other.Display.Equals(Display);
                }

                return false;
            }

            public string GetLeftEdge()
            {
                var rows = Visual.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                var sb = new StringBuilder();
                for (var i = 0; i < rows.Length; i++)
                {
                    sb.Append(rows[i][0]);
                }
                return sb.ToString();
            }
            
            public string GetRightEdge()
            {
                var rows = Visual.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                var sb = new StringBuilder();
                for (var i = 0; i < rows.Length; i++)
                {
                    sb.Append(rows[i][rows.Length - 1]);
                }
                return sb.ToString();
            }

            public string GetTopEdge()
            {
                var rows = Visual.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                return rows[0];
            }
            
            public string GetLowEdge()
            {
                var rows = Visual.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                return rows[rows.Length - 1];
            }

            
        }

        public class AsciiConstraintGenerator : ConstraintGenerator<AsciiRoadModule>
        {
            public override AsciiRoadModule Copy(AsciiRoadModule module, List<ModuleConstraint> allConstraints)
            {
                var clone = new AsciiRoadModule(module);
                
                // collapse the constraints into common form...
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

            public override List<ModuleConstraint> CreateConstraints(AsciiRoadModule source, AsciiRoadModule target)
            {
                var sourceConstraints = new List<ModuleConstraint>();

                // left edge
                

                var sourceLeft = source.GetLeftEdge();
                var sourceTop = source.GetTopEdge();
                var sourceRight = source.GetRightEdge();
                var sourceLow = source.GetLowEdge();
                
                var targetRight = target.GetRightEdge();
                var targetLow = target.GetLowEdge();
                var targetLeft = target.GetLeftEdge();
                var targetTop = target.GetTopEdge();

                if (sourceLeft.Equals(targetRight))
                {
                    sourceConstraints.Add(new AdjacencyConstraint
                    {
                        Delta = new Vector3Int(-1, 0, 0),
                        NeedsOneOf = new ModuleSet(target)
                    });
                }

                if (sourceTop.Equals(targetLow))
                {
                    sourceConstraints.Add(new AdjacencyConstraint
                    {
                        Delta = new Vector3Int(0, -1, 0),
                        NeedsOneOf = new ModuleSet(target)
                    });
                }

                if (sourceRight.Equals(targetLeft))
                {
                    sourceConstraints.Add(new AdjacencyConstraint
                    {
                        Delta = new Vector3Int(1, 0, 0),
                        NeedsOneOf = new ModuleSet(target)
                    });
                }
                
                if (sourceLow.Equals(targetTop))
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

        [SetUp]
        public void SetupModules()
        {
            allGrass = new AsciiRoadModule("grass",
                @".....
.....
.....
.....
.....");
            horizontal1 = new AsciiRoadModule("horizontal1",
                @".....
.....
XXXXX
.....
.....");
            
            vertical1 = new AsciiRoadModule("vertical1",
                @"..X..
..X..
..X..
..X..
..X..");
            
            cornertl = new AsciiRoadModule("cornertl",
                @"..X..
..X..
XXX..
.....
.....");
            
            cornertr = new AsciiRoadModule("cornertr",
                @"..X..
..X..
..XXX
.....
.....");
            
            cornerlr = new AsciiRoadModule("cornerlr",
                @".....
.....
..XXX
..X..
..X..");
            
            cornerll = new AsciiRoadModule("cornerll",
                @".....
.....
XXX..
..X..
..X..");
        }

        [Test]
        public void RoadModulesHave_HaveSides()
        {
            Assert.AreEqual(".....", allGrass.GetLeftEdge());
            Assert.AreEqual(".....", allGrass.GetRightEdge());
            Assert.AreEqual(".....", allGrass.GetTopEdge());
            Assert.AreEqual(".....", allGrass.GetLowEdge());
            
            Assert.AreEqual("..X..", horizontal1.GetLeftEdge());
            Assert.AreEqual("..X..", horizontal1.GetRightEdge());
            Assert.AreEqual(".....", horizontal1.GetTopEdge());
            Assert.AreEqual(".....", horizontal1.GetLowEdge());
            
            Assert.AreEqual(".....", vertical1.GetLeftEdge());
            Assert.AreEqual(".....", vertical1.GetRightEdge());
            Assert.AreEqual("..X..", vertical1.GetTopEdge());
            Assert.AreEqual("..X..", vertical1.GetLowEdge());
            
            Assert.AreEqual("..X..", cornertl.GetLeftEdge());
            Assert.AreEqual(".....", cornertl.GetRightEdge());
            Assert.AreEqual("..X..", cornertl.GetTopEdge());
            Assert.AreEqual(".....", cornertl.GetLowEdge());
        }

        [Test]
        public void Generate_LeftEdgeConstraints()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);

            var moduleList = modules.ToList();
            Assert.AreEqual(2, modules.Count);

            var grassModule = moduleList[0];
            var horizontalModule = moduleList[1];
            Assert.AreEqual(allGrass.Display, grassModule.Display);
            Assert.AreEqual(horizontal1.Display, horizontalModule.Display);

            Assert.AreEqual(4, grassModule.Constraints.Count);
            Assert.AreEqual(1, grassModule.Constraints.Cast<AdjacencyConstraint>().Where(c => c.Delta == new Vector3Int(-1, 0, 0)).FirstOrDefault().NeedsOneOf.Count); // left constraint
            Assert.AreEqual(2, grassModule.Constraints.Cast<AdjacencyConstraint>().Where(c => c.Delta == new Vector3Int(0, -1, 0)).FirstOrDefault().NeedsOneOf.Count); // top constraint
            Assert.AreEqual(grassModule.Display, grassModule.Constraints.Cast<AdjacencyConstraint>().ToList()[0].NeedsOneOf.ToList()[0].Display);
            
            Assert.AreEqual(4, horizontalModule.Constraints.Count);
            Assert.AreEqual(2, horizontalModule.Constraints.Cast<AdjacencyConstraint>().Where(c => c.Delta == new Vector3Int(0, -1, 0)).FirstOrDefault().NeedsOneOf.Count);
            Assert.AreEqual(1, horizontalModule.Constraints.Cast<AdjacencyConstraint>().Where(c => c.Delta == new Vector3Int(-1, 0, 0)).FirstOrDefault().NeedsOneOf.Count);
            Assert.AreEqual(horizontalModule.Display, horizontalModule.Constraints.Cast<AdjacencyConstraint>().Where(c => c.Delta == new Vector3Int(-1, 0, 0)).FirstOrDefault().NeedsOneOf.ToList()[0].Display);

            
        }

        [Test]
        public void Generate_CornerConstraints()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1, vertical1, cornertl);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);
            var moduleList = modules.ToList();


            var cornerModule = moduleList.FirstOrDefault(m => m.Display.Equals(cornertl.Display));
            Assert.AreEqual(4, cornerModule.Constraints.Count);

            var leftConstraint = cornerModule.Constraints.Cast<AdjacencyConstraint>().ToList().Find(c => c.Delta == new Vector3Int(-1, 0, 0));
            Assert.AreEqual(1, leftConstraint.NeedsOneOf.Count);
            Assert.AreEqual(horizontal1.Display, leftConstraint.NeedsOneOf.ToList()[0].Display);

            var rightConstraint = cornerModule.Constraints.Cast<AdjacencyConstraint>().ToList().Find(c => c.Delta == new Vector3Int(1, 0, 0));
            Assert.AreEqual(2, rightConstraint.NeedsOneOf.Count);
            Assert.AreEqual(allGrass.Display, rightConstraint.NeedsOneOf.ToList()[0].Display);
            Assert.AreEqual(vertical1.Display, rightConstraint.NeedsOneOf.ToList()[1].Display);
            
            var topConstraint = cornerModule.Constraints.Cast<AdjacencyConstraint>().ToList().Find(c => c.Delta == new Vector3Int(0, -1, 0));
            Assert.AreEqual(1, topConstraint.NeedsOneOf.Count); // TODO FAILURE?
            Assert.AreEqual(vertical1.Display, topConstraint.NeedsOneOf.ToList()[0].Display);
            
            var lowConstraint = cornerModule.Constraints.Cast<AdjacencyConstraint>().ToList().Find(c => c.Delta == new Vector3Int(0, 1, 0));
            Assert.AreEqual(2, lowConstraint.NeedsOneOf.Count);
            Assert.AreEqual(allGrass.Display, lowConstraint.NeedsOneOf.ToList()[0].Display);
            Assert.AreEqual(horizontal1.Display, lowConstraint.NeedsOneOf.ToList()[1].Display);
        }

        [Test]
        public void GenerateSimple_1X2_Options()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);

            var grassModule = modules.FindByDisplay(allGrass);
            var horizModule = modules.FindByDisplay(horizontal1);
            
            var space = GenerationSpace.From2DGrid(1, 2, modules);
            var lowSlot = space.GetSlot(new Vector3Int(0, 0, 0));
            var topSlot = space.GetSlot(new Vector3Int(0, 1, 0));
            
            space.RemoveSlotOption(lowSlot, grassModule);

            var hasLow = space.TryGetOnlyOption(lowSlot, out var selectedLowModule);
            Assert.IsTrue(hasLow);
            Assert.AreEqual(horizModule, selectedLowModule);

            var hasTop = space.TryGetOnlyOption(topSlot, out var selectedTopModule);
            Assert.IsFalse(hasTop);
        }
        
        [Test]
        public void GenerateSimple_3X1_Implications()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);

            var grassModule = modules.FindByDisplay(allGrass);
            var horizModule = modules.FindByDisplay(horizontal1);
            
            var space = GenerationSpace.From2DGrid(3, 1, modules);
            var left = space.GetSlot(new Vector3Int(0, 0, 0));
            var mid = space.GetSlot(new Vector3Int(1, 0, 0));
            var right = space.GetSlot(new Vector3Int(2, 0, 0));
            
            space.RemoveSlotOption(left, grassModule);

            var hasLow = space.TryGetOnlyOption(left, out var selectedLowModule);
            Assert.IsTrue(hasLow);
            Assert.AreEqual(horizModule, selectedLowModule);

            var hasMid = space.TryGetOnlyOption(mid, out var selectedMidModule);
            Assert.IsTrue(hasMid);
            Assert.AreEqual(horizModule, selectedMidModule);

            var hasRight = space.TryGetOnlyOption(right, out var selectedRightModule);
            Assert.IsTrue(hasRight);
            Assert.AreEqual(horizModule, selectedRightModule);

            var str = AsciiRenderer.Render(space, 3, 1);
            Debug.Log(str);

        }

        [Test]
        public void RelaxTest_OnlyOne_3X3_Middle()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1, vertical1);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);
            var space = GenerationSpace.From2DGrid(3, 3, modules);

            space.RemoveSlotOption(space.GetSlot(new Vector3Int(1, 1, 0)), modules.FindByDisplay(allGrass));

            space.CollapseSlot(space.GetSlot(new Vector3Int(1, 1, 0)));
            
            var str = AsciiRenderer.Render(space, 3, 3);
            Debug.Log(str);
        }

        [Test]
        public void RelaxTest_3X3()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1, vertical1, cornertl);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);
            var space = GenerationSpace.From2DGrid(3, 3, modules, seed:24);

            space.RemoveSlotOption(space.GetSlot(new Vector3Int(1, 1, 0)), modules.FindByDisplay(allGrass));
            space.Collapse().RunAsImmediate();
            
            var str = AsciiRenderer.Render(space, 3, 3);
            Debug.Log(str);
        }
        
        [Test]
        public void RelaxTest_10X5()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1, vertical1, cornertl, cornertr, cornerll, cornerlr);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);
            var space = GenerationSpace.From2DGrid(10, 5, modules, seed:24);

            space.RemoveSlotOption(space.GetSlot(new Vector3Int(1, 1, 0)), modules.FindByDisplay(allGrass));
            space.Collapse().RunAsImmediate();
            
            var str = AsciiRenderer.Render(space, 10, 5);
            Debug.Log(str);
        }
        [Test]
        public void RelaxTest_10X10()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1, vertical1, cornertl, cornertr, cornerll, cornerlr);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);
            var space = GenerationSpace.From2DGrid(50, 50, modules, seed:28);

            space.RemoveSlotOption(space.GetSlot(new Vector3Int(1, 1, 0)), modules.FindByDisplay(allGrass));
            space.RemoveSlotOption(space.GetSlot(new Vector3Int(3, 3, 0)), modules.FindByDisplay(allGrass));
            space.RemoveSlotOption(space.GetSlot(new Vector3Int(5, 5, 0)), modules.FindByDisplay(allGrass));
            space.RemoveSlotOption(space.GetSlot(new Vector3Int(7, 1, 0)), modules.FindByDisplay(allGrass));
            space.RemoveSlotOption(space.GetSlot(new Vector3Int(9, 7, 0)), modules.FindByDisplay(allGrass));
            space.Collapse().RunAsImmediate();
            
            space.CollapseSlot(space.GetSlot(new Vector3Int(5, 6, 0)));
            space.CollapseSlot(space.GetSlot(new Vector3Int(4, 5, 0)));
            space.CollapseSlot(space.GetSlot(new Vector3Int(6, 4, 0)));
            
            var str = AsciiRenderer.Render(space, 50, 50, false);
            Debug.Log(str);
        }

        
        [Test]
        public void RelaxTest_2X1_CornerInRight()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1, vertical1, cornertl);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);
            var space = GenerationSpace.From2DGrid(2, 1, modules, seed:24);

            var rightSlot = space.GetSlot(new Vector3Int(1, 0, 0));
            var leftSlot = space.GetSlot(new Vector3Int(0, 0, 0));
            var cornerModule = modules.FindByDisplay(cornertl);
            space.CollapseSlot(rightSlot, cornerModule);
            //space.RelaxAllSlots();

            if (space.TryGetOnlyOption(leftSlot, out var selection) && selection is AsciiRoadModule mod)
            {
                Assert.AreEqual(mod.Display, horizontal1.Display);
            }
            else
            {
                Assert.Fail("The left slot should have been marked as horizontal");
            }
            
            var str = AsciiRenderer.Render(space, 2, 1);
            Debug.Log(str);
        }

        
        [Test]
        public void RelaxTest_OnlyOne_3X3_LeftMid()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1, vertical1);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);
            var space = GenerationSpace.From2DGrid(3, 3, modules);

            space.RemoveSlotOption(space.GetSlot(new Vector3Int(0, 1, 0)), modules.FindByDisplay(allGrass));

            space.CollapseSlot(space.GetSlot(new Vector3Int(0, 1, 0)));
            
            var str = AsciiRenderer.Render(space, 3, 3);
            Debug.Log(str);
        }
        
        
        [Test]
        public void RelaxTest_Two_3X3_DiagCorners()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1, vertical1);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);
            var space = GenerationSpace.From2DGrid(3, 3, modules);

            space.RemoveSlotOption(space.GetSlot(new Vector3Int(0, 0, 0)), modules.FindByDisplay(allGrass));
            space.RemoveSlotOption(space.GetSlot(new Vector3Int(2, 2, 0)), modules.FindByDisplay(allGrass));

            space.CollapseSlot(space.GetSlot(new Vector3Int(0, 0, 0)));
            space.CollapseSlot(space.GetSlot(new Vector3Int(2, 2, 0)));
         
            var str = AsciiRenderer.Render(space, 3, 3);
            Debug.Log(str);
        }

        [Test]
        public void RelaxTest_Two_3X3_DiagCorners_Invalid_Intersection()
        {
            var vanillaModules = new ModuleSet(allGrass, horizontal1, vertical1);
            var constraintGenerator = new AsciiConstraintGenerator();
            var modules = vanillaModules.ProduceConstraints(constraintGenerator);
            var space = GenerationSpace.From2DGrid(3, 3, modules);

            space.RemoveSlotOption(space.GetSlot(new Vector3Int(0, 0, 0)), modules.FindByDisplay(allGrass));
            space.RemoveSlotOption(space.GetSlot(new Vector3Int(0, 0, 0)), modules.FindByDisplay(horizontal1));
            space.RemoveSlotOption(space.GetSlot(new Vector3Int(2, 2, 0)), modules.FindByDisplay(allGrass));

            Assert.Throws<SlotCannotHaveEmptyModuleSetException>(() =>
            {
                space.RemoveSlotOption(space.GetSlot(new Vector3Int(2, 2, 0)), modules.FindByDisplay(vertical1));
            });

        }
        class AsciiRenderer
        {
            public static string Render(GenerationSpace space, int width, int height, bool debug=true)
            {
                var final = new StringBuilder();
                for (var y = 0; y < height; y++)
                {
                    var rowBuilder1 = new StringBuilder();
                    var rowBuilder2 = new StringBuilder();
                    var rowBuilder3 = new StringBuilder();
                    var rowBuilder4 = new StringBuilder();
                    var rowBuilder5 = new StringBuilder();
                    for (var x = 0; x < width; x++)
                    {
                        var slot = space.GetSlot(new Vector3Int(x, y, 0));
                        var data = @"?????
?????
?????
?????
?????";
                        if (space.TryGetOnlyOption(slot, out var option) && option is AsciiRoadModule module)
                        {
                            data = module.Visual;
                        }

                        var lines = data.Split(new string[] {Environment.NewLine},
                            StringSplitOptions.RemoveEmptyEntries);

                        rowBuilder1.Append(lines[0]);
                        
                        if (debug) rowBuilder1.Append(" ");
                        rowBuilder2.Append(lines[1]);
                        if (debug) rowBuilder2.Append(" ");
                        rowBuilder3.Append(lines[2]);
                        if (debug) rowBuilder3.Append(" ");
                        rowBuilder4.Append(lines[3]);
                        if (debug) rowBuilder4.Append(" ");
                        rowBuilder5.Append(lines[4]);
                        if (debug) rowBuilder5.Append(" ");
                    }

                    final.Append(rowBuilder1);
                    final.Append(Environment.NewLine);
                    
                    final.Append(rowBuilder2);
                    final.Append(Environment.NewLine);
                    
                    final.Append(rowBuilder3);
                    final.Append(Environment.NewLine);
                    
                    final.Append(rowBuilder4);
                    final.Append(Environment.NewLine);
                    
                    final.Append(rowBuilder5);
                    final.Append(Environment.NewLine);
                    if (debug) final.Append(Environment.NewLine); // one for good luck.

                }

                return final.ToString();

            }
        }
        
    }
}