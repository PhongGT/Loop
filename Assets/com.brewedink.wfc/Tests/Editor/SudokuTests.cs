using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BrewedInk.WFC.Examples.Sudoku;
using NUnit.Framework;
using UnityEngine;

namespace BrewedInk.WFC.Editor
{
    public class SudokuTests
    {

        public static SudokuModule Module1 = new SudokuModule(1);
        public static SudokuModule Module2 = new SudokuModule(2);
        public static SudokuModule Module3 = new SudokuModule(3);
        public static SudokuModule Module4 = new SudokuModule(4);
        public static SudokuModule Module5 = new SudokuModule(5);
        public static SudokuModule Module6 = new SudokuModule(6);
        public static SudokuModule Module7 = new SudokuModule(7);
        public static SudokuModule Module8 = new SudokuModule(8);
        public static SudokuModule Module9 = new SudokuModule(9);
        
        public static ModuleSet FullSet = new ModuleSet(Module1, Module2, Module3, Module4, Module5, Module6, Module7, Module8, Module9);
        
        
        [Test]
        public void OnlyOneAllowedInRow()
        {
            for (var seed = 0; seed < 1; seed++)
            {
                var constraintSolver = new SudokuConstraintGenerator();
                var space = GenerationSpace.From2DGrid(9, 9, FullSet.ProduceConstraints(constraintSolver), seed, SudokuConstraintGenerator.PrepareSpace);
                var sb = new StringBuilder();
                
                try
                {
                    space.Collapse().RunAsImmediate();

                    // Debug.Log(
                    //     $"P=[{space.PropagateLoops}] E=[{space.EvalsDone}] C=[{space.PropagateCalls}] IR=[{space.ImpliedRemoves}] DR=[{space.DirectRemoves}]");
                    var blocks = new Dictionary<Vector2Int, HashSet<int>>();

                    foreach (var slot in space.Slots)
                    {
                        if (space.TryGetOnlyOption(slot, out var selection) && selection is SudokuModule module)
                        {
                            var group = SudokuModule.GetRowCoordinate(slot.Coordinate);
                            if (!blocks.ContainsKey(group))
                            {
                                blocks[group] = new HashSet<int>();
                            }

                            var seen = blocks[group];
                            if (seen.Contains(module.N))
                            {
                                throw new Exception($"Already found {module.N} at {slot.Coordinate}");
                            }

                            seen.Add(module.N);
                        }
                    }

                    var str = SudokuRenderer.PrintBoard(space);
                    Debug.Log(str);
                }
                catch (Exception ex)
                {
                    Debug.Log("FAILURE: " + ex.Message);

                    var str = SudokuRenderer.PrintBoard(space);
                    Debug.Log(str);

                    Assert.Fail(ex.Message);
                    throw;
                }
                finally
                {
                    File.WriteAllText("test.txt", sb.ToString());
                }

            }
        }
        
        [Test]
        public void OnlyOneAllowedInBox()
        {
            for (var seed = 0; seed < 1; seed++)
            {

                var constraintSolver = new SudokuConstraintGenerator();
                var space = GenerationSpace.From2DGrid(9, 9, FullSet.ProduceConstraints(constraintSolver), 54,  SudokuConstraintGenerator.PrepareSpace);
                // space.OnPropagate += (slot) =>
                // {
                //     Debug.Log($"P=[{space.PropagateLoops}] E=[{space.EvalsDone}] C=[{space.PropagateCalls}] IR=[{space.ImpliedRemoves}] DR=[{space.DirectRemoves}]");
                //
                //     var str = SudokuRenderer.PrintBoard(space);
                //     Debug.Log(str);
                //     Debug.Log("----");
                // };
                try
                {
                    space.Collapse().RunAsImmediate();

                    //var seen = new HashSet<int>();

                    var blocks = new Dictionary<Vector2Int, HashSet<int>>();

                    foreach (var slot in space.Slots)
                    {
                        if (space.TryGetOnlyOption(slot, out var selection) && selection is SudokuModule module)
                        {
                            var group = SudokuModule.GetGroupCoordinate(slot.Coordinate);
                            if (!blocks.ContainsKey(group))
                            {
                                blocks[group] = new HashSet<int>();
                            }

                            var seen = blocks[group];
                            if (seen.Contains(module.N))
                            {
                                throw new Exception($"Already found {module.N} at {slot.Coordinate}");
                            }

                            seen.Add(module.N);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("FAILURE: " + ex.Message);

                    var str = SudokuRenderer.PrintBoard(space);
                    Debug.Log(str);

                    Assert.Fail(ex.Message);
                    throw;
                }
                finally
                {
                    var str = SudokuRenderer.PrintBoard(space);
                    Debug.Log(str);
                   
                }

            }
        }

        public static class SudokuRenderer
        {
            public static string PrintBoard(GenerationSpace space)
            {
                var sb = new StringBuilder();
                for (var y = 0; y < 9; y++)
                {
                    for (var x = 0; x < 9; x++)
                    {
                        var slot = space.GetSlot(new Vector3Int(x, y, 0));
                        
                        if (slot != null && space.TryGetOnlyOption(slot, out var selection) &&
                            selection is SudokuModule module)
                        {
                            for (var i = 0; i < 4; i++)
                            {
                                sb.Append(" ");
                            }
                            sb.Append(module.N);
                            
                            for (var i = 0; i < 9; i++)
                            {
                                sb.Append(" ");
                            }
                        }
                        else
                        {
                            var options = slot != null ? space.GetSlotOptions(slot) : null;
                            var pad = 10 - (options?.Count ?? 9);
                            var leftPad = Mathf.FloorToInt(pad / 2f);
                            var rightPad = Mathf.CeilToInt(pad / 2f);
                            for (var i = 0; i < leftPad; i++)
                            {
                                sb.Append(" ");
                            }

                            if (options != null)
                            foreach (var o in options)
                            {
                                sb.Append(o.Display);
                            }
                            for (var i = 0; i < rightPad; i++)
                            {
                                sb.Append(" ");
                            }
                            // sb.Append("?");
                        }

                        if (x > 0 && x < 8 && x % 3 == 2)
                        {
                            sb.Append("|");
                        }
                    }

                    sb.Append(Environment.NewLine);

                    if (y > 0 && y < 8 && y % 3 == 2)
                    {
                        for (var i = 0; i < 11*9; i++)
                        {
                            sb.Append("-");
                        }
                    }
                    sb.Append(Environment.NewLine);
                    
                }
                sb.Append(Environment.NewLine);

                var str = sb.ToString();
                return str;

            }
        }
        
    }
}