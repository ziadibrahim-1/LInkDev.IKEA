using LinkDev.IKEA.DAL.Common.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.IKEA.DAL.Persistence.Data.Configurations.Common
{
    public class BaseEntityConfigurations<Tkey,TEntity> : IEntityTypeConfiguration<TEntity>
        where Tkey : IEquatable<Tkey>
        where TEntity : BaseEntity<Tkey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);
        } 
    }
}
