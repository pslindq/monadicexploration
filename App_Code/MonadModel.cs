using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

public partial class MonadModel : DbContext
{
    public MonadModel()
        : base("name=MonadModel")
    {
    }

    public virtual DbSet<Monad> Monads { get; set; }
    public virtual DbSet<NodeLink> NodeLinks { get; set; }
    public virtual DbSet<Node> Nodes { get; set; }
    public virtual DbSet<NodeType> NodeTypes { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Monad>()
            .Property(e => e.Title)
            .IsUnicode(false);

        modelBuilder.Entity<Monad>()
            .Property(e => e.URL)
            .IsUnicode(false);

        modelBuilder.Entity<Monad>()
            .Property(e => e.URLSegment)
            .IsUnicode(false);

        modelBuilder.Entity<Monad>()
            .Property(e => e.AdminPWD)
            .IsUnicode(false);

        modelBuilder.Entity<NodeLink>()
            .Property(e => e.UniqueID)
            .IsUnicode(false);

        modelBuilder.Entity<Node>()
            .Property(e => e.Title)
            .IsUnicode(false);

        modelBuilder.Entity<Node>()
            .Property(e => e.Text)
            .IsUnicode(false);

        modelBuilder.Entity<Node>()
            .Property(e => e.URL)
            .IsUnicode(false);

        modelBuilder.Entity<Node>()
            .HasMany(e => e.NodeLinks)
            .WithRequired(e => e.Node)
            .HasForeignKey(e => e.NodeID1)
            .WillCascadeOnDelete(false);

        modelBuilder.Entity<Node>()
            .HasMany(e => e.NodeLinks1)
            .WithRequired(e => e.Node1)
            .HasForeignKey(e => e.NodeID2)
            .WillCascadeOnDelete(false);

        modelBuilder.Entity<NodeType>()
            .Property(e => e.Name)
            .IsUnicode(false);

        modelBuilder.Entity<NodeType>()
            .Property(e => e.PluralName)
            .IsUnicode(false);

        modelBuilder.Entity<NodeType>()
            .Property(e => e.SlugName)
            .IsUnicode(false);

        modelBuilder.Entity<NodeType>()
            .Property(e => e.Color)
            .IsUnicode(false);
    }
}