The purpose of the Wave Function Collapse (WFC) algorithm is to procedurally solve problems that could have many solutions. The algorithm was popularized in this [Github Project by mxgmn](https://github.com/mxgmn/WaveFunctionCollapse). 

There are already several fantastic explanations of the WFC algorithm.
* [Robert Heaton](https://robertheaton.com/2018/12/17/wavefunction-collapse-algorithm/)

***

## Quick Terms

#### Slot
A Slot is a physical spot in the WFC. When the WFC starts, each Slot has many Module possibilities. When the WFC ends, each Slot has one possible Module. For example, in a 2D world generation demo, each cell in the 2D grid is a Slot.

#### Module
A Module is a potential outcome for a Slot. Modules are placed on Slots. For example, in a 2D world generation demo, a Module could be a tile of grass, a tile of water, a tile of sky, or any other world piece. A Module has a set of constraints about how it can be placed. When a constraint is invalidated for a Slot, the Module becomes invalid for the Slot.

#### Superposition
The superposition is the set of possible Modules per Slot. As the WFC algorithm runs, modules are removed as possibilities from each Slot, and the superposition shrinks. Eventually, the superposition completely collapses such that there is only one possible Module per Slot.

#### Slot Entropy
Entropy is a rough measure of how many Modules are possible per Slot. At the start of the WFC, the Entropy of each Slot is high, because all Modules are possible per Slot. As the algorithm runs, the Entropy lowers. The Entropy is similar to the amplitude of the superposition. The entropy of a slot is also related to the weights of the modules still available at that slot. Modules with higher weights produce lower entropies. 

#### Module Constraints
Each Module has a set of Constraints that control how the Module can be placed on Slots. A common example is an Adjacency Constraint. A Module, X, may have an Adjacency Constraint that says, "only Modules A, B, or C can exist above this Module". If a Slot doesn't have Modules A, B, or C in its superposition, then the Slot below that cannot contain Module X, because the Adjacency constraint isn't valid.



***

