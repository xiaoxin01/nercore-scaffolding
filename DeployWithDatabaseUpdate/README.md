# 部署站点时更新数据库

本方法采用环境变量标识的方法更新数据库

## 建立项目

```bash
    dotnet new mvc -o DeployWithDatabaseUpdate --auth Individual
    dotnet restore DeployWithDatabaseUpdate
```

## 修改代码，监控环境变量

修改Startup.cs，当环境变量中包含UPDATE_DB=TRUE时，进行更新数据库的动作：

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime,
    ApplicationDbContext dbContext)
{
    if (System.Environment.GetEnvironmentVariable("UPDATE_DB") == "TRUE")
    {
        Console.WriteLine($"Update database for env: {env.EnvironmentName}");
        dbContext.Database.Migrate();
        appLifetime.StopApplication();
    }
}
```

## 通过docker更新数据库

进入工程目录，运行如下命令：

```bash
docker build -f DeployWithDbUpdate/Dockerfile . -t dotnetupdatedb
docker run -e UPDATE_DB=TRUE dotnetupdatedb
```

## 通过docker-compose更新数据库

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml -f docker-compose.updatedb.yml up
```
