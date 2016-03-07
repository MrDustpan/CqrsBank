﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Respawn;

namespace Cqrs.Tests.Integration
{
  [TestFixture]
  public abstract class DatabaseTest
  {
    protected const string CONNECTION_STRING_NAME = "IntegrationTestingDb";

    private static readonly string[] CreateScripts = { "up.sql" };
    private static readonly string[] DeleteScripts = { "down.sql" };
    private static readonly string ScriptsDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\database\");
    private static readonly Func<string, bool> StartsBatch = s => s.StartsWith("GO", StringComparison.OrdinalIgnoreCase);
    private static readonly Func<string, bool> IsUse = s => s.StartsWith("USE", StringComparison.OrdinalIgnoreCase);
    private static readonly Func<string, bool> IsSet = s => s.StartsWith("SET", StringComparison.OrdinalIgnoreCase);
    private static readonly Func<string, bool> IsComment = s => s.StartsWith("/*") && s.EndsWith("*/");
    private static readonly Checkpoint Checkpoint = new Checkpoint();

    [OneTimeSetUp]
    public void SetUpDatabase()
    {
      var createScripts = CreateScripts.Select(s => Path.Combine(ScriptsDirectory, s)).ToList();

      Assert.IsTrue(createScripts.All(File.Exists));

      ExecuteScripts(createScripts);
    }

    [OneTimeTearDown]
    public void TearDownDatabase()
    {
      var deleteScripts = DeleteScripts.Select(s => Path.Combine(ScriptsDirectory, s)).ToList();

      Assert.IsTrue(deleteScripts.All(File.Exists));

      ExecuteScripts(deleteScripts);
    }

    [TearDown]
    public void TearDown()
    {
      ResetDb();
    }

    protected void ResetDb()
    {
      var cn = GetConnection();
      cn.Open();
      Checkpoint.Reset(cn);
    }

    private static void ExecuteScripts(IEnumerable<string> scripts)
    {
      Func<string, bool> isValidCommand = c => !IsUse(c) && !IsSet(c) && !IsComment(c);
      using (var conn = GetConnection())
      {
        conn.Open();
        foreach (var script in scripts)
        {
          var commands = File.ReadAllLines(script).Where(isValidCommand).ToList();
          IEnumerable<IEnumerable<string>> batches = CreateBatches(commands).ToList();
          foreach (var batch in batches)
          {
            using (var query = conn.CreateCommand())
            {
              query.CommandText = string.Join(Environment.NewLine, batch);
              query.ExecuteNonQuery();
            }
          }
        }
      }
    }

    /// <summary>
    /// In order to execute groups of commands that can be executed together (not all can),
    /// we separate them by the GO command into distinct, executable sets.  This assumes
    /// that the scripts have been generated by Tasks->Generate Scripts in SSMS but will
    /// also work if each script only contains commands that can be grouped together.
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    private static IEnumerable<IEnumerable<string>> CreateBatches(IEnumerable<string> commands)
    {
      var batch = new List<string>();
      foreach (var command in commands)
      {
        if (StartsBatch(command))
        {
          if (batch.Any())
          {
            yield return batch;
            batch = new List<string>();
          }
        }
        else if (!string.IsNullOrWhiteSpace(command))
        {
          batch.Add(command);
        }
      }

      if (batch.Any())
      {
        yield return batch;
      }
    }

    protected static DbConnection GetConnection()
    {
      var cs = ConfigurationManager.ConnectionStrings["IntegrationTestingDb"].ConnectionString;
      return new SqlConnection(cs);
    }
  }
}