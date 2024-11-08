using Microsoft.Extensions.Configuration;
using Testcontainers.Elasticsearch;
using Testcontainers.MySql;
using Testcontainers.Redis;

namespace TestFramework.Common.Fixtures;

public sealed class DockerFixture
{
    private readonly string _testScope;

    public DockerFixture(string testScope)
    {
        _testScope = testScope;

        MySql ??= BuildMySql();
        Redis ??= BuildRedis();
        Elasticsearch ??= BuildElasticsearch();
    }

    public MySqlContainer MySql { get; }
    public RedisContainer Redis { get; }
    public ElasticsearchContainer Elasticsearch { get; }

    public void Start()
    {
        Task.WaitAll(MySql.StartAsync(), Redis.StartAsync(), Elasticsearch.StartAsync());
    }

    private MySqlContainer BuildMySql()
    {
        return new MySqlBuilder()
            .WithName($"saveapis-mysql-{_testScope}")
            .WithImage("mysql:9.1.0")
            .WithEnvironment("MYSQL_ROOT_PASSWORD", "root")
            .WithPortBinding(3306, true)
            .WithReuse(true)
            .Build();
    }

    private RedisContainer BuildRedis()
    {
        return new RedisBuilder()
            .WithName($"saveapis-redis-{_testScope}")
            .WithImage("redis:8.0-M01-bookworm")
            .WithPortBinding(6379, true)
            .WithReuse(true)
            .Build();
    }

    private ElasticsearchContainer BuildElasticsearch()
    {
        return new ElasticsearchBuilder()
            .WithName($"saveapis-elasticsearch-{_testScope}")
            .WithImage("docker.elastic.co/elasticsearch/elasticsearch:8.16.0")
            .WithEnvironment("discovery.type", "single-node")
            .WithEnvironment("xpack.security.enabled", "false")
            .WithPortBinding(9200, true)
            .WithPortBinding(9300, true)
            .WithReuse(true)
            .Build();
    }

    public IConfiguration LoadConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();

        configuration["ELASTICSEARCH_NAME"] = $"tests-{_testScope}";
        configuration["ELASTICSEARCH_URL"] =
            $"http://{Elasticsearch.Hostname}:{Elasticsearch.GetMappedPublicPort(9200)}";

        configuration["MYSQL_HOST"] = MySql.Hostname;
        configuration["MYSQL_PORT"] = MySql.GetMappedPublicPort(3306).ToString();
        configuration["MYSQL_USER"] = "root";
        configuration["MYSQL_PASSWORD"] = "root";

        configuration["HANGFIRE_REDIS_HOST"] = Redis.Hostname;
        configuration["HANGFIRE_REDIS_PORT"] = Redis.GetMappedPublicPort(6379).ToString();
        configuration["HANGFIRE_REDIS_DATABASE"] = "0";

        configuration["EASYCACHING_REDIS_HOST"] = Redis.Hostname;
        configuration["EASYCACHING_REDIS_PORT"] = Redis.GetMappedPublicPort(6379).ToString();
        configuration["EASYCACHING_REDIS_DATABASE"] = "1";
        configuration["EASYCACHING_REDIS_BUS_DATABASE"] = "2";

        return configuration;
    }
}