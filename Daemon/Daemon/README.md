# Overview

#Buildstatus
[![Build status](http://tfs.ad.jinyinmao.com.cn:8080/tfs/Jinyinmao/_apis/public/build/definitions/31b07fe6-2fea-450f-9b3f-8100f7a2f51c/11/badge)](http://tfs.ad.jinyinmao.com.cn:8080/tfs/Jinyinmao/_apis/public/build/definitions/31b07fe6-2fea-450f-9b3f-8100f7a2f51c/11/badge)

# Version
1.3.14

#Package
https://dev.blob.core.chinacloudapi.cn/releases/Jinyinmao.Daemon/Jinyinmao.Daemon@1.3.14.zip

# DeployNote(1.3.12)
0.	移除余额猫赎回锁单功能

# DeployNote(1.3.11)
0.	添加余额猫赎回锁单功能

# DeployNote(1.3.10)
0.	系统代码清理

# DeployNote(1.3.9)
0.	将银行存管（融资方）流水15分钟后银行未受理至为失败-功能移除。
	将 “Setting”为：“EbibpbCenterConnectionString” 移除。

# DeployNote(1.3.8)
0.	将银行存管（融资方）流水15分钟后银行未受理至为失败  每条先读取银行返回的信息进行修改，如果银行网关接口显示该数据没有受理则制为失败，否则继续等待。

# DeployNote(1.3.5)
0.	增加定时同步新开通存管用户账户的功能

# DeployNote(1.2.35)
0.	将银行存管（融资方）流水15分钟后银行未受理至为失败。

# DeployNote(1.2.34)
0.	新增超级合伙人跑批任务

# DeployNote(1.2.33)
0.	更新daemon项目

# DeployNote(1.2.32)
0.	更新daemon项目

# DeployNote(1.2.31)
0.	更新daemon项目

# DeployNote(1.2.30)
0.	更新daemon项目

# DeployNote(1.2.14)
0.	更新daemon项目

# DeployNote(1.2.13)
0.	更新daemon项目

# DeployNote(1.2.12)
0.	更新daemon项目

# DeployNote(1.2.11)
0.	更新daemon项目

# DeployNote(1.2.9)
0.	更新daemon项目

# DeployNote(1.2.8)
0.	更新daemon项目

# DeployNote(1.2.7)
0.	更新daemon项目

# DeployNote(1.2.6)
0.	更新daemon项目

# DeployNote(1.2.4)
0.	更新daemon项目

# DeployNote(1.2.3)
0.	更新daemon项目

# DeployNote(1.2.2)
0.	更新daemon项目

# DeployNote(1.1.1)
0.	新增daemon项目配置
1.	更新daemon项目

# DeployNote(1.1.0)
0.	更新daemon项目

# DeployNote(1.0.9)
0.	更新daemon项目

# DeployNote(1.0.*)
0. 需要建立数据库jym-daemon, 配置用户db-user-front

# ReleaseNote
- 1.2.33
	- 功能: 增加功能：银行存管融资方流水和平台方无响应流水制为失败
- 1.2.32
	- 功能: 去掉呼叫中心所有内容
- 1.2.31
	- 功能: 增加呼叫中心定时修改用户状态（每天晚上23:50分执行），在插入用户的定时任务时添加了渠道名称字段。
- 1.2.30
	- 功能: 优化好友流水归并Job归并7天内未归并数据
- 1.2.13
	- 功能: 好友奖励流水归并
- 1.2.12
	- 功能: 京东结果轮询重打包
- 1.2.11
	- 功能: 修改好友奖励流水归并job起始时间
- 1.2.10
	- 功能: 删除交易系统跑批程序
- 1.2.9
	- 功能: 修改生日奖励发送Job起始时间
- 1.2.8
	- 功能: 修改一下呼叫中心定时任务时间。
- 1.2.7
	- 功能: 新增京东支付结果查询Job
- 1.2.6
	- 功能: 注释盛付通支付结果查询Job
- 1.2.4
	- 功能: 重新打包 
- 1.2.3
	- 功能: 新增生日奖励发送Job
	- 功能: 新增奖励流水每日归并Job
- 1.2.2
	- 功能: 新增金运通支付结果查询Job
- 1.2.1
	- 功能: 增加呼叫中心导入用户定时任务（每天凌晨1:30分执行）-定时任务名称-CallCenter。
	       增加呼叫中心修改用户定时任务（根据认证时间）（每天凌晨2:00分执行）-定时任务名称-CallCenterUpdate。
		   增加呼叫中心修改手机号码定时任务（每天凌晨2:20分执行）-定时任务名称-CallCenterUpdatePhone。
		   增加呼叫中心修改用户会员等级定时任务（每月28号凌晨2:50分执行）-定时任务名称-CallCenterUpdateLevel。
- 1.1.0
	- 功能: 新增支付系统补单Job
- 1.1.0
	- 优化: 移除PollDoDailyUserInfoSync的Job
- 1.0.7 发布对应新资产后台上线加入新服务
- 1.0.9
	- 优化: 先锋及易联后台轮训只操作7天内处理中数据
- 1.0.6
	- 优化: 内部调用

- 1.0.5
	- 功能: 增加好友返现流水

- 1.0.4
	- 功能: 回滚版本中关于先锋支付的新功能

- 1.0.*
	- 优化: DoDailyWork中用户获取方式
	- 优化: DoDailyWork中调用接口为多线程