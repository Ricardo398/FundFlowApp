using Fund.Core.Handlers;
using Fund.Core.Models;
using Fund.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Security.Cryptography.X509Certificates;

namespace Fund.Web.Pages.Categories
{
    public partial class GetAllCategoriesPage : ComponentBase
    {
        #region Properties
        public bool IsBusy { get; set; } = false;
        public List<Category> Categories { get; set; } = [];
        public CreateCategoryRequest InputModel { get; set; } = new();
        #endregion

        #region Services

        [Inject]
        public ICategoryHandler Handler { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public IDialogService Dialog { get; set; } = null!;
        #endregion

        #region Methods
        public async Task OnValidSubmitAsync()
        {
            {
                IsBusy = true;

                try
                {
                    var result = await Handler.CreateAsync(InputModel);
                    if (result.IsSuccess)
                    {
                        Snackbar.Add(result.Message, Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add(result.Message, Severity.Error);
                    }
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error creating category: {ex.Message}", Severity.Error);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;
            try
            {
                var request = new GetAllCategoriesRequest();
                var result = await Handler.GetAllAsync(request);
                if (result.IsSuccess)
                {
                    Categories = result.Data ?? [];
                }
                else
                {
                    Snackbar.Add(result.Message, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading categories: {ex.Message}", Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region 
        public async void OnDeleteButtonClickedAsync(long id, string title)
        {
            var result = await Dialog.ShowMessageBox(
                 "Atenção",
                 $"Ao prosseguir a categoria {title} será removida. Deseja continuar?",
                 yesText: "Excluir",
                 cancelText: "Cancelar");

            if (result is true)
            {
                await OnDeleteAsync(id, title);
                StateHasChanged();
            }
        }

        public async Task OnDeleteAsync(long id, string title)
        {
            try
            {
                var request = new DeleteCategoryRequest
                {
                    Id = id
                };
                await Handler.DeleteAsync(request);
                Categories.RemoveAll(x => x.Id == id);
                Snackbar.Add($"Categoria {title} removida.", Severity.Info);
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
        #endregion
    }
}

