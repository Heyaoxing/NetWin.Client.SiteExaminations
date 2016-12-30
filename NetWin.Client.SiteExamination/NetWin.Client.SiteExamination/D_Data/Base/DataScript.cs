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
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (1, 2, 1, 'description', 5, '描述字数需小于80字', 1, '描述字数小于80字');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (2, 2, 2, 'keywords', 5, '关键字需大于等于3个', 1, '关键字增加到3个以上');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (3, 1, 3, '网站域名年龄', 5, '>=12Mouth', 1, '>=12Mouth');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (4, 1, 4, '网站备案', 5, '网站需备案', 1, '网站需备案');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (5, 1, 5, '服务器响应时间', 5, '服务器响应时间小于3秒', 1, '服务器响应时间小于3秒');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (6, 1, 6, '网站页面压缩', 5, '启用网站页面压缩', 1, '启用网站页面压缩');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (7, 1, 7, '域名到期时间', 5, '>=12Mouth', 1, '>=12Mouth');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (8, 1, 8, '网站域名后缀', 5, '主流后缀', 1, '主流后缀');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (9, 1, 9, '网站服务状态码', 5, 200, 1, 200);
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (10, 1, 10, '网站IP及所属地', 5, '大陆IP', 1, '大陆IP');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (11, 2, 11, 'robots.txt', 5, '有', 1, '有');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (12, 2, 12, 'Sitemap.xml', 5, '有', 1, '有');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (13, 2, 13, 'iframe', 5, '不使用iframe', 1, '不使用iframe');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (14, 2, 14, 'flash', 5, '不使用flash', 1, '不使用flash');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (15, 2, 15, 'title', 5, '<=30', 1, '<=30');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (16, 2, 16, 'js放于body内', 5, 'js不能放于body内', 1, 'js不能放于body内');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (17, 2, 17, 'H1标签', 5, '建议使用在H1标签', 1, '建议使用H1标签');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (18, 2, 18, 'H2-H6标签', 5, '建议使用H2-H6标签', 1, '建议使用H2-H6标签');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (19, 2, 19, '空链接', 5, '不能存在空链接', 1, '不能存在空链接');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (20, 2, 20, 'URL层级3(含)占比', 5, '总体URL层级3（含）以内占比80%', 1, '总体URL层级3（含）以内占比80%');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (21, 2, 21, '网站内链数量', 5, '网站内链数量>=100', 1, '网站内链数量>=100');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (22, 2, 22, '外部链接数量', 5, '外部链接数量>=1000', 1, '外部链接数量>=1000');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (23, 2, 23, 'css文件数', 5, 'css文件不超过4个', 1, 'css文件不超过4个');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (24, 2, 24, 'js文件数', 5, 'js文件不超过4个', 1, 'js文件不超过4个');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (25, 2, 25, 'url静态', 5, 'url静态100%', 1, 'url静态100%');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (26, 2, 26, '网页大小', 5, '网页大小需小于200kb', 1, '网页大小需小于200kb');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (27, 2, 27, '各关键词出现次数', 5, '各关键词出现次数不低于5次', 1, '各关键词出现次数不低于5次');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (28, 2, 28, '首选域', 5, '需设置首选域', 1, '需设置首选域');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (29, 2, 29, '描述关键词', 5, '描述关键词', 1, '描述关键词');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (30, 2, 30, '图片logo包含关键词', 5, '图片logo包含关键词', 1, '图片logo包含关键词');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (31, 2, 31, '图片具备alt和title属性含目标关键词', 5, '图片具备alt和title属性含目标关键词', 1, '图片具备alt和title属性含目标关键词');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (32, 2, 32, '页面锚文本链接唯一', 5, '页面锚文本链接唯一', 1, '页面锚文本链接唯一');
                                                                        INSERT INTO ExaminationItemDetailConfig VALUES (33, 2, 33, '网站图片描述', 5, '网站图片描述', 1, '图片周围需要有文字描述');";

        protected const string InsertComputeRuleConfig = @"delete from  ComputeRuleConfig;
                                                                        INSERT INTO ComputeRuleConfig VALUES (1, 1, 1, 'description', '', 100, 88, '描述字数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (2, 1, 1, 'keywords', '', 100, 3, '关键字字数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (3, 1, 2, 'domainage', '', 200, 12, '域名年龄为:_COUNT_个月');
                                                                        INSERT INTO ComputeRuleConfig VALUES (4, 1, 2, 'record', '', 200, 1, ' _CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (5, 1, 2, 'responsetime', '', 100, 3, '服务器响应时间为:_COUNT_秒');
                                                                        INSERT INTO ComputeRuleConfig VALUES (6, 1, 2, 'compress', '', 200, 0, '压缩比为:_COUNT_%');
                                                                        INSERT INTO ComputeRuleConfig VALUES (7, 1, 2, 'expiredate', '', 200, 12, '_COUNT_月');
                                                                        INSERT INTO ComputeRuleConfig VALUES (8, 1, 2, 'domainsuffix', '', 200, 1, '域名后缀为: _CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (9, 1, 2, 'statuscode', '', 200, 200, '服务器状态码:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (10, 1, 2, 'domainaddress', '', 200, 1, '_CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (11, 1, 2, 'robots', null, 200, 1, '不存在robots.txt');
                                                                        INSERT INTO ComputeRuleConfig VALUES (12, 1, 2, 'sitemap', null, 200, 1, '不存在sitemap.xml');
                                                                        INSERT INTO ComputeRuleConfig VALUES (13, 1, 1, 'iframe', null, 100, 1, '存在iframe');
                                                                        INSERT INTO ComputeRuleConfig VALUES (14, 1, 1, 'flash', null, 100, 1, '存在flash');
                                                                        INSERT INTO ComputeRuleConfig VALUES (15, 1, 1, 'title', null, 100, 30, '标题字数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (16, 1, 1, 'jswithinbody', null, 100, 1, '存在js放于body内');
                                                                        INSERT INTO ComputeRuleConfig VALUES (17, 2, 1, 'h1', null, 200, 1, 'h1标签出现数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (18, 2, 1, 'h2-h6', null, 100, 1, '_CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (19, 1, 1, 'nulllink', null, 100, 1, '存在空链接');
                                                                        INSERT INTO ComputeRuleConfig VALUES (20, 2, 1, 'level', null, 400, 0.8, '总体URL层级3（含）以内占低于80%');
                                                                        INSERT INTO ComputeRuleConfig VALUES (21, 2, 1, 'insidelinkcount', null, 200, 100, '内部链接数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (22, 2, 1, 'outsidelinkcount', null, 200, 1000, '外链数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (23, 1, 1, 'css', null, 100, 4, 'css:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (24, 1, 1, 'js', null, 100, 4, 'js:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (25, 2, 1, 'dynamic', null, 100, 1, '存在动态链接:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (26, 1, 1, 'size', null, 100, 200, '网页大小:_COUNT_KB');
                                                                        INSERT INTO ComputeRuleConfig VALUES (27, 2, 1, 'keywordtime', null, 200, 5, '关键字出现次数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (28, 1, 2, 'preferred', null, 100, 1, '首选域:_CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (29, 1, 1, 'desccontainsword', null, 200, 3, '描述关键词出现次数:_COUNT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (30, 1, 2, 'logo', null, 100, 1, '图片logo中的alt或title属性包含关键字');
                                                                        INSERT INTO ComputeRuleConfig VALUES (31, 1, 1, 'contentimg', null, 200, 1, ' _CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (32, 1, 1, 'anchorlink', null, 100, 1, ' _CONTENT_');
                                                                        INSERT INTO ComputeRuleConfig VALUES (33, 1, 1, 'imgdescript', null, 100, 1, ' _CONTENT_');";

        /// <summary>
        /// 初始化插入所有系统数据
        /// </summary>
        protected static readonly string InsertData = InsertExaminationItemConfig + InsertExaminationItemDetailConfig + InsertComputeRuleConfig;

        #endregion
    }
}
