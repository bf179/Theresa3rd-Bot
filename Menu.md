# 使用方法

## pixiv涩图
### 环境
- 首先需要一个能翻墙的运行环境，本人使用的是[自由鲸](https://www.freewhale.us/auth/register?code=sQAT)(原心阶)，邀请码为sQAT

### 设置cookie
- 初次使用时需要设置cookie，使用一个平时较少使用的账号登录[pixiv](https://www.pixiv.net)，然后获取cookie，cookie中必须包含PHPSESSID
- pc端登录pixiv后按下F12，在p站中随意搜索一个标签，在网络中找到如下请求，这里以搜索Hololive为例
![image](https://user-images.githubusercontent.com/89188316/153154862-8785396e-414a-4f2d-bba3-f7ca8c34f144.png)
- 使用 #pixivcookie+获取到的cookie 格式私聊发送给机器人
![image](https://user-images.githubusercontent.com/89188316/153157373-047aa094-483f-4051-9833-ca6af15698ff.png)
- cookie需要定期更新，在获取色图失败时可以先尝试更新cookie

### 指令
- 使用 #涩图 可以根据配置获取随机涩图
- 使用 #涩图+自定义标签 可以随机搜索一张该标签的涩图
- 使用 #涩图+作品id 可以根据id获取涩图
- 如果获取到的作品为动图时，会自动转换为gif，可以使用 #涩图+作品id 转换自己喜欢的动图
![image](https://user-images.githubusercontent.com/89188316/153163179-cab64f76-8b5b-47b5-a59b-099168d8a995.png)
![image](https://user-images.githubusercontent.com/89188316/153164054-604ad40e-d272-4652-923b-88fd45d911d8.png)
![image](https://user-images.githubusercontent.com/89188316/153159925-d0dff1cd-0e26-4be1-9870-c16d57ea01b5.png)

## Lolicon瑟图
### 指令
- 使用 #瑟图 可以根据随机获取一张Lolicon图床中的瑟图
- 使用 #瑟图+自定义标签 可以随机搜索一张该标签的瑟图，多标签可以使用逗号或者空格分割，可以进行多标签搜索
![image](https://user-images.githubusercontent.com/89188316/153168163-bb47b63d-bbbb-4ab9-8007-2e33f9a5c13d.png)
![image](https://user-images.githubusercontent.com/89188316/153169798-ce49c3be-154c-48fd-9a99-e991430c682a.png)

## 禁止涩图
### 说明
- 将一个标签加入到禁止搜索列表中，防止群友整活，匹配方式为完全一致
![image](https://user-images.githubusercontent.com/89188316/153175892-80e31abe-cbf7-4485-bfb1-bc7370f8c06d.png)

### 指令
- 使用 #禁止标签+关键词 禁止一个标签
- 使用 #解禁标签+关键词 解除一个标签
![image](https://user-images.githubusercontent.com/89188316/153180083-4bf06489-a5df-48ee-84f6-56805a60e007.png)


## 订阅pixiv画师
### 说明
- 使用轮询的方式定时扫描画师的最新作品，并将作品自动推送到qq群，r18类的作品将会被忽略
![image](https://user-images.githubusercontent.com/89188316/153171928-b9e90263-5351-41a4-824f-6a999feca886.png)

### 获取画师id
- 在pixiv网页版中点开画师头像后，网页地址中 https://www.pixiv.net/users/15034125 的 15034125 为画师id

### 指令
- 使用 #订阅画师+画师id 订阅一个画师
- 使用 #退订画师+画师id 退订一个画师
![image](https://user-images.githubusercontent.com/89188316/153182768-f7ffd4a2-eb46-424c-8126-347b737fde11.png)
![image](https://user-images.githubusercontent.com/89188316/153170330-ecb886e6-2e59-4423-bfb1-d122001ea1fc.png)

## 订阅pixiv标签
### 说明
- 使用轮询的方式定时扫描标签的最新作品，并将作品自动推送到qq群，r18类的作品将会被忽略，匹配方式为部分一致
![image](https://user-images.githubusercontent.com/89188316/153169722-389c2058-a54f-46e6-9004-c9073498f0b9.png)

### 指令
- 使用 #订阅标签+关键词 订阅一个标签
- 使用 #退订标签+关键词 退订一个标签
![image](https://user-images.githubusercontent.com/89188316/153172783-c09563f7-2bf7-4d54-b112-1f539b69e7fe.png)
![image](https://user-images.githubusercontent.com/89188316/153180473-a9065289-1ada-4a04-83a2-8920313dba2c.png)

