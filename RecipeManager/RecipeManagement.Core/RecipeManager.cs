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
    private Dictionary<int, Recipe> RecipeDictionary = new Dictionary<int, Recipe>();
    private List<string> ShoppingList = new List<string>();
    private LinkedList<int> CookingPlan = new LinkedList<int>();
    private Queue<string> CookingInstructions = new Queue<string>();
    private Stack<int> RemovedRecipe = new Stack<int>();

    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        // TODO Part A: validate recipes and build Dictionary<int, Recipe>.
        _ = recipes;

        if (recipes == null)
        {
            throw new ArgumentNullException(nameof(recipes));
        }

        foreach(Recipe recipe in recipes)
        {
            if(recipe.Id <= 0)
            {
                throw new ArgumentException("Recipe ID must be positive");
            }

            if (string.IsNullOrWhiteSpace(recipe.Title))
            {
                throw new ArgumentException("Recipe title cannot be blank or null");
            }

            if (RecipeDictionary.ContainsKey(recipe.Id))
            {
                throw new ArgumentException("Recipe ID cannot be repeated");
            }

            RecipeDictionary.Add(recipe.Id, recipe);
        }
    }

    public int RecipeCount => 0;
    public int ShoppingItemCount => 0;
    public int CookingPlanCount => 0;
    public int PendingInstructionCount => 0;
    public int RemovedRecipeCount => 0;

    public bool AddRecipe(Recipe recipe)
    {
        if(recipe == null)
        {
            throw new ArgumentNullException(nameof(recipe));
        }
        if(recipe.Id < 0)
        {
            return false;
        }
        if(string.IsNullOrWhiteSpace(recipe.Title))
        {
            return false;
        }
        if(RecipeDictionary.ContainsKey(recipe.Id)){
            return false;
        }
        RecipeDictionary.Add(recipe.Id, recipe);
        return true;
    }

    public Recipe? FindRecipe(int recipeId)
    {
        if(RecipeDictionary.ContainsKey(recipeId))
        {
            return RecipeDictionary[recipeId];
        }
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
        ShoppingList.AddRange(RecipeDictionary[recipeId].Ingredients);
        return RecipeDictionary[recipeId].Ingredients.Count;
    }

    public IReadOnlyList<string> GetShoppingList(){
        return ShoppingList;
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
