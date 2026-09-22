using System;
using System.Collections.Generic;

namespace RecipeManagement.Core;

/// <summary>
/// Implement this class using the five Part A collections as private fields:
/// Dictionary&lt;int, Recipe&gt;, List&lt;string&gt;, LinkedList&lt;int&gt;,
/// Stack&lt;int&gt; and Queue&lt;string&gt;.
/// </summary>
public sealed class RecipeManager : IRecipeManager
{
    // TODO Part A: add your private collection fields here.
    // Stores recipes by unique recipe ID
    private Dictionary<int, Recipe> RecipeDictionary = new Dictionary<int, Recipe>();
    // Stores shopping list ingredient strings
    private List<string> ShoppingList = new List<string>();
    // Stores recipe IDs in the current cooking plan.
    private LinkedList<int> CookingPlan = new LinkedList<int>();
    // Stores cooking instructions in the order which should be processed.
    private Queue<string> CookingInstructions = new Queue<string>();
    // Stores recently removed recipe IDs.
    private Stack<int> RemovedRecipe = new Stack<int>();

    /// <summary>
    /// Initializes a new RecipeManager and builds the recipe dictionary from JSON file.
    /// </summary>
    /// <param name="recipes">The collection of recipes used to build the recipe dictionary.</param>
    /// <exception cref="ArgumentNullException">Throw an error when the recipes collection is null</exception>
    /// <exception cref="ArgumentException">Throw an error when a recipe has a non-positive ID, a blank title, or a duplicate ID/</exception>
    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        // TODO Part A: validate recipes and build Dictionary<int, Recipe>.
        _ = recipes;
        // Check if recipes are null, then throw an error.
        if (recipes == null)
        {
            throw new ArgumentNullException(nameof(recipes));
        }
        // Validate every recipe and add it to the dictionary.
        foreach(Recipe recipe in recipes)
        {
            // Recipe IDs must be positive.
            if(recipe.Id <= 0)
            {
                throw new ArgumentException("Recipe ID must be positive");
            }

            // Recipe titles cannot be null, empty or whitespace.
            if (string.IsNullOrWhiteSpace(recipe.Title))
            {
                throw new ArgumentException("Recipe title cannot be blank or null");
            }

            // Every recipe ID must be unique.
            if (RecipeDictionary.ContainsKey(recipe.Id))
            {
                throw new ArgumentException("Recipe ID cannot be repeated");
            }

            // Add the validated recipe using its ID as the dictionary key.
            RecipeDictionary.Add(recipe.Id, recipe);
        }
    }

    public int RecipeCount => 0;
    public int ShoppingItemCount => 0;
    public int CookingPlanCount => 0;
    public int PendingInstructionCount => 0;
    public int RemovedRecipeCount => 0;

    /// <summary>
    /// Adds a new recipe to the recipe dictionary.
    /// </summary>
    /// <param name="recipe">recipe to be added</param>
    /// <returns>if the recipe is validated and added successfully, return ture; otherwise false.</returns>
    /// <exception cref="ArgumentNullException">Throw an error when recipe is null</exception>
    public bool AddRecipe(Recipe recipe)
    {
        //Reject a null recipe
        if(recipe == null)
        {
            throw new ArgumentNullException(nameof(recipe));
        }
        //Recipe IDs must be positive
        if(recipe.Id <= 0)
        {
            return false;
        }
        // Recipe title cannot be null, empry or whitespace.
        if(string.IsNullOrWhiteSpace(recipe.Title))
        {
            return false;
        }
        // Recipe IDs should be unique.
        if(RecipeDictionary.ContainsKey(recipe.Id)){
            return false;
        }
        // Add the validated recipe to the dictionary.
        RecipeDictionary.Add(recipe.Id, recipe);
        return true;
    }

    /// <summary>
    /// Finds a recipe by its recipe ID.
    /// </summary>
    /// <param name="recipeId">The ID of the recipe to find.</param>
    /// <returns>The matching Recipe if the ID exists; otherwise null.</returns>
    public Recipe? FindRecipe(int recipeId)
    {
        // Check whether the recipe ID exists in the dictionary.
        if(RecipeDictionary.ContainsKey(recipeId))
        {
            // Return the matched recipe.
            return RecipeDictionary[recipeId];
        }
        // If it does not exist, return null.
        return null;
    }

    public bool RemoveRecipe(int recipeId)
    {
        if (!RecipeDictionary.ContainsKey(recipeId) || CookingPlan.Contains(recipeId))
        {
            return false;
        }
        RecipeDictionary.Remove(recipeId);
        return true;
    }

    public int AddIngredientsToShoppingList(int recipeId)
    {
        if (!RecipeDictionary.ContainsKey(recipeId))
        {
            return 0;
        }
        ShoppingList.AddRange(RecipeDictionary[recipeId].Ingredients);
        return RecipeDictionary[recipeId].Ingredients.Count;
    }

    public IReadOnlyList<string> GetShoppingList(){
        return ShoppingList.AsReadOnly();
    }

    public void ClearShoppingList(){
        ShoppingList.Clear();
    }
    public bool AddRecipeToCookingPlan(int recipeId) =>
        throw new NotImplementedException("Part A: implement AddRecipeToCookingPlan.");

    public bool RemoveRecipeFromCookingPlan(int recipeId) =>
        throw new NotImplementedException("Part A: implement RemoveRecipeFromCookingPlan.");

    public bool RestoreLastRemovedRecipe() =>
        throw new NotImplementedException("Part A: implement RestoreLastRemovedRecipe.");

    public int? PeekLastRemovedRecipe() =>
        throw new NotImplementedException("Part A: implement PeekLastRemovedRecipe.");

    public IReadOnlyList<int> GetCookingPlan() =>
        throw new NotImplementedException("Part A: implement GetCookingPlan.");

    public bool StartCooking(int recipeId) =>
        throw new NotImplementedException("Part A: implement StartCooking.");

    public string? PeekNextInstruction() =>
        throw new NotImplementedException("Part A: implement PeekNextInstruction.");

    public string? CompleteNextInstruction() =>
        throw new NotImplementedException("Part A: implement CompleteNextInstruction.");

    public IReadOnlyList<Recipe> SearchByTitle(string searchText) =>
        throw new NotImplementedException("Part B: implement SearchByTitle.");

    public IReadOnlyList<Recipe> SearchByIngredient(string searchText) =>
        throw new NotImplementedException("Part B: implement SearchByIngredient.");

    public IReadOnlyList<Recipe> GetHighestProteinRecipes(int count) =>
        throw new NotImplementedException("Part B: implement GetHighestProteinRecipes.");

    public bool AddSavedRecipe(int recipeId) =>
        throw new NotImplementedException("Part B: implement AddSavedRecipe.");

    public bool RemoveSavedRecipe(int recipeId) =>
        throw new NotImplementedException("Part B: implement RemoveSavedRecipe.");

    public bool IsRecipeSaved(int recipeId) =>
        throw new NotImplementedException("Part B: implement IsRecipeSaved.");

    public IReadOnlyList<int> GetSavedRecipes() =>
        throw new NotImplementedException("Part B: implement GetSavedRecipes.");
}
