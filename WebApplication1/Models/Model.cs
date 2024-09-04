﻿using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public class DemoContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Person> People { get; set; }
    //public DbSet<Actor> Actors { get; set; }

    public DemoContext(DbContextOptions<DemoContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>()
            .HasKey(m => m.Id);;

        modelBuilder.Entity<Person>()
            .HasKey(m => m.Id);

        //modelBuilder.Entity<Actor>()
        //    .HasKey(m => new { m.MovieId, m.PersonId });

        //modelBuilder.Entity<Actor>()
        //    .HasKey(m => new { m.MovieId, m.PersonId });
    }
}

public class Movie
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Genre Genre { get; set; }
    public DateTime Released { get; set; }
    //public List<Actor> Actors { get; set; }
    //public List<Writer> Writers { get; set; }
    public Person Director { get; set; }
    public int? DirectorId { get; set; }
    public double Rating { get; internal set; }
}

//public class Actor
//{
//    public uint PersonId { get; set; }
//    public Person Person { get; set; }
//    public uint MovieId { get; set; }
//    public Movie Movie { get; set; }
//}
//public class Writer
//{
//    public uint PersonId { get; set; }
//    public Person Person { get; set; }
//    public uint MovieId { get; set; }
//    public Movie Movie { get; set; }
//}

public enum Genre
{
    Action,
    Drama,
    Comedy,
    Horror,
    Scifi,
}

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Dob { get; set; }
    //public List<Actor> ActorIn { get; set; }
    //public List<Writer> WriterOf { get; set; }
    //public List<Movie> DirectorOf { get; set; }
    public DateTime? Died { get; set; }
    public bool IsDeleted { get; set; }
}
