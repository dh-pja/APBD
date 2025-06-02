using Microsoft.EntityFrameworkCore;
using Tutorial10.Models;

namespace Tutorial10.DAL;

public class TutTenContext : DbContext
{
    public TutTenContext()
    {
    }
    
    public TutTenContext(DbContextOptions<TutTenContext> options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Medicament>()
            .HasKey(m => m.IdMedicament);

        modelBuilder.Entity<Prescription>()
            .HasKey(p => p.IdPrescription);

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });
    }
}