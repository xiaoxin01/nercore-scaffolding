# 为 Console 程式添加外部配置文件

1. 添加依赖包

    dotnet add package Microsoft.Extensions.Configuration.Json

2. 添加读取配置文件代码

```c#
var environmentName = null == Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ? "Development" : Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

Console.WriteLine($"Environment: {environmentName}");

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environmentName}.json", true, true)
    .Build();
```

3. 添加配置文件

appsettings.[Environment].json：

```json
{
    "TheKey": "Hello word!"
}
```

4. 添加配置文件到项目输出

编辑工程文件(.csproj)，添加：

```xml  
  <ItemGroup>
    <None Include="appsettings*.json" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>
```