using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tzather.Identity.Api.Entities;
using Tzather.Identity.Api.Extensions;

namespace Tzather.Identity.Api;

public class IdentityDbContext : IdentityDbContext<UserEntity, RoleEntity, Guid>, IIdentityDbContext
{
  public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    // Data Seeding
    builder.LoadData<UserEntity>();
  }
}
