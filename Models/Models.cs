using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductsAndCategories.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Product> Products {get;set;}
        public DbSet<Category> Categories {get;set;}
        public DbSet<Association> Associations {get;set;}
    }

    public class Product
    {
        [Key]
        public int ProductId {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public double Price {get;set;}
        public List<Association> AssocCats {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }

    public class Category
    {
        [Key]
        public int CategoryId {get;set;}
        public string Name {get;set;}
        public List<Association> AssocProds {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }

    public class Association
    {
        [Key]
        public int AssociationId {get;set;}
        public int ProductId {get;set;}
        public int CategoryId {get;set;}
        public Product Product {get;set;}
        public Category Category {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}