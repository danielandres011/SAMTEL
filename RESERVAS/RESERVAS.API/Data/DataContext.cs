using Microsoft.EntityFrameworkCore;
using RESERVAS.API.Entities;
using RESERVAS.API.Models;

namespace RESERVAS.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        
        /// <summary>
        /// Definición de tablas
        /// </summary>
        public DbSet<Usuario> USUARIOS { get; set; }
        public DbSet<Hotel> HOTELES { get; set; }
        public DbSet<Reserva> RESERVAS { get; set; }


        /// <summary>
        /// Sobreescribe la función OnModelCreating para agregar caracteres unicos.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().HasIndex(x => x.EMAIL).IsUnique();
            modelBuilder.Entity<Hotel>().HasIndex(x => x.NOMBRE).IsUnique();
        }

        /// <summary>
        /// Sobre escribe la función ChangesAsync para que se ejecute la función AddTimestamps antes de hacer cambios.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Sobre escribe la función ChangesAsync para que se ejecute la función AddTimestamps antes de hacer cambios.
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifica si es una clase que hereda de ISoftDelete realice el proceso.
        /// Agrega la fecha en que se modifico y elimino y cambia la entrada para que nunca se elimine el registro de la base de datos.
        /// </summary>
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is ISoftDetete && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var entity in entities)
            {
                DateTime DateNow = DateTime.Now;
                if (entity.State == EntityState.Added)
                {
                    ((ISoftDetete)entity.Entity).CREATED = DateNow;
                }
                ((ISoftDetete)entity.Entity).UPDATED = DateNow;
                if (entity.State == EntityState.Deleted)
                {
                    ((ISoftDetete)entity.Entity).DELETED = DateNow;
                    entity.State = EntityState.Modified;
                }
            }
        }

    }
}
