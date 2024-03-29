﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Remake.Models
{
    public partial class kesifdbContext : DbContext
    {
        public kesifdbContext()
        {
        }

        public kesifdbContext(DbContextOptions<kesifdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alanholder> Alanholders { get; set; } = null!;
        public virtual DbSet<Kategoriler> Kategorilers { get; set; } = null!;
        public virtual DbSet<Kesifler> Kesiflers { get; set; } = null!;
        public virtual DbSet<Kesifmekanholder> Kesifmekanholders { get; set; } = null!;
        public virtual DbSet<Kullanıcı> Kullanıcıs { get; set; } = null!;
        public virtual DbSet<Mekantürleri> Mekantürleris { get; set; } = null!;
        public virtual DbSet<Paket> Pakets { get; set; } = null!;
        public virtual DbSet<Paketholder> Paketholders { get; set; } = null!;
        public virtual DbSet<Roller> Rollers { get; set; } = null!;
        public virtual DbSet<Stokısenabled> Stokısenableds { get; set; } = null!;
        public virtual DbSet<Urunholder> Urunholders { get; set; } = null!;
        public virtual DbSet<Urunler> Urunlers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=127.0.0.1;database=kesifdb;uid=root;pwd=mysql8.7", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Alanholder>(entity =>
            {
                entity.ToTable("alanholder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AlanAdi).HasMaxLength(80);

                entity.Property(e => e.Konum).HasMaxLength(200);

                entity.Property(e => e.Not).HasMaxLength(200);
            });

            modelBuilder.Entity<Kategoriler>(entity =>
            {
                entity.ToTable("kategoriler");

                entity.Property(e => e.KategoriAdi).HasMaxLength(100);
            });

            modelBuilder.Entity<Kesifler>(entity =>
            {
                entity.ToTable("kesifler");

                entity.Property(e => e.Ad)
                    .HasMaxLength(150)
                    .HasColumnName("ad");
            });

            modelBuilder.Entity<Kesifmekanholder>(entity =>
            {
                entity.ToTable("kesifmekanholder");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<Kullanıcı>(entity =>
            {
                entity.ToTable("kullanıcı");

                entity.HasIndex(e => e.RolId, "fk_roller");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ad).HasMaxLength(40);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Psswrd)
                    .HasMaxLength(60)
                    .HasColumnName("psswrd");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Kullanıcıs)
                    .HasForeignKey(d => d.RolId)
                    .HasConstraintName("fk_roller");
            });

            modelBuilder.Entity<Mekantürleri>(entity =>
            {
                entity.ToTable("mekantürleri");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MekanAdi).HasMaxLength(50);
            });

            modelBuilder.Entity<Paket>(entity =>
            {
                entity.ToTable("paket");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PaketAdi).HasMaxLength(60);
            });

            modelBuilder.Entity<Paketholder>(entity =>
            {
                entity.ToTable("paketholder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PaketId).HasColumnName("paketId");

                entity.Property(e => e.UrunAdeti).HasColumnName("urunAdeti");

                entity.Property(e => e.UrunId).HasColumnName("urunId");
            });

            modelBuilder.Entity<Roller>(entity =>
            {
                entity.HasKey(e => e.RolId)
                    .HasName("PRIMARY");

                entity.ToTable("roller");

                entity.Property(e => e.RolAdi).HasMaxLength(30);
            });

            modelBuilder.Entity<Stokısenabled>(entity =>
            {
                entity.ToTable("stokısenabled");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.IsEnabled).HasColumnName("isEnabled");
            });

            modelBuilder.Entity<Urunholder>(entity =>
            {
                entity.ToTable("urunholder");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<Urunler>(entity =>
            {
                entity.ToTable("urunler");

                entity.Property(e => e.KullanilanUrunAdet).HasDefaultValueSql("'0'");

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.UrunAdi).HasMaxLength(100);

                entity.Property(e => e.UrunKategorisi).HasMaxLength(50);

                entity.Property(e => e.UrunKodu).HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
