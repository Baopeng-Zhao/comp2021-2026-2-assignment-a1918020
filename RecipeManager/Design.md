# Design

Complete this document for **Part B**.

## 1. Saved recipe collection

Name the C# collection you selected for the saved/favourite feature and explain why it suits add, remove, duplicate prevention and membership checks.
Answer: I selected List<int> to store saved recipe IDs because it is simple to use and supports adding, removing and searching for ricepe IDs.

The List<int> collection has the simplest addition and deletion operations. Since its data type is int, and int corresponds to the recipeID. Because each entity in the Recipe class has a unique recipeID, this choice can effectively prevent duplicates. Although the List type allows duplicate int-type recipeIDs to be added, in the AddSavedRecipe method, the if statement limits the addition of duplicate recipeIDs. This recipeID selection is very convenient for membership checks.

List<int> is suitable for the feature because it is easy to manage and provides all the required operations.

## 2. Integration

In a short paragraph, explain how the new Part B features use the existing Part A `RecipeManager` and recipe catalogue.

Answer: The Part B features utilize the RecipeManager and RecipeDictionary that were completed in Part A. The implementation of the Search function is all based on the results of LINQ in RecipeDictionary. The implementation of the GetHighestProteinRecipes method is based on the protein_G in the Recipe class being implemented on the dictionary. The features of Part B are all implemented without altering the structure of Part A and based on that structure.
## 3. Basic complexity

| Operation | Structure | Expected complexity | Reason |
| --- | --- | --- | --- |
| Lookup recipe by ID | Dictionary | Average O(1) | Hash-based key lookup. |
| Traverse cooking plan | LinkedList | O(n) | Each planned recipe may need to be visited. |
| Complete next instruction | Queue | O(1) | The item at the front is removed. |
| LINQ title/ingredient search | Recipe collection | O(n) | Each recipe may need to be inspected. |
| Check whether a recipe is saved | List | O(n) | Contains() checks every recipe IDs until it find matched one |
