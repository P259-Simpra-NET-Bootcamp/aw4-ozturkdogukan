using Dapper;
using SimApi.Base;
using SimApi.Data.Context;
using System.Data;
using System.Reflection;

namespace SimApi.Data.Repository;

public class DapperRepository<Entity> : IDapperRepository<Entity> where Entity : BaseModel
{
    protected readonly SimDapperDbContext dbContext;
    private bool disposed;

    public DapperRepository(SimDapperDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void DeleteById(int id)
    {
        string tableName = GetTableName();
        string sql = $"DELETE FROM {tableName} WHERE Id = @Id";
        using IDbConnection connection = dbContext.CreateConnection();
        connection.Execute(sql, new { Id = id });
    }

    public List<Entity> Filter(string sql)
    {
        using IDbConnection connection = dbContext.CreateConnection();
        return connection.Query<Entity>(sql).ToList();
    }

    public List<Entity> GetAll()
    {
        string tableName = GetTableName();
        string sql = $"SELECT * FROM {tableName}";
        using IDbConnection connection = dbContext.CreateConnection();
        return connection.Query<Entity>(sql).ToList();
    }

    public Entity GetById(int id)
    {
        string tableName = GetTableName();
        string sql = $"SELECT * FROM {tableName} WHERE Id = @Id";
        using IDbConnection connection = dbContext.CreateConnection();
        return connection.QuerySingleOrDefault<Entity>(sql, new { Id = id });
    }

    public void Insert(Entity entity)
    {
        string tableName = GetTableName();
        string columns = GetColumnNames(entity);
        string values = GetColumnValues(entity);
        string sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

        using (IDbConnection connection = dbContext.CreateConnection())
        {
            var parameters = CreateParameters(entity);

            connection.Execute(sql, parameters);
        }
    }

    public void Update(Entity entity)
    {
        string tableName = GetTableName();
        string columns = GetColumnNames(entity);
        string updateValues = GetUpdateValues(entity);
        string sql = $"UPDATE {tableName} SET {updateValues} WHERE Id = @Id";
        using IDbConnection connection = dbContext.CreateConnection();
        connection.Execute(sql, entity);
    }

    private string GetTableName()
    {
        Type entityType = typeof(Entity);
        return entityType.Name;
    }
    // Kolon isimlerini getirir.
    private string GetColumnNames(Entity entity)
    {
        PropertyInfo[] properties = typeof(Entity).GetProperties();
        string columnNames = string.Join(", ", properties.Where(p => p.GetValue(entity) != null && p.Name != "Id").Select(p => $"[{p.Name}]"));
        return columnNames;
    }

    // Kolonun değerlerini getirir.
    private string GetColumnValues(Entity entity)
    {
        PropertyInfo[] properties = typeof(Entity).GetProperties();

        // Null olmayan ve Id sütununu filtrele
        var propertyValues = properties
            .Where(p => p.Name != "Id")
            .Select(p =>
            {
                var value = p.GetValue(entity);
                if (value != null)
                {
                    return $"'{value}'";
                }
                return null;
            }).Where(p => p != null);

        return string.Join(", ", propertyValues);
    }

    private string GetUpdateValues(Entity entity)
    {
        Type entityType = typeof(Entity);
        var propertyNames = entityType.GetProperties()
            .Where(p => p.Name != "Id" && p.GetValue(entity) != null)
            .Select(p => $"[{p.Name}] = '{p.GetValue(entity)}'");
        return string.Join(", ", propertyNames);
    }
    // SQL Injection için önlem.
    private DynamicParameters CreateParameters(Entity entity)
    {
        var parameters = new DynamicParameters();

        // Entity sınıfındaki özellikleri dolaşarak parametreleri ekleyin
        PropertyInfo[] properties = typeof(Entity).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            string propertyName = property.Name;
            object propertyValue = property.GetValue(entity);
            parameters.Add(propertyName, propertyValue);
        }

        return parameters;
    }
}