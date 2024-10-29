using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BrewedInk.WFC.Examples.Sudoku
{
    [System.Serializable]
    public class SudokuModule : Module
    {
        public int N;
        

        public SudokuModule(int n)
        {
            N = n;
            Display = N.ToString();
        }

        public static Vector2Int GetGroupCoordinate(Vector3Int slotCoordinate)
        {
            return new Vector2Int(slotCoordinate.x / 3, slotCoordinate.y / 3);
        }

        public static Vector2Int GetRowCoordinate(Vector3Int slotCoordinate)
        {
            return new Vector2Int(0, slotCoordinate.y);
        }

        public static Vector2Int GetColCoordinate(Vector3Int slotCoordinate)
        {
            return new Vector2Int(slotCoordinate.x, 0);
        }
    }

    public class GroupConstraint : ModuleConstraint
    {
        public Func<Vector3Int, Vector2Int> CoordFunc { get; }
        public Func<Vector3Int, GenerationSpace, IEnumerable<Slot>> GroupBuilder { get; }

        public GroupConstraint(Func<Vector3Int, Vector2Int> coordFunc,
            Func<Vector3Int, GenerationSpace, IEnumerable<Slot>> groupBuilder)
        {
            CoordFunc = coordFunc;
            GroupBuilder = groupBuilder;
        }

        public static IEnumerable<Slot> GetBlockNeighbors(Vector3Int slotCoordinate, GenerationSpace space)
        {
            var groupCoord = SudokuModule.GetGroupCoordinate(slotCoordinate);
            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    var coord = new Vector3Int(x + (groupCoord.x * 3), y + (groupCoord.y * 3), 0);
                    yield return space.GetSlot(coord);
                }
            }
        }

        public static IEnumerable<Slot> GetRowNeighbors(Vector3Int slotCoordinate, GenerationSpace space)
        {
            var groupCoord = SudokuModule.GetRowCoordinate(slotCoordinate);
            for (var x = 0; x < 9; x++)
            {
                var coord = new Vector3Int(x, groupCoord.y, 0);
                yield return space.GetSlot(coord);

            }
        }

        public static IEnumerable<Slot> GetColNeighbors(Vector3Int slotCoordinate, GenerationSpace space)
        {
            var groupCoord = SudokuModule.GetColCoordinate(slotCoordinate);
            for (var x = 0; x < 9; x++)
            {
                var coord = new Vector3Int(groupCoord.x, x, 0);
                yield return space.GetSlot(coord);

            }
        }

        

        public override bool ShouldRemoveModule(SlotEdge edge, GenerationSpace space, Module m, ModuleSet modulesToRemove)
        {
            
            var source = edge.Source;
            var target = edge.Target;
            var sourceOptions = space.GetSlotOptions(source);
            var targetOptions = space.GetSlotOptions(target);

            var sourceCoord = CoordFunc(source.Coordinate);
            var targetCoord = CoordFunc(target.Coordinate);
            if (sourceCoord != targetCoord)
            {
                return false;
            }

            if (targetOptions.Count == 1 && targetOptions.Contains(m))
            {
                //Debug.Log($"Source=[{source.Coordinate}] cannot have m=[{m}] because target=[{target.Coordinate}] has it.");
                return true; // nope nope, we can't handle this shit. There is already that number selected...
            }

            // the other check we need to make is, "is this the only module of that type in the row? If so, 

            var neighbors = GroupBuilder(source.Coordinate, space).ToList();

            var existsInAnotherSlot = false;
            foreach (var neighbor in neighbors)
            {
                if (neighbor == null || neighbor.Equals(source)) continue;

                if (space.GetSlotOptions(neighbor).Contains(m))
                {
                    existsInAnotherSlot = true;
                    break; // nope, nothing important...
                }
            }

            if (!existsInAnotherSlot)
            {
                // oh wow, its not that this module SHOULD go here... it HAS to...
                foreach (var sourceOption in sourceOptions)
                {
                    if (sourceOption == m) continue;
                    modulesToRemove.Add(sourceOption);
                }

                return false;
            }


            return false;
            
        }
        

    }

    public class SudokuConstraintGenerator : ConstraintGenerator<SudokuModule>
    {
        public override SudokuModule Copy(SudokuModule module, List<ModuleConstraint> constraints)
        {
            return new SudokuModule(module.N)
            {
                Constraints = constraints,
                Weight = module.Weight
            };
        }

        public override List<ModuleConstraint> CreateConstraints(SudokuModule source)
        {
            var constraints = new List<ModuleConstraint>();
            constraints.Add(new GroupConstraint(SudokuModule.GetRowCoordinate, GroupConstraint.GetRowNeighbors));
            constraints.Add(new GroupConstraint(SudokuModule.GetGroupCoordinate, GroupConstraint.GetBlockNeighbors));
            constraints.Add(new GroupConstraint(SudokuModule.GetColCoordinate, GroupConstraint.GetColNeighbors));
            return constraints;
        }

        public override List<ModuleConstraint> CreateConstraints(SudokuModule source, SudokuModule target)
        {
            return null;
        }

        public static void PrepareSpace(List<Slot> slots, List<SlotEdge> edges)
        {
            // slots in the same 3x3 group are neighbors...
            var slotDict = slots.ToDictionary(s => s.Coordinate);
            foreach (var slot in slots)
            {
                var blockCoord = SudokuModule.GetGroupCoordinate(slot.Coordinate);

                var rowCoord = SudokuModule.GetRowCoordinate(slot.Coordinate);
                var colCoord = SudokuModule.GetColCoordinate(slot.Coordinate);

                for (var x = 0; x < 3; x++)
                {
                    for (var y = 0; y < 3; y++)
                    {
                        var coord = new Vector3Int(x + (blockCoord.x * 3), y + (blockCoord.y * 3), 0);
                        if (slotDict.TryGetValue(coord, out var neighbor))
                        {
                            if (!Equals(neighbor, slot))
                            {

                                edges.Add(new SlotEdge
                                {
                                    Source = slot, Target = neighbor
                                });
                            }
                        }
                    }
                }
                
                for (var col = 0; col < 9; col++)
                {
                    if (col == slot.Coordinate.x) continue; // self.
                    var otherCoord = new Vector3Int(col, rowCoord.y, 0);
                    var neighbor = slotDict[otherCoord];

                    edges.Add(new SlotEdge
                    {
                        Source = slot, Target = neighbor
                    });
                }

                for (var row = 0; row < 9; row++)
                {
                    if (row == slot.Coordinate.y) continue; // self.
                    var otherCoord = new Vector3Int(colCoord.x, row, 0);
                    var neighbor = slotDict[otherCoord];

                    edges.Add(new SlotEdge
                    {
                        Source = slot, Target = neighbor
                    });
                }
            }
        }
    }
}