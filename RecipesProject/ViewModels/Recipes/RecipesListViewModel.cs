﻿namespace RecipesProject.ViewModels.Recipes
{
    public class RecipesListViewModel
    {
        public IEnumerable<RecipeInListViewModel> Recipes { get; set; }

        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1; 

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int PreviousPageNumer => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;

        public int PagesCount => (int)Math.Ceiling((double)this.RecipesCount / this.ItemsPerPage);    

        public int RecipesCount { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
