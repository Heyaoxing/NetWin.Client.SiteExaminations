using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWin.Client.SiteExamination.D_Data.Base
{
    /// <summary>
    /// 初始化数据库脚本
    /// </summary>
    public abstract class DataScript:DataConfig
    {

        #region 表创建脚本
        /// <summary>
        /// 资源检查记录表(SiteExaminationInfo)
        /// </summary>
        protected const string CreateSiteExaminationInfo = @"create table SiteExaminationInfo (
                                                                    SiteId                   INTEGER                         not null,
                                                                    UserId               BIGINT,
                                                                    SiteUrl              VARCHAR(200)                   not null default '',
                                                                    Score                SMALLINT                        default 100,
                                                                    CompletedOn          TIMESTAMP,
                                                                    CreatedOn            TIMESTAMP,
                                                                    IsCompleted          SMALLINT,
                                                                    CertificateNum       VARCHAR(20),
                                                                    primary key (SiteId)
                                                                    );";

        /// <summary>
        /// 资源检查详细记录表(SiteExaminationDetailInfo)
        /// </summary>
        protected const string CreateSiteExaminationDetailInfo = @"create table SiteExaminationDetailInfo (
                                                                    DetailInfoId         INTEGER                         not null,
                                                                    DetailId             INTEGER,
                                                                    SiteId               INTEGER,
                                                                    IsPass               SMALLINT,
                                                                    Result               VARCHAR(500),
                                                                    Position             LONG VARCHAR,
                                                                    Score                INTEGER,
                                                                    CreatedOn            TIMESTAMP,
                                                                    primary key (DetailInfoId)
                                                                    );";

        /// <summary>
        /// 体检项目配置表(ExaminationItemConfig)
        /// </summary>
        protected const string CreateExaminationItemConfig = @"create table ExaminationItemConfig (
                                                                    ItemId               INTEGER                        not null,
                                                                    Name                 VARCHAR(50),
                                                                    IsEnable             SMALLINT                       not null default 1,
                                                                    primary key (ItemId)
                                                                    );";

        /// <summary>
        /// 体检项目详细配置表(ExaminationItemDetailConfig)
        /// </summary>
        protected const string CreateExaminationItemDetailConfig = @"create table ExaminationItemDetailConfig (
                                                                            DetailId             INTEGER                        not null,
                                                                            ItemId               INTEGER,
                                                                            RuleId               INTEGER,
                                                                            Name                 VARCHAR(50),
                                                                            Score                INTEGER,
                                                                            Require              VARCHAR(100),
                                                                            IsEnable             SMALLINT                       not null default 1,
                                                                            Suggest              LONG VARCHAR,
                                                                            Department           LONG VARCHAR,
                                                                            primary key (DetailId)
                                                                            );";


        /// <summary>
        /// 计算规则配置表(ComputeRuleConfig)
        /// </summary>
        protected const string CreateComputeRuleConfig = @"create table ComputeRuleConfig (
                                                                            RuleId               INTEGER                            not null,
                                                                            ComputeType          SMALLINT,
                                                                            SpiderType            INTEGER,
                                                                            AimsContainText      VARCHAR(200),
                                                                            WingManContainText   VARCHAR(200),
                                                                            JudgeType            INTEGER,
                                                                            JudgeNumber          DECIMAL(2),
                                                                            MatchMessage         VARCHAR(100),
                                                                            primary key (RuleId)
                                                                            );";


        /// <summary>
        /// 所有表,新增表记得需要在此添加
        /// </summary>
        protected static readonly string CreateTable = CreateSiteExaminationInfo + CreateSiteExaminationDetailInfo +
                                                       CreateExaminationItemConfig + CreateExaminationItemDetailConfig +
                                                       CreateComputeRuleConfig;

        #endregion

        #region 初始化数据

        protected const string InsertExaminationItemConfig = @"delete from  ExaminationItemConfig;
                                                               insert into ExaminationItemConfig values(1,'基本信息',1);
                                                               insert into ExaminationItemConfig values(2,'网站优化度',1);";
        protected const string InsertExaminationItemDetailConfig = @"delete from  ExaminationItemDetailConfig;
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (1, 2, 1, 'description', 2, '描述字数需小于80字', 1, '哎呀，您的网站描述有点长呢。官方给出的标准可是在80个字（160个字符）以内哦，要不然不能完整显示，会出现省略号的，记得精简一下描述的话语哈。','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (2, 2, 2, 'keywords', 2, '关键字需大于等于3个', 1, '您的网站keywords中没有包含关键词耶。keywords是网站管理者给网站页面设定的以便用户通过搜索引擎能搜到本网页的词汇，关键词代表了网站的市场定位。网站的首页关键词至关重要。首页网站关键词代表了网站主题内容，内页和栏目页的关键词一般紧扣页面主题，代表的是当前页面或者栏目内容的主体。直接影响网站排名，这可是优化的关键点呢，记得加上哦......','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (3, 1, 3, '网站域名年龄', 1, '>=12Mouth', 1, '域名的注册时间越长，对于搜索引擎收录优化排名更加好哦，在不影响网站战略下，建议您这边选购老域名！','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (4, 1, 4, '网站备案', 1, '网站需备案', 1, '网站一旦备案，可以使用国内空间啦！也不用担心用户访问网速稳慢，不仅收录与优化排名都会有好处的哦！还可以用户对网站信息提高信任度，建议您网站要备案！','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (5, 1, 5, '服务器响应时间', 4, '服务器响应时间小于3秒', 1, '服务器响应时间越快，不仅满足用户访问体验需求，也是搜索引擎与网站建立基础的信任度，利于爬取收录，才能更好的将网站优化发挥极致哦！亲，建议您购买合适国内主机，不宜内存与流量过少！','通俗的说就是网站的打开速度最好是在3秒以内');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (6, 1, 6, '网站页面压缩', 1, '启用网站页面压缩', 1, '为了搜索引擎减少HTTP请求数，用户的页面访问速度，需将代码内容压缩一下哦！这边会将HTTP尽量减少！','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (7, 1, 7, '域名到期时间', 1, '>=12Mouth', 1, '为了网站能够持续稳定的优化，需保证网站的连通率，以及给客户24小时在线的体验感，这边需要您将网站域名续费一年以上 。','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (8, 1, 8, '网站域名后缀', 1, '主流后缀', 1, '亲，选择这七中域名后缀是国家规定为正规域名后缀，而其他域名后缀是国家不认同的哦！只怕到时候国家政府发话禁止使用此域名后缀，那就得不偿失了，建议你从以上七中域名后缀选择哦！','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (9, 1, 9, '网站服务状态码', 1, 200, 1, '200代表正常索引返回，无任何异常。而301一般网站需改版时，设置301状态码，会使页面在线上保持一段时间新旧内容共存的呢！且新版内容已开始收录后，对新旧内容设置301跳转，将旧版内容指向新版对应内容。利于网站首选域的确定，对于同一资源页面多条路径的301重定向有助于URL权重的集中。301能够尽可能的降低网站因改版带来的流量损失，提高用户体验度，同时也会有利于网站优化哦!','通俗的说就是指网页链接状态码，提醒，可以适当的了解一下网页链接返回状态码的每个含义。');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (10, 1, 10, '网站IP及所属地', 1, '大陆IP', 1, '网站的服务器主机一定要选择国内大陆备案的呢！即使香港与台湾的主机也不是长久之计哦！使用大陆备案域名即使网站被涉及被封站点了，也会很容易解决恢复站点呢！建议您使用国内备案之域名。','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (11, 2, 11, 'robots.txt', 2, '有', 1, '您的网站尚未设置robots文件。Robots能够屏蔽网站内的死链接、屏蔽搜索引擎蜘蛛抓取站点内重复内容和页面、阻止搜索引擎索引网站隐私性的内容，对于SEO优化而言，建立robots.txt文件是很有必要的。','网站通过robots协议告诉搜索引擎哪些页面可以抓取，哪些页面不能抓取。');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (12, 2, 12, 'Sitemap.xml', 2, '有', 1, '您尚未设置相应sitemap.xml。sitemap被搜索引擎用来了解网站内部链接层次和结构，确定网站中网站的页面的重要等级。sitemap能让搜索引擎蜘蛛快速了解你网站内部的所有页面URL，从而利于优化。','sitemap中译：网站地图。作用：通知搜索引擎，您的网站上有哪些可以抓取的网页，以便搜索引擎可以更加智能的抓取网站链接。最简单的sitemap形式，就是XML文件。');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (13, 2, 13, 'iframe',1, '不使用iframe', 1, 'iframe搜索引擎无法识别的亲，里面展示的内容不仅无法识别，还会影响网站的加载速度哟~','iframe是网站浮动的框架');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (14, 2, 14, 'flash', 1, '不使用flash', 1, 'flash搜索引擎无法识别的亲，里面展示的内容不仅无法识别，还会影响网站的加载速度哟~','flash是网站视频或者是动画。');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (15, 2, 15, 'title', 2, '<=30', 1, '亲的网站标题太长哟，百度展示页面显示不完整哦，要适当控制一下呢，30字以内最完美了。','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (16, 2, 16, 'js放于body内', 2, 'js不能放于body内', 1, '这样处理的好处是无需担心因页面未完成加载，造成DOM节点获取不到，使脚本报错的问题，而且能避免因脚本运行缓慢造成页面卡死的问题。说白了就是：影响加载速度','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (17, 2, 17, 'H1标签', 5, '建议使用在H1标签', 1, '您的网站尚未设置H1标签。H1标签是网页html 中对文本标题所进行的着重强调的一种标签，作用仅次于Title，建议在页面进行添加。','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (18, 2, 18, 'H2-H6标签', 2, '建议使用H2-H6标签', 1, '您的网站缺乏相应标签，建议进行添加。H标签也叫做Heading标签，在HTML语言里一共有六种大小的heading标签，是网页html 中对文本标题所进行的着重强调的一种标签，以标签h1、h2、h3到h6定义标题头的七个不同文字大小的tags，本质是为了呈现内容结构。','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (19, 2, 19, '空链接', 2, '不能存在空链接', 1, '亲的网站空链接有些多哟，程序员为了省事在编写返回当前页或者未决定链向的代码时，经常用“#”代替链接，这对搜索引擎是不友好的行为哟~','空链接就是无指向链接');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (20, 2, 20, 'URL层级3(含)占比', 5, '总体URL层级3（含）以内占比80%', 1, '您的网站url过长，会加大蜘蛛抓取难度，造成收录效率降低，这样会影响网站的收录的哟！','URL层级一般使用斜杠来区分，一个”/”为一层。斜杠后面带有参数也表示一层级。');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (21, 2, 21, '网站内链数量', 8, '网站内链数量>=100', 1, '您目前的内链数量不符合标准。对于网站的seo优化来讲，内链数量非常重要，在关键词排名当中占据相当大的因素。','网站内链数量指的是站内链接的数量，不包括友情链接。');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (22, 2, 22, '外部链接数量', 13, '外部链接数量>=1000', 1, '友情链接还可以适当增加哟。益友越多，相当于对亲的网站越是认可，搜索引擎也会酌情给予分值。另一方面，多一条网站的入口，能够提高蜘蛛抓取的几率','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (23, 2, 23, 'css文件数', 1, 'css文件不超过4个', 1, '亲的网站CSS文件有点多哟，搜索引擎蜘蛛抓取的时候要去加载，会很累的~','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (24, 2, 24, 'js文件数', 1, 'js文件不超过4个', 1, '亲的网站JS文件有点多哟，搜索引擎蜘蛛抓取的时候要去加载，会很累的~','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (25, 2, 25, 'url静态', 1, 'url静态100%', 1, 'url中有符号例如？、&等这种字符就是动态链接www.zxtrdljz.com/information_complex.aspx?FId=n34:34:34&IsActiveTarge我们建议：网站动态过长，不利于收录和网站排名，建议进行网站静态化处理呢。','URL静态化：http://www.zxtrdljz.com/AboutSt/AboutSt.html <br/>URL动态：url中有符号例如？、&等这种字符就是动态链接');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (26, 2, 26, '网页大小', 2, '网页大小需小于200kb', 1, '搜索引擎抓取一个网页只有200K，超过部分就无法被抓取，请亲适当删除多余代码（注释、内部调用等）哟~','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (27, 2, 27, '各关键词出现次数',3, '各关键词出现次数不低于5次', 1, '你的页面中关键词密度不够哦。要加大关键词的密度哦，要不然网站中的主体内容与关键词离散性太大哦，记得加强呢。','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (28, 2, 28, '首选域', 8, '需设置首选域', 1, '您的网站尚未设置首选域，建议进行首选域设置。如果存在两个不同根域名，指向同一站点，导致镜像站严重，让网站降权，另外对用户体验也有一定的影响。首选域设置方法：1、在域名绑定解析时，只绑定解析一个域名。（中大型站点不建议如此）有两个不同；2、将非首选域，做301重定向到首选域集中权重。','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (29, 2, 29, '网站描述关键词', 2, '描述关键词', 1, '哎呀，描述中没有关键词耶......虽然现在百度搜索引擎排名规则中，网页描述标签的作用越来越弱，但是你要知道搜索引擎的关键词排名是考虑的一个综合性的因素哦，因此不要漏掉任何一个细节哦，况且这是增加关键词密度的重要位置呢，可要记得加上。','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (30, 2, 30, '图片logo包含关键词', 1, '图片logo包含关键词', 1, 'logo是网站的大脑，由于大家打开网站的时候直接就能看到logo。同样蜘蛛抓取也是自上向下，但是不能忽略一点蜘蛛可是无法识别图片的，那么为了告诉蜘蛛logo的内容，添加alt和title属性可是相当重要的，同时这可是做关键词的最佳位置，记得添加一个关键词哦~','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (31, 2, 31, '图片具备alt和title属性含目标关键词', 3, '图片具备alt和title属性含目标关键词', 1, '您的网站中图片没有属性呢，蜘蛛可是无法识别的哟；为了让蜘蛛能够识别您网站图片中的内容，记得加上alt和title属性哈，包含关键词可是最佳的方式哦~','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (32, 2, 32, '页面锚文本链接唯一', 5, '页面锚文本链接唯一', 1, '同页面不能出现相同关键词的锚文本，以及同一页面下不能出现相同链接的锚文本，这样不利于蜘蛛判断链接指向，从而影响网站排名哦。为了关键词排名，记得处理一下哈~','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (33, 2, 33, '网站图片描述', 3, '网站图片描述', 1, '您网站的图片周围没有文字描述哦，百度可是不青睐的呢。百度在百度站长学院中指出：图片周边有可信的、精准的、针对图片的相关描述，包括上下文描述、图片说明、alt属性、图片title，以及图片anchor，这可是官方说明哦，因此记得添加上哦。http://zhanzhang.baidu.com/college/articleinfo?id=204','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (34, 2, 34, 'strong', 2, 'strong', 1, '您的网站缺乏相应标签，建议进行添加。strong标签跟b标签的区别在于，虽然2个都能加粗文本，但是strong还有一种强调重点的意思，权重值比较高。','');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (35, 2, 35, '垃圾页面占比', 8, '垃圾页面占比不超过5%', 1, '您的网站页面垃圾页面过多呢。垃圾页面对于搜索引擎的友好性极差，这难以提升网站的优化效果。甚至会严重影响到网站的优化，进而被降权或者惩罚。因此呢记得处理一下垃圾页面，要不然百度可不喜欢您的网站了呢。','');";

        protected const string InsertComputeRuleConfig = @"delete from  ComputeRuleConfig;
                                                                        INSERT INTO ComputeRuleConfig VALUES (1, 1, 1, 'description', '', 200, 80, ' 描述字数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (2, 1, 1, 'keywords', '', 100, 3, ' 关键字字数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (3, 1, 2, 'domainage', '', 100, 12, '域名年龄为:_COUNT_个月');
                                                                        INSERT INTO ComputeRuleConfig VALUES (4, 1, 2, 'record', '', 100, 1, ' _CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (5, 1, 2, 'responsetime', '', 200, 3000, '服务器响应时间为:_COUNT_毫秒');
                                                                        INSERT INTO ComputeRuleConfig VALUES (6, 1, 2, 'compress', '', 100, 0, '压缩比为:_COUNT_%');
                                                                        INSERT INTO ComputeRuleConfig VALUES (7, 1, 2, 'expiredate', '',100, 12, '_COUNT_月');
                                                                        INSERT INTO ComputeRuleConfig VALUES (8, 1, 2, 'domainsuffix', '', 100, 1, '域名后缀为: _CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (9, 1, 2, 'statuscode', '', 100, 200, '服务器状态码:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (10, 1, 2, 'domainaddress', '', 100, 1, '_CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (11, 1, 2, 'robots', null, 100, 1, '_CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (12, 1, 2, 'sitemap', null, 100, 1, '_CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (13, 1, 1, 'iframe', null, 200, 1, ' iframe出现次数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (14, 1, 1, 'flash', null, 200, 1, ' flash出现次数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (15, 1, 1, 'title', null, 200, 30, ' 标题字数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (16, 1, 1, 'jswithinbody', null, 100, 1, '_CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (17, 2, 1, 'h1', null, 100, 1, ' 出现标签数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (18, 2, 1, 'h2-h6', null, 100, 1, ' h2-h6出现数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (19, 1, 1, 'nulllink', null, 200, 1, ' 存在空链接');
                                                                        INSERT INTO ComputeRuleConfig VALUES (20, 2, 1, 'level', null, 300, 0.8, '_CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (21, 2, 1, 'insidelinkcount', null, 100, 100, '内部链接数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (22, 2, 1, 'outsidelinkcount', null, 100, 1000, '外链数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (23, 1, 1, 'css', null, 200, 5, '  css:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (24, 1, 1, 'js', null, 200,5, '  js:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (25, 2, 1, 'dynamic', null, 100, 1, ' 存在动态链接:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (26, 1, 1, 'size', null, 200, 200, ' 网页大小:_COUNT_ KB');
                                                                        INSERT INTO ComputeRuleConfig VALUES (27, 2, 1, 'keywordtime', null, 100, 5, ' 关键字出现次数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (28, 1, 2, 'preferred', null, 100, 1, '_CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (29, 1, 1, 'desccontainsword', null, 100, 3, ' 描述关键词出现次数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (30, 1, 2, 'logo', null, 100, 1, '_CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (31, 1, 1, 'contentimg', null, 100, 1, ' _CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (32, 1, 1, 'anchorlink', null, 100, 1, ' _CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (33, 1, 1, 'imgdescript', null, 100, 1, ' _CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (34, 1, 1, 'strong', null, 100, 1, ' _CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (35, 2, 1, 'nullsite', null, 300, 0.5, '_CONTENT_');";

        /// <summary>
        /// 初始化插入所有系统数据
        /// </summary>
        protected static readonly string InsertData = InsertExaminationItemConfig + InsertExaminationItemDetailConfig + InsertComputeRuleConfig;

        #endregion
    }
}
