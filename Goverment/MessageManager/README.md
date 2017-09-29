# Overview

#Buildstatus
[![Build status](http://tfs.ad.jinyinmao.com.cn:8080/tfs/Jinyinmao/_apis/public/build/definitions/31b07fe6-2fea-450f-9b3f-8100f7a2f51c/8/badge)](http://tfs.ad.jinyinmao.com.cn:8080/tfs/Jinyinmao/_apis/public/build/definitions/31b07fe6-2fea-450f-9b3f-8100f7a2f51c/8/badge)

# Version
1.0.4

#Package
https://dev.blob.core.chinacloudapi.cn/releases/Jinyinmao.MessageManager/Jinyinmao.MessageManager@1.0.4.zip

# DeployNote(1.0)
0. 建立数据库Database，命名为 jym-message，并且创建读写权限用户
1. 执行 SqlScripts/jym-message.sql 脚本创建数据库表
2. 修改配置文件 MessageManager.Api/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的AppKeys、Env为生产版本的配置值
3. 修改配置文件 MessageManager.Api/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的网络配置
4. 调整 MessageManager.Api 实例数量
5. 创建或者确认 CloudService jym-{env}-messagemanager 已经被创建，并且DNS解析为 jym-{env}-messagemanager.jinyinmao.com.cn
6. 上传证书 CN=*.jinyinmao.com.cn ABB33B0AAE59296BDECF7DEC1D0BD491EBEBF894
7. 上传证书 remote.jinyinmao.com.cn.pfx EAEBC85D3723BAF21E6E7926F150C0CE39D299C7
8. 修改配置文件 MessageManager.Api/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的 JinyinmaoSSL thumbprint 为生产证书的值
9. 确认 Diagnostics 插件已经打开，并且存储已经配置到 jymstore{env}
10. 发布 CloudService MessageManager.Api/DevTest(Product)/Jinyinmao.MessageManager.Api.CloudService.cspkg
11. 修改配置文件 ValidateCode.Api/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的AppKeys、Env为生产版本的配置值
12. 修改配置文件 ValidateCode.Api/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的网络配置
13. 调整 ValidateCode.Api 实例数量
14. 创建或者确认 CloudService jym-{env}-validatecode 已经被创建，并且DNS解析为 jym-{env}-validatecode.jinyinmao.com.cn
15. 上传证书 CN=*.jinyinmao.com.cn ABB33B0AAE59296BDECF7DEC1D0BD491EBEBF894
16. 上传证书 remote.jinyinmao.com.cn.pfx EAEBC85D3723BAF21E6E7926F150C0CE39D299C7
17. 修改配置文件 ValidateCode.Api/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的 JinyinmaoSSL thumbprint 为生产证书的值
18. 确认 Diagnostics 插件已经打开，并且存储已经配置到 jymstore{env}
19. 发布 CloudService ValidateCode.Api/DevTest(Product)/Jinyinmao.ValidateCode.Api.CloudService.cspkg
20. 修改配置文件 MessageWork/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的AppKeys、Env为生产版本的配置值
21. 修改配置文件 MessageWork/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的网络配置
22. 调整 MessageWork 实例数量
23. 上传证书 remote.jinyinmao.com.cn.pfx EAEBC85D3723BAF21E6E7926F150C0CE39D299C7
24. 确认 Diagnostics 插件已经打开，并且存储已经配置到 jymstore{env}
25. 发布 CloudService MessageWork/DevTest(Product)/Jinyinmao.MessageWork.CloudService.cspkg

# ReleaseNote
- 1.0
    - 功能：发布初始版本