using System.Collections.Generic;
using System.Diagnostics.Contracts;
using RecipeManagement.Core;

namespace RecipeManagement.Tests;

/// <summary>
/// Example tests from the assignment specification. Add your own tests as you work.
/// </summary>
public sealed class RecipeManagerTests
{
    [Fact]
    public void Constructor_BuildsRecipeDictionary()
    {
        var manager = CreateManager();
        Assert.Equal(2, manager.RecipeCount);
        Assert.Equal("Recipe A", manager.FindRecipe(10)?.Title);
    }

    [Fact]
    public void InstructionsAreCompletedInFileOrder()
    {
        var manager = CreateManager();
        Assert.True(manager.StartCooking(10));
        Assert.Equal("First step", manager.PeekNextInstruction());
        Assert.Equal("First step", manager.CompleteNextInstruction());
        Assert.Equal("Second step", manager.PeekNextInstruction());
    }

    [Fact]
    public void RemovedRecipesAreRestoredLastInFirstOut()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);
        manager.RemoveRecipeFromCookingPlan(10);
        manager.RemoveRecipeFromCookingPlan(20);
        Assert.Equal(20, manager.PeekLastRemovedRecipe());
        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 20 }, manager.GetCookingPlan());
    }

    private static RecipeManager CreateManager()
    {
        return new RecipeManager(new[]
        {
            new Recipe
            {
                Id = 10,
                Title = "Recipe A",
                Ingredients = new() { "1 apple" },
                Instructions = new() { "First step", "Second step" }
            },
            new Recipe
            {
                Id = 20,
                Title = "Recipe B"
            }
        });
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenRecipesIsNull()
    {
        IEnumerable<Recipe>? recipes = null;
        void CreateManager()
        {
            RecipeManager manager = new RecipeManager(recipes!);
        }
        Assert.Throws<ArgumentNullException>(CreateManager);
    }
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenRecipeIdIsZero()
    {
        Recipe recipe = new Recipe{};
        recipe.Id = 0;
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        void CreateManager()
        {
            RecipeManager manager = new RecipeManager(recipes);
        }
        Assert.Throws<ArgumentException>(CreateManager);
    }
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenRecipeIdIsNegative()
    {
        Recipe recipe = new Recipe{};
        recipe.Id = -1;
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        void CreateManager()
        {
            RecipeManager manager = new RecipeManager(recipes);
        }
        Assert.Throws<ArgumentException>(CreateManager);
    }
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenRecipeTitleIsEmpty()
    {
        Recipe recipe = new Recipe{};
        recipe.Title="";
        recipe.Id = 1;
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        void CreateManager()
        {
            RecipeManager manager = new RecipeManager(recipes);
        }
        Assert.Throws<ArgumentException>(CreateManager);
    }
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenRecipeTitleIsWhitespace()
    {
        Recipe recipe = new Recipe{};
        recipe.Title = "   ";
        recipe.Id = 1;
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        void CreateManager()
        {
            RecipeManager manager = new RecipeManager(recipes);
        }
        Assert.Throws<ArgumentException>(CreateManager);
    }
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenRecipeTitleIsNull()
    {
        Recipe recipe = new Recipe{};
        recipe.Title = null!;
        recipe.Id = 1;
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        void CreateManager()
        {
            RecipeManager manager = new RecipeManager(recipes);
        }
        Assert.Throws<ArgumentException>(CreateManager);
    }
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenRecipeIdIsDuplicated()
    {
        Recipe recipe1 = new Recipe{};
        Recipe recipe2 = new Recipe{};
        recipe1.Id = 1;
        recipe1.Title = "a dish";
        recipe2.Id = 1;
        recipe2.Title = "another dish";
        List<Recipe> recipes = new List<Recipe>{recipe1, recipe2};
        void CreateManager()
        {
            RecipeManager manager = new RecipeManager(recipes);
        }
        Assert.Throws<ArgumentException>(CreateManager);
    }
    [Fact]
    public void Constructor_BuildsRecipeDictionary_WhenRecipesAreValid()
    {
        Recipe recipe1 = new Recipe{};
        Recipe recipe2 = new Recipe{};
        recipe1.Id = 1;
        recipe1.Title = "pizza";
        recipe2.Id = 2;
        recipe2.Title = "dumpling";
        List<Recipe> recipes = new List<Recipe>{recipe1, recipe2};
        RecipeManager manager = new RecipeManager(recipes);
        Recipe? result1 = manager.FindRecipe(1);
        Recipe? result2 = manager.FindRecipe(2);
        Assert.Equal(recipe1,result1);
        Assert.Equal(recipe2,result2);
    }

    [Fact]
    public void AddRecipe_ThrowsArgumentNullException_WhenRecipeIsNull()
    {
        Recipe? recipe = null;
        RecipeManager manager = new RecipeManager(new List<Recipe>());
        void AddNullRecipe()
        {
            manager.AddRecipe(recipe!);
        }
        Assert.Throws<ArgumentNullException>(AddNullRecipe);
    }
    
    [Fact]
    public void AddRecipe_ReturnFalse_WhenRecipeIdIsNotPositive()
    {
        Recipe? recipe = new Recipe();
        recipe.Id = -1;
        recipe.Title = "a dish";
        RecipeManager manager = new RecipeManager(new List<Recipe>());
        bool result = manager.AddRecipe(recipe);
        Assert.False(result);
    }

    [Fact]
    public void AddRecipe_ReturnFalse_WhenRecipeTitleIsBlank()
    {
        Recipe? recipe = new Recipe();
        recipe.Title = "   ";
        recipe.Id = 1;
        RecipeManager manager = new RecipeManager(new List<Recipe>());
        bool result = manager.AddRecipe(recipe);
        Assert.False(result);
    }

    [Fact]
    public void AddRecipe_ReturnFalse_WhenRecipeIdIsAlreadyExists()
    {
        Recipe? recipe1 = new Recipe();
        recipe1.Id = 1;
        recipe1.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe1);
        RecipeManager manager = new RecipeManager(new List<Recipe>(recipes));
        Recipe? recipe2 = new Recipe();
        recipe2.Id = 1;
        recipe2.Title = "another dish";
        bool result = manager.AddRecipe(recipe2);
        Assert.False(result);
    }

    [Fact]
    public void AddRecipe_ReturnsTrue_WhenRecipeIsValid()
    {
        Recipe? recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        RecipeManager manager = new RecipeManager(new List<Recipe>());
        bool result = manager.AddRecipe(recipe);
        Assert.True(result);
    }

    [Fact]
    public void FindRecipe_ReturnRecipe_WhenRecipeIdExists()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        Recipe? result = manager.FindRecipe(1);
        Assert.Equal(recipe, result);
    }

    [Fact]
    public void FindRecipe_ReturnsNull_WhenRecipeIdDoesNotExist()
    {
        Recipe recipe = new Recipe();
         recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        Recipe? result = manager.FindRecipe(2);
        Assert.Null(result);
    }

    [Fact]
    public void RemoveRecipe_ReturnsFalse_WhenRecipeIdDoesNotExist()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        bool result = manager.RemoveRecipe(2);
        Assert.False(result);
    }

    [Fact]
    public void RemoveRecipe_ReturnsFalse_WhenRecipeIsInCookingPlan()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        manager.AddRecipeToCookingPlan(1);
        bool result = manager.RemoveRecipe(1);
        Assert.False(result);
    }

    [Fact]
    public void RemoveRecipe_ReturnsTure_WhenRecipeCanBeRemoved()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        bool result = manager.RemoveRecipe(1);
        Assert.True(result);
    }

    [Fact]
    public void AddIngredientsToShoppingList_ReturnsZero_WhenRecipeIdDoesNotExist()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        recipe.Ingredients = new List<string>{"tomato","beef"};
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        int result = manager.AddIngredientsToShoppingList(2);
        Assert.Equal(0, result);
    }

    [Fact]
    public void AddIngredientsToShoppingList_AddsIngredients_WhenRecipeIdExists()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        recipe.Ingredients = new List<string>{"tomato", "beef"};
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        int result = manager.AddIngredientsToShoppingList(1);
        Assert.Equal(2, result);
        IReadOnlyList<string> shoppingList = manager.GetShoppingList();
        Assert.Equal("tomato", shoppingList[0]);
        Assert.Equal("beef", shoppingList[1]);
    }

    [Fact]
    public void GetShoppingList_ReturnsShoppingList_WhenItemsExist()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        recipe.Ingredients = new List<string>{"beef","tomato"};
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        manager.AddIngredientsToShoppingList(1);
        IReadOnlyList<string> result = manager.GetShoppingList();
        Assert.Equal("beef", result[0]);
        Assert.Equal("tomato", result[1]);
    }

    [Fact]
    public void ClearShoppingList_RemovesAllItems()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        recipe.Ingredients = new List<string>{"tomato", "beef"};
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        manager.AddIngredientsToShoppingList(1);
        manager.ClearShoppingList();
        IReadOnlyList<string> result = manager.GetShoppingList();
        Assert.Empty(result);
    }
    
    [Fact]
    public void AddRecipeToCookingPlan_ReturnsFalse_WhenRecipeDoesNotExist()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        bool result = manager.AddRecipeToCookingPlan(2);
        Assert.False(result);
    }

    [Fact]
    public void AddRecipeToCookingPlan_ReturnsFalse_WhenRecipeAlreadyExistsInCookingPlan()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        manager.AddRecipeToCookingPlan(1);
        bool result = manager.AddRecipeToCookingPlan(1);
        Assert.False(result);
    }

    [Fact]
    public void AddRecipeToCookingPlan_ReturnsTrue_WhenRecipeExists()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        bool result = manager.AddRecipeToCookingPlan(1);
        Assert.True(result);
    }

    [Fact]
    public void RemoveRecipeFromCookingPlan_ReturnFalse_WhenRecipeIsNotInCookingPlan()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        bool result = manager.RemoveRecipeFromCookingPlan(1);
        Assert.False(result);
    }

    [Fact]
    public void RemoveRecipeFromCookingPlan_ReturnTrue_WhenRecipeIsInCookingPlan()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        manager.AddRecipeToCookingPlan(1);
        bool result = manager.RemoveRecipeFromCookingPlan(1);
        Assert.True(result);
    }

    [Fact]
    public void RestoreLastRemovedRecipe_ReturnsFalse_WhenRemovedRecipeStackIsEmpty()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        bool result = manager.RestoreLastRemovedRecipe();
        Assert.False(result);
    }

    [Fact]
    public void RestoreLastRemovedRecipe_ReturnsFalse_WhenRecipeIsAlreadyInCookingPlan()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        manager.AddRecipeToCookingPlan(1);
        manager.RemoveRecipeFromCookingPlan(1);
        manager.AddRecipeToCookingPlan(1);
        bool result = manager.RestoreLastRemovedRecipe();
        Assert.False(result);
    }

    [Fact]
    public void RestoreLastRemovedRecipe_ReturnsTrue_WhenRecipeCanBeRestored()
    {
        Recipe recipe = new Recipe();
        recipe.Id = 1;
        recipe.Title = "a dish";
        List<Recipe> recipes = new List<Recipe>();
        recipes.Add(recipe);
        RecipeManager manager = new RecipeManager(recipes);
        manager.AddRecipeToCookingPlan(1);
        manager.RemoveRecipeFromCookingPlan(1);
        bool result = manager.RestoreLastRemovedRecipe();
        Assert.True(result);
    }
}
