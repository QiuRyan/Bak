# Overview
[![Build status](http://tfs.ad.jinyinmao.com.cn:8080/tfs/Jinyinmao/_apis/public/build/definitions/31b07fe6-2fea-450f-9b3f-8100f7a2f51c/2/badge)](http://tfs.ad.jinyinmao.com.cn:8080/tfs/Jinyinmao/_apis/public/build/definitions/31b07fe6-2fea-450f-9b3f-8100f7a2f51c/2/badge)

# Version
1.1.12

#Package
https://dev.blob.core.chinacloudapi.cn/releases/Jinyinmao.AuthManager/Jinyinmao.AuthManager@1.1.12.zip
# DeployNote(1.1)
0.	更新Api项目包


# DeployNote(1.0)
0. 建立数据库Database，命名为 jym-auth，并且创建读写权限用户
1. 执行 SqlScripts/jym-auth.sql 脚本创建数据库表
2. 执行数据迁移程序 DataTransfer/DataTransfer.exe, 需要在配置文件中配置相应环境的配置值（暂跳过，迁移程序未开发完成）
3. Government 服务配置远程配置和权限
4. 在 ServiceBus 服务 sb://jym-{env}.servicebus.chinacloudapi.cn/ 按最佳配置上创建 Topic: jym-user-changed-login-cellphone, jym-user-registered, jym-user-reset-login-password, jym-user-set-login-password 及其默认订阅
5. 修改配置文件 Jinyinmao.AuthManager.Api/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的AppKeys、Env为对应环境（Dev,Test,Product）的配置值
6. 修改配置文件 Jinyinmao.AuthManager.Api/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的网络配置
7. 调整 Jinyinmao.AuthManager.Api 实例数量
8. 创建或者确认 CloudService jym-{env}-authmanager 已经被创建，并且DNS解析为 jym-{env}-authmanager.jinyinmao.com.cn
9. 上传证书 CN=*.jinyinmao.com.cn ABB33B0AAE59296BDECF7DEC1D0BD491EBEBF894
10. 上传证书 remote.jinyinmao.com.cn.pfx EAEBC85D3723BAF21E6E7926F150C0CE39D299C7
11. 修改配置文件 Jinyinmao.AuthManager.Api/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的 JinyinmaoSSL thumbprint 为对应环境（Dev,Test,Product）使用的证书的值
12. 确认 Diagnostics 插件已经打开，并且存储已经配置到 jymstore{env}
13. 发布 CloudService Jinyinmao.AuthManager.Api/DevTest(Product)/Jinyinmao.AuthManager.Api.CloudService.cspkg
14. 修改配置文件 Jinyinmao.AuthManager.Silo/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的AppKeys、Env为对应环境（Dev,Test,Product）的配置值
15. 修改配置文件 Jinyinmao.AuthManager.Silo/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的网络配置
16. 确认 Diagnostics 插件已经打开，并且存储已经配置到 jymstore{env}
17. 发布 CloudService Jinyinmao.AuthManager.Silo/DevTest(Product)/Jinyinmao.ValidateCode.Api.CloudService.cspkg

# ReleaseNote
- 1.1.12
    - 功能: 数据存储恢复到SQL Server

- 1.1.11
    - 功能: 重构与数据存储改为DocumentDb

- 1.1.10
    - 功能: 修改手机号码和注销用户修改内存

- 1.1.9
    - 功能: 优化Auth注册功能

- 1.1.8 
    - 功能: 修改手机号码功能

- 1.1.7 
    - 功能: 修改手机号码功能

- 1.1.5 
    - 功能: 快速注册增加队列功能

- 1.1.5 
    - 功能: 队列功能bug修复

- 1.1.4 
    - 功能: 注册添加队列功能

- 1.1
	- 修复：修改支付密码输入特殊字符报错的问题
    - 优化：优化注册逻辑
    - 优化：优化邀请码生成逻辑，防止更改注册手机号造成验证码重复
    - 功能：添加api/User/Auth/GetSignUpUserId接口，交易系统调用此接口判断用户是否注册
    - 修复：修复登录错误次数多于10次用户账户不存在.
- 1.0
    - 功能：使用验证码和手机号码快速登录
    - 优化：Azure.Storage 升级到6.0.0, Orleans 升级到 1.0.10
    - 功能：通过手机验证码注册和登录
    - 优化：重构工程结构
    - 优化：增加老版本快速登陆和快速注册接口的兼容
    - 修复：修复检查用户密码的时候使用GET方式传入参数没有URLENCODED的问题