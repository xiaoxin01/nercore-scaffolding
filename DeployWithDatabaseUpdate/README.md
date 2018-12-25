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

# 通过k8s部署站点

当部署环境为k8s时，需要考虑如下问题：

* 私有docker镜像仓库 --> 用于构建，拉取镜像
* 不同环境部署 --> 用于区分测试、正式环境

最终需要的文件包含：k8s目录下的部署文件，deploy.sh文件，流程如下：

1. 读取命令行，判断部署环境
2. 构建镜像
3. 将镜像推送到私有docker镜像仓库
4. 部署项目配置文件
5. 部署项目ingress
6. 更新项目数据库
7. 检查数据库更新状况
8. 部署项目

其中，将ingress和configmap文件从k8s.yaml中抽离出来，主要是为了区分不同环境，目前k8s部署yaml文件还没有办法来动态配置参数。
