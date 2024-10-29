using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace BrewedInk.WFC.Editor
{
    public class WaveCollapTests
    {

        [Test]
        public void CanRepresentGridSpace_1X1()
        {
            var space = GenerationSpace.From2DGrid(1, 1, new ModuleSet());
            Assert.AreEqual(1, space.Slots.Count);
            Assert.AreEqual(0, space.Edges.Count);
            
            Assert.AreEqual(0, space.GetEdges(space.Slots[0]).Count);
            Assert.AreEqual(Vector3Int.zero, space.Slots[0].Coordinate);
        }
        
        
        [Test]
        public void CanRepresentGridSpace_1X3()
        {
            var space = GenerationSpace.From2DGrid(1, 3, new ModuleSet());
            Assert.AreEqual(3, space.Slots.Count);
            Assert.AreEqual(4, space.Edges.Count);

            Assert.AreEqual(1, space.GetEdges(space.Slots[0]).Count);
            
            Assert.AreEqual(Vector3Int.zero, space.Slots[0].Coordinate);
            Assert.AreEqual(new Vector3Int(0, 1, 0), space.Slots[1].Coordinate);
            Assert.AreEqual(new Vector3Int(0, 2, 0), space.Slots[2].Coordinate);


        }

        [Test]
        public void CanRepresentGridSpace_3X2()
        {
            var space = GenerationSpace.From2DGrid(3, 2, new ModuleSet());
            Assert.AreEqual(6, space.Slots.Count);
            Assert.AreEqual(14, space.Edges.Count);
            Assert.AreEqual(2, space.GetEdges(space.Slots[0]).Count);
            Assert.AreEqual(new Vector3Int(0, 0, 0), space.Slots[0].Coordinate);
            Assert.AreEqual(new Vector3Int(1, 0, 0), space.Slots[1].Coordinate);
            Assert.AreEqual(new Vector3Int(2, 0, 0), space.Slots[2].Coordinate);
            Assert.AreEqual(new Vector3Int(0, 1, 0), space.Slots[3].Coordinate);
            Assert.AreEqual(new Vector3Int(1, 1, 0), space.Slots[4].Coordinate);
            Assert.AreEqual(new Vector3Int(2, 1, 0), space.Slots[5].Coordinate);

        }

        [Test]
        public void CanRemoveModule_1X2()
        {
            var m1 = new Module();
            var m2 = new Module();
            var modules = new ModuleSet
            {
                m1, m2
            };
            var space = GenerationSpace.From2DGrid(1, 2, modules);
   
            var slot = space.Slots[0];
            Assert.AreEqual(2, space.GetSlotOptions(slot).Count());
            space.RemoveSlotOption(slot, m1);
            Assert.AreEqual(1, space.GetSlotOptions(slot).Count());

            Assert.Throws<SlotCannotHaveEmptyModuleSetException>(() => space.RemoveSlotOption(slot, m2));
        }

        [Test]
        public void Imply_1X2()
        {
            var m1 = new Module{Display = "m1"};
            var m2 = new Module{Display = "m2"};
            var modules = new ModuleSet
            {
                m1, m2
            };
            
            m1.Constraints.Add(new AdjacencyConstraint
            {
                Delta = new Vector3Int(0, 1, 0),
                NeedsOneOf = new ModuleSet(m2)
            }); // m1 needs an m2 above it. 
            
            var space = GenerationSpace.From2DGrid(1, 2, modules);
   
            var slot = space.Slots[0];
            space.RemoveSlotOption(slot, m2);
            var lowOptions = space.GetSlotOptions(slot);
            Assert.AreEqual(1, lowOptions.Count());
            Assert.AreEqual(m1, lowOptions.ToList()[0]);
            
            var highOptions = space.GetSlotOptions(space.Slots[1]);
            Assert.AreEqual(1, highOptions.Count());
            Assert.AreEqual(m2, highOptions.ToList()[0]);

        }
        
        [Test]
        public void Imply_1X2_NoImply()
        {
            var m1 = new Module{Display = "m1"};
            var m2 = new Module{Display = "m2"};
            var modules = new ModuleSet
            {
                m1, m2
            };
            
            m1.Constraints.Add(new AdjacencyConstraint
            {
                Delta = new Vector3Int(0, 1, 0),
                NeedsOneOf = new ModuleSet(m2)
            }); // m1 needs an m2 above it. 
            
            
            var space = GenerationSpace.From2DGrid(1, 2, modules);
   
            var slot = space.Slots[0];
            space.RemoveSlotOption(space.Slots[1], m2);
            var lowOptions = space.GetSlotOptions(slot);
            Assert.AreEqual(1, lowOptions.Count());
            // Assert.AreEqual(m1, lowOptions.ToList()[0]);
            
            var highOptions = space.GetSlotOptions(space.Slots[1]);
            Assert.AreEqual(1, highOptions.Count());
            Assert.AreEqual(m1, highOptions.ToList()[0]);

            Assert.Throws<SlotCannotHaveEmptyModuleSetException>(() => space.RemoveSlotOption(slot, m2));


        }
    }
}