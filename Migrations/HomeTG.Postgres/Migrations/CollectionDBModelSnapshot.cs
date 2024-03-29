﻿// <auto-generated />
using System;
using HomeTG.API.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeTG.Postgres.Migrations
{
    [DbContext(typeof(CollectionDB))]
    partial class CollectionDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HomeTG.API.Models.Collection", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.HasKey("Id");

                    b.ToTable("collection");
                });

            modelBuilder.Entity("HomeTG.API.Models.CollectionCard", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("uuid");

                    b.Property<string>("CollectionId")
                        .HasColumnType("text")
                        .HasColumnName("collection");

                    b.Property<int>("FoilQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("foilquantity");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<DateTime?>("TimeAdded")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timeadded");

                    b.HasKey("Id", "CollectionId");

                    b.HasIndex("Id", "CollectionId");

                    b.ToTable("cards");
                });
#pragma warning restore 612, 618
        }
    }
}
