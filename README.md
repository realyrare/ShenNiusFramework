# ShenNius.Framework

#### 介绍


 **一款基于Asp.net core3.1的模块化开发框架,提取了ABP VNext的模块核心，适合中小型项目快速开发，取名为神牛(ShenNius),意为大神大牛多提意见，一起参与，神牛云集，为dotnet社区贡献一份自己的力量。** 

 所有的业务代码可以按照类库划分，使用宿主托管。所有的代码坚持" **Don't repeat yourself** "。坚决反对" **简单的事情复杂干** "，" **大量代码在应用层过度层层封装** "，反对" **过度设计** "的原则下进行开发。

 如果你有这个习惯，并且也想找一款迷你型、上手快，不用学习太多东西的框架，那么它就适合你，只要你会dotnet core,几乎没有学习成本，除了ABP VNext的模块化核心代码。


#### 软件架构
1. API框架：dotNetCore 
2. ORM：SqlSugarCore 
3. 缓存：MemoryCache（后期会上redis，看项目业务需求） 
4. 日志管理：Nlog
5. 工具类：Aes加密、Md5加密、RSA加密、Des加密  
6. token:Json Web Token
7. 实体验证：FluentValidation
8. 实体映射：AutoMapper
9. 数据库使用mysql（后面会做sqlserver等其他数据库的兼容）
10.支持跨平台部署 linux/windows

#### 安装教程

1.  git clone  xxxx
2.  在mysql上创建数据库shenniusdb，然后执行源码db文件下的sql语句，以最近日期为准。
3.  更改appsettings.json中的数据库链接字符串
4.  ShenNius.API.Hosting 可以配置你要启动的API项目
5.  如果要启动后台管理，请多项启动ShenNius.API.Hosting  和ShenNius.Layui.Admin。如果只是想启动api swagger ,启动ShenNius.API.Hosting即可。

#### 使用说明

1.   ShenNius.ModuleCore是模块化的核心代码
2.   ShenNius.Swagger是封装的API Doc
3.   ShenNius.Share.Infrastructure 基础设施、里面包含常用的扩展方法、静态类
4.   ShenNius.Share.Models 实体层、里面包括Dto验证、配置类
5.   ShenNius.Share.Service 服务层、业务逻辑基本都在这个里面、里面包含了数据访问操作。
6.   ShenNius.Sys.API  基于RBAC的权限API。
7.   ShenNius.Cms.API  基于Cms的API （待完成）
8.   ShenNius.Shop.API  基于Cms的API （待完成）
9.   ShenNius.API.Hosting API的宿主，所有API目前都放在这个里面托管，这也是魅力所在，可以Sys和Cms API各自托管使用独立的宿主，主要看业务量。拆合很容易。
10.  ShenNius.Layui.Admin 基于layui的后台管理。
#### 参与贡献


1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request


#### 效果图
![输入图片说明](https://images.gitee.com/uploads/images/2021/0304/164851_824fb005_1173871.png "1.PNG")
![输入图片说明](https://images.gitee.com/uploads/images/2021/0304/164910_4917a1c1_1173871.png "2.PNG")
![输入图片说明](https://images.gitee.com/uploads/images/2021/0304/164921_d6dff912_1173871.png "3.PNG")
![输入图片说明](https://images.gitee.com/uploads/images/2021/0304/164934_1b63bf8f_1173871.png "4.PNG")
![输入图片说明](https://images.gitee.com/uploads/images/2021/0304/164943_44d2dacd_1173871.png "5.PNG")
![输入图片说明](https://images.gitee.com/uploads/images/2021/0304/165019_be2d1343_1173871.png "api-1.PNG")
![输入图片说明](https://images.gitee.com/uploads/images/2021/0304/165031_41b05a14_1173871.png "api-2.PNG")

