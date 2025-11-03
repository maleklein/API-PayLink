using Microsoft.EntityFrameworkCore;
using PayLink.Models;

namespace PayLink.Data
{
    // Esta clase representa el "puente" entre tus modelos (Business y Payment)
    // y la base de datos real (PayLink.db). 
    // Entity Framework usa esta clase para crear y acceder a las tablas.
    public class PayLinkDbContext : DbContext
    {
        // DbSet representa una tabla en la base de datos.
        // Cada Business será una fila dentro de la tabla Businesses.
        public DbSet<Business> Businesses { get; set; }

        // Cada Payment será una fila dentro de la tabla Payments.
        public DbSet<Payment> Payments { get; set; }

        // Constructor: recibe las opciones de configuración (cadena de conexión, proveedor, etc.)
        // y las envía al constructor base de DbContext.
        public PayLinkDbContext(DbContextOptions<PayLinkDbContext> options)
            : base(options)
        {
        }

        // Este método se ejecuta cuando EF Core está creando el modelo de base de datos.
        // Sirve para definir reglas y relaciones entre las tablas.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llama al método base por si EF necesita hacer su configuración interna.
            base.OnModelCreating(modelBuilder);

            // ====================================================================
            //                CONFIGURACIÓN DE LA ENTIDAD BUSINESS
            // ====================================================================

            // Define que la clave primaria (Primary Key) será el campo Id
            modelBuilder.Entity<Business>()
                .HasKey(b => b.Id);

            // Define la propiedad Nombre:
            // - Máximo 100 caracteres
            // - Obligatoria (no puede ser nula)
            modelBuilder.Entity<Business>()
                .Property(b => b.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            // Propiedad CUIT:
            // - Máximo 15 caracteres
            // - Obligatoria
            modelBuilder.Entity<Business>()
                .Property(b => b.Cuit)
                .HasMaxLength(15)
                .IsRequired();

            // Propiedad ApiUrl:
            // - Guarda la URL de la API del negocio (por ejemplo: https://api.tiendaluna.com)
            // - Máximo 200 caracteres
            // - Obligatoria
            modelBuilder.Entity<Business>()
                .Property(b => b.ApiUrl)
                .HasMaxLength(200)
                .IsRequired();

            // Propiedad ApiKey:
            // - Guarda la clave privada que usa el negocio para autenticarse en PayLink
            // - Máximo 100 caracteres
            // - Obligatoria
            modelBuilder.Entity<Business>()
                .Property(b => b.ApiKey)
                .HasMaxLength(100)
                .IsRequired();

            // Crea un índice único en el campo CUIT:
            // evita que dos negocios diferentes tengan el mismo CUIT.
            modelBuilder.Entity<Business>()
                .HasIndex(b => b.Cuit)
                .IsUnique();

            // ====================================================================
            //                  CONFIGURACIÓN DE LA ENTIDAD PAYMENT
            // ====================================================================

            // Define que la clave primaria será el campo Id
            modelBuilder.Entity<Payment>()
                .HasKey(p => p.Id);

            // TransactionId:
            // - Identificador único del pago (por ejemplo un GUID)
            // - Obligatorio
            // - Máximo 50 caracteres
            modelBuilder.Entity<Payment>()
                .Property(p => p.TransactionId)
                .HasMaxLength(50)
                .IsRequired();

            // FacturaId:
            // - Guarda el código de la factura (Ej: F001-45)
            // - Obligatorio
            // - Máximo 50 caracteres
            modelBuilder.Entity<Payment>()
                .Property(p => p.FacturaId)
                .HasMaxLength(50)
                .IsRequired();

            // Monto:
            // - Valor decimal con dos decimales (por ejemplo 1250.50)
            // - Obligatorio
            modelBuilder.Entity<Payment>()
                .Property(p => p.Monto)
                .HasPrecision(10, 2)
                .IsRequired();

            // Fecha:
            // - Obligatoria
            modelBuilder.Entity<Payment>()
                .Property(p => p.Fecha)
                .IsRequired();

            // Estado:
            // - Puede ser "Aprobado", "Pendiente" o "Rechazado"
            // - Máximo 20 caracteres
            // - Obligatorio
            modelBuilder.Entity<Payment>()
                .Property(p => p.Estado)
                .HasMaxLength(20)
                .IsRequired();

            // ====================================================================
            // RELACIÓN ENTRE BUSINESS Y PAYMENT
            // ====================================================================

            // Define una relación de uno a muchos (1:N):
            // - Un Business puede tener varios Payments.
            // - Cada Payment pertenece a un solo Business.
            // - BusinessId es la clave foránea (FK) que une las tablas.
            // Además, OnDelete(DeleteBehavior.Restrict) evita que se borre un Business
            // si todavía tiene pagos asociados.
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Business)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
