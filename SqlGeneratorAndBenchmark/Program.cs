using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SqlGeneratorAndBenchmark
{
    class Program
    {

        static void Main(string[] args)
        {
            //SeedDb();

            BenchmarkRunner.Run<NormalizationBenchmark>();
        }


        static void SeedDb()
        {
            using var context = new BenchmarkContext();
            for (var i = 0; i < 10000; i++)
            {
                var p = HelperFunctions.GenerateRandomString();
                var c1 = HelperFunctions.GenerateRandomString();
                var c2 = HelperFunctions.GenerateRandomString();
                var c3 = HelperFunctions.GenerateRandomString();

                context.DeNormalizedObjs.Add(new()
                {
                    ParentObjName = p,
                    ChildObjName1 = c1,
                    ChildObjName2 = c2,
                    ChildObjName3 = c3
                });

                context.ParentObjs.Add(new()
                {
                    ParentObjName = p,
                    ChildObjs = new()
                    {
                        new() { ChildObjName = c1 },
                        new() { ChildObjName = c2 },
                        new() { ChildObjName = c3 }
                    }
                });
            }

            context.SaveChanges();
        }
    }

    [SimpleJob(RuntimeMoniker.Net60)]
    [RPlotExporter]
    public class NormalizationBenchmark
    {
        private string p, c1, c2, c3;
        private BenchmarkContext context;

        [Params(1, 10/*, 100*/)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            p = HelperFunctions.GenerateRandomString();
            c1 = HelperFunctions.GenerateRandomString();
            c2 = HelperFunctions.GenerateRandomString();
            c3 = HelperFunctions.GenerateRandomString();
            context = new BenchmarkContext();
        }

        #region Insert
        [Benchmark]
        public void DeNormalizedInsert()
        {
            context.DeNormalizedObjs.Add(new()
            {
                ParentObjName = p,
                ChildObjName1 = c1,
                ChildObjName2 = c2,
                ChildObjName3 = c3
            });
            context.SaveChanges();
        }

        [Benchmark]
        public void NormalizedInsert()
        {
            context.ParentObjs.Add(new()
            {
                ParentObjName = p,
                ChildObjs = new()
                {
                    new() {ChildObjName = c1},
                    new() {ChildObjName = c2},
                    new() {ChildObjName = c3}
                }
            });
            context.SaveChanges();
        }

        #endregion

        #region Scan
        [Benchmark]
        public void DeNormalizedScan()
        {
            context.DeNormalizedObjs.FirstOrDefault(c =>
                c.ParentObjName.StartsWith(p[0]));
            context.SaveChanges();
        }

        [Benchmark]
        public void NormalizedScan()
        {
            context.ParentObjs.Include(p => p.ChildObjs).FirstOrDefault(c =>
                c.ParentObjName.StartsWith(p[0]));
            context.SaveChanges();
        }

        #endregion

        #region Index on parent prop
        [Benchmark]
        public void DeNormalizedIndexOnParentProp()
        {
            context.DeNormalizedObjs.Find(1);
            context.SaveChanges();
        }

        [Benchmark]
        public void NormalizedIndexOnParentProp()
        {
            context.ParentObjs.Include(p => p.ChildObjs)
                .FirstOrDefault(c => c.Id == 1);
            context.SaveChanges();
        }

        #endregion

        #region Index on child prop
        [Benchmark]
        public void DeNormalizedIndexOnChildProp()
        {
            context.DeNormalizedObjs.FirstOrDefault(c => c.ChildObjName1 == c1);
            context.SaveChanges();
        }

        [Benchmark]
        public void NormalizedIndexOnChildProp()
        {
            context.ParentObjs.Include(p => p.ChildObjs.Where(p => p.ChildObjName == c1)).ToList();
            context.SaveChanges();
        }

        #endregion

    }

    public static class HelperFunctions
    {
        const string CHAR_SET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static readonly Random RND = new((int)DateTime.Now.Ticks);

        public static string GenerateRandomString(int len = 8) => 
            new(Enumerable.Range(0, len).Select(c => CHAR_SET[RND.Next(CHAR_SET.Length - 1)]).ToArray());
    }

    public class BenchmarkContext : DbContext
    {
        public DbSet<DeNormalizedObj> DeNormalizedObjs { get; set; }
        public DbSet<ParentObj> ParentObjs { get; set; }
        public DbSet<ChildObj> ChildObjs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=BenchmarkDB;Trusted_Connection=True;MultipleActiveResultSets=true");
    }

    public class DeNormalizedObj
    {
        public int DeNormalizedObjId { get; set; }
        public string ParentObjName { get; set; }
        public string ChildObjName1 { get; set; }
        public string ChildObjName2 { get; set; }
        public string ChildObjName3 { get; set; }

    }

    public class ParentObj
    {
        public int Id { get; set; }
        public string ParentObjName { get; set; }

        public List<ChildObj> ChildObjs { get; set; }
    }

    public class ChildObj
    {
        public int Id { get; set; }
        public int ParentObjId { get; set; }
        public string ChildObjName { get; set; }
        public virtual ParentObj ParentObj { get; set; }
    }
}
