# Overview

#Buildstatus
[![Build status](http://tfs.ad.jinyinmao.com.cn:8080/tfs/Jinyinmao/_apis/public/build/definitions/31b07fe6-2fea-450f-9b3f-8100f7a2f51c/7/badge)](http://tfs.ad.jinyinmao.com.cn:8080/tfs/Jinyinmao/_apis/public/build/definitions/31b07fe6-2fea-450f-9b3f-8100f7a2f51c/7/badge)

# Version
1.1.5

#Package
https://dev.blob.core.chinacloudapi.cn/releases/Jinyinmao.Government/Jinyinmao.Government@1.1.5.zip

# DeployNote(1.1)
0. 建立数据库Database，命名为 jym-database，并且创建读写权限用户
1. 执行 SqlScripts/jym-government.sql 脚本创建数据库表
2. 修改配置文件 Government/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的AppKeys、Env为生产版本的配置值
3. 修改配置文件 Government/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的jym-government-connection-string为生产数据库的值
4. 修改配置文件 Government/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的网络配置
5. 调整实例数量
6. 创建或者确认 CloudService jym-{env}-government 已经被创建，并且DNS解析为 jym-{env}-government.jinyinmao.com.cn
7. 上传证书 CN=*.jinyinmao.com.cn ABB33B0AAE59296BDECF7DEC1D0BD491EBEBF894
8. 上传证书 remote.jinyinmao.com.cn.pfx EAEBC85D3723BAF21E6E7926F150C0CE39D299C7
9. 修改配置文件 Government/DevTest(Product)/ServiceConfiguration.Cloud.cscfg 中的 JinyinmaoSSL thumbprint 为生产证书的值
10. 确认 Diagnostics 插件已经打开，并且存储已经配置到 jymstore{env}
11. 发布 CloudService Government/DevTest(Product)/Jinyinmao.Government.CloudService.cspkg

# ReleaseNote
- 1.1
    - 修复：修改了部分BUG

- 1.0
    - 功能：发布初始版本