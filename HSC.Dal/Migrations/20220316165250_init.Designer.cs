﻿// <auto-generated />
using System;
using HSC.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HSC.Dal.Migrations
{
    [DbContext(typeof(HSCContext))]
    [Migration("20220316165250_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.Property<int>("GroupsId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("GroupsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("HSC.Dal.Entities.Block", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("BlockedUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BlockerUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Block", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.Challenge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<TimeSpan>("Increment")
                        .HasColumnType("time");

                    b.Property<double>("MaximumBet")
                        .HasColumnType("float");

                    b.Property<double>("MinimumBet")
                        .HasColumnType("float");

                    b.Property<string>("Offerer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Receiver")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("TimeLimit")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("Challenge", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.ChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TimeStamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("HSC.Dal.Entities.FriendRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ReceiverUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequesterUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FriendRequest", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Group", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BlackRating")
                        .HasColumnType("int");

                    b.Property<string>("BlackUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CurrentBet")
                        .HasColumnType("float");

                    b.Property<TimeSpan>("Increment")
                        .HasColumnType("time");

                    b.Property<double>("MaximumBet")
                        .HasColumnType("float");

                    b.Property<double>("MinimumBet")
                        .HasColumnType("float");

                    b.Property<string>("Moves")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<TimeSpan>("TimeLimit")
                        .HasColumnType("time");

                    b.Property<int?>("TournamentId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("WhiteRating")
                        .HasColumnType("int");

                    b.Property<string>("WhiteUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Match", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.Tournament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("BuyIn")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("GameIncrement")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("GameTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("Length")
                        .HasColumnType("time");

                    b.Property<double>("PrizePool")
                        .HasColumnType("float");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("WinnerUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tournament", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.TournamentMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TimeStamp")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentMessage", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.TournamentPlayer", b =>
                {
                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double?>("Points")
                        .HasColumnType("float");

                    b.HasKey("TournamentId", "UserName");

                    b.ToTable("TournamentPlayer", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastPlayMoneyRedeemDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PlayMoneyBalance")
                        .HasColumnType("float");

                    b.Property<int>("RatingBlitz")
                        .HasColumnType("int");

                    b.Property<int>("RatingBullet")
                        .HasColumnType("int");

                    b.Property<int>("RatingClassical")
                        .HasColumnType("int");

                    b.Property<int>("RatingRapid")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.HasOne("HSC.Dal.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HSC.Dal.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HSC.Dal.Entities.Match", b =>
                {
                    b.HasOne("HSC.Dal.Entities.Tournament", "Tournament")
                        .WithMany("Matches")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Match_Tournament");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("HSC.Dal.Entities.TournamentMessage", b =>
                {
                    b.HasOne("HSC.Dal.Entities.Tournament", "Tournament")
                        .WithMany("Messages")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_TournamentMessage_Tournament");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("HSC.Dal.Entities.TournamentPlayer", b =>
                {
                    b.HasOne("HSC.Dal.Entities.Tournament", "Tournament")
                        .WithMany("Players")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_TournamentPlayer_Tournament");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("HSC.Dal.Entities.User", b =>
                {
                    b.HasOne("HSC.Dal.Entities.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("HSC.Dal.Entities.Tournament", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Messages");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("HSC.Dal.Entities.User", b =>
                {
                    b.Navigation("Friends");
                });
#pragma warning restore 612, 618
        }
    }
}
