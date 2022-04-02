using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TodoList.Extensions;

public static class ModelStateExtensions
{
    public static bool IsNotValid(this ModelStateDictionary model)
    {
        return !model.IsValid;
    }
}
