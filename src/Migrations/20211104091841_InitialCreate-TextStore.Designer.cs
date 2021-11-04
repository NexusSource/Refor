﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Refor;

namespace Refor.Migrations
{
    [DbContext(typeof(ReforContext))]
    [Migration("20211104091841_InitialCreate-TextStore")]
    partial class InitialCreateTextStore
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Refor.Models.StoredText", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<Instant>("Uploaded")
                        .HasColumnType("timestamp")
                        .HasColumnName("uploaded");

                    b.HasKey("ID")
                        .HasName("pk_texts");

                    b.HasIndex("ID")
                        .HasDatabaseName("ix_texts_id");

                    b.ToTable("texts");
                });
#pragma warning restore 612, 618
        }
    }
}