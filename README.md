#### QQ群
<a target="_blank" href="https://qm.qq.com/cgi-bin/qm/qr?k=IlNhUh4OZ4IS0fjt2O6b8HtjKuxiNY3I&jump_from=webapi"><img border="0" src="//pub.idqqimg.com/wpa/images/group.png" alt="dotnet根据地" title="dotnet根据地">878303823</a>
# ShenNius.Framework

#### 介绍

 **基于Asp.NetCore的开发框架,目前已开发完权限管理模块，CMS模块、商城模块。特别适合中小型项目快速开发，对CRUD API接口抽象到上层controller,只需要建立对应的实体，即可生成某张表的增删改查接口。** 
 
 所有的代码坚持" **Don't repeat yourself** "。坚决反对" **简单的事情复杂干** "，" **大量代码在应用层过度层层封装** "，反对" **过度设计** "的原则下进行开发。

 如果你有这个习惯，并且也想找一款迷你型、上手快，不用学习太多东西的框架，那么它就适合你，只要你会dotnet core,几乎没有学习成本。

#### 特别申明
 **  master分支为asp.netcore3.1单体分支:git clone -b master https://gitee.com/shenniu_code_group/shen-nius.-modularity.git  ** 

 ** .net6.0分支为asp.netcore6.0单体分支:git clone -b .net6.0 https://gitee.com/shenniu_code_group/shen-nius.-modularity.git  **

 ** page5.0分支为asp.netcore5.0单体前后端分离分支:git clone -b page5.0 https://gitee.com/shenniu_code_group/shen-nius.-modularity.git   。 注：该分支不再维护，适合大家学习二开使用。**  

 ** 特别抱歉，由于单体分支和模块化的分支放在一起导致项目的分支过多且比较分散杂乱，因此借着asp.netcore6.0来临之际，准备把项目的模块化在高度重构下，尽可能的模块之间降低耦合度，因此会单独重新建立一个仓库，大家敬请期待** 


 ** 此外该项目新功能的迭代我都会放在模块化分支上面，单体新功能的迭代一般都会在最新的asp.netcore版本上开发。老版本新功能有偿服务！** 

#### 软件架构
![输入图片说明](https://images.gitee.com/uploads/images/2021/1109/172453_89cc7f93_1173871.jpeg "项目架构图.jpg")
 整体框架分为服务层、基础设施层，实体层，展现层。

 很多人都使用过三层架构，该架构是从三层架构上面简化而来，去掉了以前三层架构中的数据访问层，配合顺手的ORM，不管开发API还是mvc效率和性能都是杠杠的。

 以后整体架构会向DDD发展，逐步缓慢的演进，为解决业务需求会把实体层的贫血模型更改为充血模型，这是当下的任务。


#### 使用技术

1. API框架：dotNetCore 
2. ORM：SqlSugarCore 
3. 缓存：MemoryCache和redis一键任意切换
4. 日志管理：Nlog
5. 工具类：Aes加密、Md5加密、RSA加密、Des加密  
6. token:Json Web Token
7. 实体验证：FluentValidation
8. 实体映射：AutoMapper
9. 数据库使用mysql（后面会做sqlserver等其他数据库的兼容）
10. 支持跨平台部署 linux/windows
11. 支持AOP缓存，使用AspectCore,缓存可做到Memarycache和redis一件切换
12. 支持AOP事务，服务层和控制器都可以打上特性标签使用
13. 对多租户使用Filter，不管是添加还是更新、查询即可自动赋值
14. 支持七牛云和本地图片一键切换使用上传
15. 对于单表的增删改查，在控制器内做了封装，有新的业务按约定建立对应的CRUD实体，一套API自动完成
16. 支持站群管理
17. 支持MediatR进程内通讯解耦(目前已取消使用，因为只支持进程内通讯)


#### 安装教程

1.  git clone -b single https://gitee.com/shenniu_code_group/shen-nius.-modularity.git 
2.  在mysql上创建数据库shenniusdb，然后执行源码doc文件夹dbscript文件下的sql语句，以最近日期为准。doc文件夹里面数据库字典文档，word、CHM、html格式的都有。
3.  ShenNius.Mvc.Admin（前后端不分离）和ShenNius.API.Hosting（前后端分离 API）可以配置你要启动的appsettings.jsonwen文件信息。
4、 使用过程中有什么问题欢迎提issues,基本都会第一时间解决。
#### 使用说明

1.   ShenNius.Share.Infrastructure 基础设施、里面包含常用的扩展方法、静态类。
2.   ShenNius.Share.Models 实体层、里面包括Dto验证、配置类。
3.   ShenNius.Share.Domain 服务层、业务逻辑基本都在这个里面、里面包含了数据访问操作。
4.   ShenNius.Admin.API 有对服务层CRUD的抽象处理，模块只需按规则建立对应的CRUD实体，API接口自动生成。
5.   ShenNius.API.Hosting 前后端分离的API。
6.   ShenNius.Mvc.Admin 基于layui的后台管理包含CMS、商城，权限管理。


#### 效果图

  #### 权限管理模块
<table>
    <tr>
        <td><img src="https://images.gitee.com/uploads/images/2021/1001/200307_6fa1bb44_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0925/235614_89800b14_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0304/164851_824fb005_1173871.png"/></td>
    </tr>
    <tr>
        <td><img src="https://images.gitee.com/uploads/images/2021/0925/235614_89800b14_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0925/235614_89800b14_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0925/235838_37691e67_1173871.png"/></td>
    </tr>              
   <tr>
      <td><img src="https://images.gitee.com/uploads/images/2021/0925/235909_4c7185de_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220143_65141036_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220202_2220b39c_1173871.png"/></td>
    </tr> 
   <tr>
     <td><img src="https://images.gitee.com/uploads/images/2021/0927/220246_a7f94f1e_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220307_c9bedea9_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220323_f940ac46_1173871.png"/></td>
    </tr>
  <tr>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220220_98eeadcf_1173871.png"/></td>
       
    </tr>
</table>

  #### CMS系统模块
![输入图片说明](https://images.gitee.com/uploads/images/2021/0927/220844_6f81e7f8_1173871.png "article.png")
![文章编辑](https://images.gitee.com/uploads/images/2021/0927/220609_911f705d_1173871.png "article-editpng.png")
![文章栏目将](https://images.gitee.com/uploads/images/2021/0927/220630_db845562_1173871.png "column.png")
![广告列表](https://images.gitee.com/uploads/images/2021/0927/220643_c0c4885d_1173871.png "adv.png")
![广告添加](https://images.gitee.com/uploads/images/2021/0927/220704_d03f4bc4_1173871.png "adv-add.png")
  #### 商城系统模块
![商品列表](https://images.gitee.com/uploads/images/2021/0927/220934_03c65880_1173871.png "goods.png")
![商品列表2](https://images.gitee.com/uploads/images/2021/0927/220950_1b6df0a5_1173871.png "goods-2.png")
![商品编辑](https://images.gitee.com/uploads/images/2021/0927/221005_423e3c97_1173871.png "goods-edit.png")
![商品分类](https://images.gitee.com/uploads/images/2021/0927/221028_39281d75_1173871.png "category.png")

#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request

