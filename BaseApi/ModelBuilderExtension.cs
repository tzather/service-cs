using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Tzather.BaseApi;

public static class ModelBuilderExtension
{
  public static void LoadData<TEntity>(this ModelBuilder builder) where TEntity : class
  {
    var jsonData = File.ReadAllText($"Dataload/{typeof(TEntity).Name}.json");
    var dataList = JsonSerializer.Deserialize<List<TEntity>>(jsonData);
    if (dataList != null)
    {
      builder.Entity<TEntity>().HasData(dataList);
    }
  }
}