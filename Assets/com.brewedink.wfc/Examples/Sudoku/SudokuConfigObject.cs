using UnityEngine;
namespace BrewedInk.WFC.Examples.Sudoku
{

    [CreateAssetMenu(fileName="New Sudoku Config", menuName="BrewedInk WFC/Sudoku Config")]
    public class SudokuConfigObject : WCFConfigObject<SudokuModuleObject, SudokuModule>
    {
        protected override GenerationSpace CreateSpace()
        {
            var constraintSolver = new SudokuConstraintGenerator();
            var space = GenerationSpace.From2DGrid(9, 9, GetModules().ProduceConstraints(constraintSolver),
                useSeed ? seed : default, SudokuConstraintGenerator.PrepareSpace);

            return space;
        }

        public override bool TryGetSprite(Module module, out Sprite sprite)
        {
            if (TryGetObject(module, out var obj))
            {
                sprite = obj.PreviewSprite;
                return true;
            }

            sprite = null;
            return false;
        }
    }

    [System.Serializable]
    public class SudokuModuleObject : ModuleObject<SudokuModule>
    {
        public Sprite PreviewSprite;
    }
}
