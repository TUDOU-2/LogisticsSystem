﻿// <auto-generated />
using System;
using LogisticsSystem.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LogisticsSystem.API.Migrations
{
    [DbContext(typeof(LogisticsSystemContext))]
    partial class LogisticsSystemContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.32");

            modelBuilder.Entity("LogisticsSystem.API.Context.AirTransport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Batch")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("CalcWeight")
                        .HasColumnType("REAL");

                    b.Property<int>("CustomerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OtherDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("OtherMoney")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("PayDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PayDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("PayMoney")
                        .HasColumnType("REAL");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("SendData")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourcePlace")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetPlace")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("AirTransport");
                });

            modelBuilder.Entity("LogisticsSystem.API.Context.AirTransportDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AirTransportId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Height")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("TEXT");

                    b.Property<double>("Length")
                        .HasColumnType("REAL");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ReceiveDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Volume")
                        .HasColumnType("REAL");

                    b.Property<double>("Weight")
                        .HasColumnType("REAL");

                    b.Property<double>("Width")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("AirTransportDetail");
                });

            modelBuilder.Entity("LogisticsSystem.API.Context.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("LogisticsSystem.API.Context.ExpressTransport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("CalcWeight")
                        .HasColumnType("REAL");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CustomerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OtherDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("OtherMoney")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("PayDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PayDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("PayMoney")
                        .HasColumnType("REAL");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("SendData")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourcePlace")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetPlace")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Volume")
                        .HasColumnType("REAL");

                    b.Property<double>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ExpressTransport");
                });

            modelBuilder.Entity("LogisticsSystem.API.Context.ExpressTransportDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExpressTransportId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Productor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ReceiveDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Volume")
                        .HasColumnType("REAL");

                    b.Property<double>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ExpressTransportDetail");
                });

            modelBuilder.Entity("LogisticsSystem.API.Context.SeaTransport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Batch")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BoxModel")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BoxNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CustomerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OtherDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("OtherMoney")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("PayDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PayDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("PayMoney")
                        .HasColumnType("REAL");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("SendData")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourcePlace")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetPlace")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SeaTransport");
                });

            modelBuilder.Entity("LogisticsSystem.API.Context.SeaTransportDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Productor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ReceiveDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("SeaTransportId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.Property<int>("USerId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Volume")
                        .HasColumnType("REAL");

                    b.Property<double>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("SeaTransportDetail");
                });

            modelBuilder.Entity("LogisticsSystem.API.Context.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
