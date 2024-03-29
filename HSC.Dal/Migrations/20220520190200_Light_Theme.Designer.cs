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
    [Migration("20220520190200_Light_Theme")]
    partial class Light_Theme
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

                    b.Property<string>("UsersUserName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GroupsId", "UsersUserName");

                    b.HasIndex("UsersUserName");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("HSC.Dal.Entities.Block", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("BlockedUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BlockerUsername")
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

                    b.Property<int>("Increment")
                        .HasColumnType("int");

                    b.Property<decimal>("MaximumBet")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MinimumBet")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Offerer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Receiver")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimeLimitMinutes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Challenge", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.ChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsSeen")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TimeStamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("HSC.Dal.Entities.Friend", b =>
                {
                    b.Property<string>("UserName1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName2")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserName1", "UserName2");

                    b.HasIndex("UserName2");

                    b.ToTable("Friend");
                });

            modelBuilder.Entity("HSC.Dal.Entities.FriendRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ReceiverUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequesterUsername")
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Group", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Increment")
                        .HasColumnType("int");

                    b.Property<decimal>("MaximumBet")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MinimumBet")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Moves")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("TimeLimitMinutes")
                        .HasColumnType("int");

                    b.Property<int?>("TournamentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Match", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.MatchPlayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Color")
                        .HasColumnType("int");

                    b.Property<decimal>("CurrentBet")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsBetting")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWinner")
                        .HasColumnType("bit");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("MatchPlayer", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.SearchingPlayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Increment")
                        .HasColumnType("int");

                    b.Property<decimal>("MaximumBet")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MinimumBet")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("TimeLimitMinutes")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SearchingPlayer", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.Tournament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("BuyIn")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GameIncrement")
                        .HasColumnType("int");

                    b.Property<int>("GameTimeMinutes")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Length")
                        .HasColumnType("time");

                    b.Property<decimal>("PrizePool")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("WinnerUserName")
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderUserName")
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

                    b.Property<decimal?>("Points")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("TournamentId", "UserName");

                    b.ToTable("TournamentPlayer", (string)null);
                });

            modelBuilder.Entity("HSC.Dal.Entities.User", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsUsingPlayMoney")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastPlayMoneyRedeemDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("LightTheme")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PlayMoneyBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RatingBlitz")
                        .HasColumnType("int");

                    b.Property<int>("RatingBullet")
                        .HasColumnType("int");

                    b.Property<int>("RatingClassical")
                        .HasColumnType("int");

                    b.Property<int>("RatingRapid")
                        .HasColumnType("int");

                    b.HasKey("UserName");

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
                        .HasForeignKey("UsersUserName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HSC.Dal.Entities.Friend", b =>
                {
                    b.HasOne("HSC.Dal.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserName1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HSC.Dal.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserName2")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HSC.Dal.Entities.Match", b =>
                {
                    b.HasOne("HSC.Dal.Entities.Tournament", "Tournament")
                        .WithMany("Matches")
                        .HasForeignKey("TournamentId")
                        .HasConstraintName("FK_Match_Tournament");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("HSC.Dal.Entities.MatchPlayer", b =>
                {
                    b.HasOne("HSC.Dal.Entities.Match", "Match")
                        .WithMany("MatchPlayers")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_MatchPlayer_Match");

                    b.Navigation("Match");
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

            modelBuilder.Entity("HSC.Dal.Entities.Match", b =>
                {
                    b.Navigation("MatchPlayers");
                });

            modelBuilder.Entity("HSC.Dal.Entities.Tournament", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Messages");

                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
