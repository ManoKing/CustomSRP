# UnityFramework
框架为项目中提供基础功能，如资源管理、UI框架、网络通信、消息管理、场景管理、数据解析及存取等，同时定义了一系列规范包括编码的，例如参数命名、注释、缩进等  
以及行为准则，例如加载场景必须用框架的xxx接口，贴图必须放在xxx文件夹下等  
框架包含内容如下：  
#### （1）框架是由多个不同的模块组成的，需要添加模块的管理类ModuleManager，该类负责各个模块的初始化、维护模块实例的引用窗口，以及Update函数负责模块的更新  
#### （2）我们打开游戏以后，用户需要更新、登录以后才能体验游戏的内容，所以我们要有一个登录模块LoginModule，该模块负责从进入游戏main场景前的加载和登录过程  
LoginModule需要完成按照顺序如下：  
1.建立网络连接，如果没有网络弹出提示框，否则跳转到步骤2  
2.扩展包更新，需要先检查扩展包是否需要更新，如果要更新，检查内存是否足够，内存不足弹出提示框，否则跳转步骤3  
3.解压扩展包，如果要解压，检查内存是否足够，内存不足弹出提示框，否则跳转4  
4.热更新  
1）获取服务端版本号文件，如果需要强更，跳转到应用商店，如果需要更新，跳转下一步，否则跳转步骤5  
2）获取服务端md5文件，对比本地md5，获取需要更新的ab包列表  
3）下载并解压需要更新的ab包，这边需要检查内存是否足够，内存不足弹出提示框  
5）更新结束。登录sdk如腾讯账号，获取账号状态，如封号，弹出提示，否则跳转步骤6  
6）登录游戏服务器（简称游服），获取游戏状态，如在维护状态，弹出提示，否则跳转步骤7  
7）新手引导（顺序跟步骤8可以对换。看情况）  
8）预加载资源（可无，看情况），预加载结束跳转步骤9  
9）请求服务端初始化数据（如main场景要显示内容需要的数据、小红点等），初始数据都接收完毕跳转步骤10  
10）切换到mian场景，打开主界面  
#### （3）sdk模块。游戏更新完毕后，我们开始登录游戏了。SDKModule模块负责的内容包括：  
1.账号类：创建、登录、切换  
2.充值  
3.外部分享  
4.打开外部链接，如论坛、社区等  
#### （4）sdk登录完毕，通过sdk返回的用户信息前面连接我们的游服  
这就需要一个专门的模块Socket负责和游服的通信，包括网络的连接、消息的接收、心跳包的发送、断线重连的监听和处理  
#### （5）用户登录结束了，我们可以进入主城了，这时候就需要用到场景模块SceneModule切换场景了，SceneModule主要完成以下工作  
　1.场景的加载、卸载、切换  
　2.加载新的场景时需要卸载旧场景的的资源，清除GC  
　3.支持场景资源的预加载  
#### （6）前面说到的预加载资源，用什么来加载呢，我们需要一个资源管理器LoadModule,这是最主要的一个模块，我们来看看该模块的功能  
1.加载资源，包括从本地加载、从ab包加载、从网络下载，而ab包的加载是其中最麻烦的地方了（ab包依赖、冗余）  
2.ab包的加载，包括mainfest的加载和依赖的加载  
3.资源、ab包的缓存和卸载：引入引用计数和销毁计时（没有资源引用且非常驻资源且销毁计时结束接可以回收了）  
4.不同资源类型的加载。如图片、音频、预制体、场景、配置表等  
#### （7）终于进入内城了，是时候打开ui界面了，UIModule用于管理我们的ui界面，该模块需要实现以下功能：  
1.界面的加载、卸载  
2.打开、关闭、隐藏、显示界面，这边隐藏是指界面被遮挡的意思，一般来说，界面被遮住时，应该关闭界面的更新  
3.界面栈的管理，主要是用于场景切换时需要回到上一个场景打开的界面栈  
4.界面的基类：弹窗、全屏窗口  
5.需要的功能：图片镜像（节省资源）、滑动列表（复用）、模糊背景等  
#### （8）前面说到的资源需要复用，UI需要复用，那就需要一个通用的对象池用来管理我们的对象啦  
#### （9）我们都知道MVC框架，核心是分隔view和module层，在我们的框架就是view和view之间，View和module之间不直接引用对象，那要怎么通信的，这时候就需要一个消息机制了  
1.添加、移除事件的方法  
2.需要触发事件的方法  
3.Lua端的消息机制  
4.c#端的消息机制  
#### （10）程序员嘛，就是来写bug的。有bug不可怕，没日志就难受了，一个好的日志系统可以帮助我们更快速的定位到bug的位置。  
1.日志开关。只有开发版本开启,因为日志还是比较耗性能的  
2.堆栈日志界面：ERROR时弹出界面，该界面显示错误的堆栈日志。方便qa测试时查看日志  
3.FPS帧率的显示  
4.游戏正式上线以后，我们很难拿到用户的错误日志，这时候我们需要把错误的日志上传到我们的服务器  
5.当游戏崩溃时我们是拿不到unity打印的日志的，这时候就需要接入FireBase了，它可以帮我们把崩溃的详细日志上传到网页  
#### （11）音乐模块，用于管理音乐（bgm、音效）的播放、暂停，因为音乐文件有打ab包，所以需统一的接口去管理  
#### （12）用户设置，例如王者荣耀里，当你选择一个英雄并进入游戏后，下一次选择英雄他会记住你上一次的出装、天赋、皮肤等内容，还有就是玩家可以打开、关闭游戏音效  
1.本地设置，设置存储在本地，更换设备就失效了，一般存成\json\xml或unity的PlayerPrefs  
2.保存服务端，设置上传到服务器，这样更换设备也不影响设置  
#### （13）终于，终于我们游戏做好了，要打包了，自动打包走起 
1.命令行参数解析，命令的参数包括平台、是否打ab包、是否重新导出、版本号、版本类型（develop\release）等等你想配置的参数  
2.设置图集名字，防止项目成员不按规则设置图集  
3.设置ab包名字、打ab包、压缩ab包（热更包过大用户容易流失）  
4.生成md5，对比旧的md5，上传需要更新的ab包和新的md5文件到服务器  
5.导出unity工程  
6.打包。ios使用xcode打ipa，android使用android studio打apk  
7.发布版本，测试包发布到叮叮群，正式包由运营上传到应用商店（运营很棒）  
#### （14）编辑器工具，工欲善其事，必先利其器，所以工具必不可少  
1.图集检查工具、图集名字设置工具  
2.ab包名字、依赖检查工具  
3.资源导入检查设置  
4.配置、服务器协议同步工具  
#### （15）性能优化  
1.CPU  
2.GPU  
3.ui优化，包括dc和界面打开速度  
4.uwa接入  

#### 热更方式
代码热更选择了ILRuntime，是因为主流热更方式如下  
（1）XLua如果选用C#开发，打补丁热更方式很麻烦，而且补丁Lua还需要用C#再重新写一遍，实际项目中补丁方式往往不能实现，需要重新打包，造成用户丢失。  
（2）ToLua或其他方式，选择使用Lua开发，考虑到统一语言，未采用  


