using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tzather.BaseApi;
using Tzather.Identity.Api.Entities;

namespace Tzather.Identity.Api;

public class IdentityDbContext : IdentityDbContext<UserEntity, RoleEntity, Guid>, IIdentityDbContext
{
  public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    // Data Seeding
    builder.LoadData<UserEntity>();
    builder.LoadData<RoleEntity>();
    builder.LoadData<IdentityUserClaim<Guid>>();
    builder.LoadData<IdentityUserRole<Guid>>();

  }
}
